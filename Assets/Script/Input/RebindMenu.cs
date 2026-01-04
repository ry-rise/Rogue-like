using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class RebindMenu : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerInput playerInput; // Settingsシーン内のPlayerInput（後述）

    [Header("UI")]
    [SerializeField] private Text submitLabel;
    [SerializeField] private Text prevLabel;
    [SerializeField] private Text nextLabel;
    [SerializeField] private Text statusLabel; // "キーを押してください..." 表示用（任意）

    [SerializeField] private Button submitRebindButton;
    [SerializeField] private Button prevRebindButton;
    [SerializeField] private Button nextRebindButton;
    [SerializeField] private Button resetButton;

    // 今回は「各アクションの1個目のキーボードバインド」を変更対象にする
    private const string MAP = "GamePlay";
    private const string ACT_SUBMIT = "Submit";
    private const string ACT_PREV = "PrevItem";
    private const string ACT_NEXT = "NextItem";

    private InputActionRebindingExtensions.RebindingOperation op;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = Object.FindFirstObjectByType<PlayerInput>();

        // 保存済みがあれば適用（念のため）
        //InputBindingStore.ApplyTo(playerInput);

        //SettingData.jsonから復元
        DataManager.SettingsDataLoad(null, playerInput.actions);

        // ボタン配線（InspectorでOnClick繋がなくても動く）
        if (submitRebindButton) submitRebindButton.onClick.AddListener(() => StartRebind(ACT_SUBMIT));
        if (prevRebindButton) prevRebindButton.onClick.AddListener(() => StartRebind(ACT_PREV));
        if (nextRebindButton) nextRebindButton.onClick.AddListener(() => StartRebind(ACT_NEXT));
        if (resetButton) resetButton.onClick.AddListener(ResetAll);

        RefreshLabels();
    }

    private void OnDisable()
    {
        op?.Dispose();
        op = null;
    }

    private void SetStatus(string msg)
    {
        if (statusLabel) statusLabel.text = msg;
    }

    private void SetButtons(bool enabled)
    {
        if (submitRebindButton) submitRebindButton.interactable = enabled;
        if (prevRebindButton) prevRebindButton.interactable = enabled;
        if (nextRebindButton) nextRebindButton.interactable = enabled;
        if (resetButton) resetButton.interactable = enabled;
    }

    private InputAction GetAction(string actionName)
    {
        var map = playerInput.actions.FindActionMap(MAP, true);
        map.Enable();
        return map.FindAction(actionName, true);
    }

    private int FindFirstKeyboardBindingIndex(InputAction action)
    {
        // composite（2DVector等）は飛ばして、最初のキーボード割り当てを探す
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            if (b.isComposite || b.isPartOfComposite) continue;
            if (!string.IsNullOrEmpty(b.effectivePath) && b.effectivePath.Contains("Keyboard"))
                return i;
        }
        // 見つからなければ最初の「単体」バインド
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var b = action.bindings[i];
            if (b.isComposite || b.isPartOfComposite) continue;
            return i;
        }
        return -1;
    }

    public void StartRebind(string actionName)
    {
        if (playerInput == null) return;

        op?.Dispose();
        op = null;

        var action = GetAction(actionName);
        int bindingIndex = FindFirstKeyboardBindingIndex(action);
        if (bindingIndex < 0)
        {
            SetStatus("バインドが見つかりません");
            return;
        }

        SetButtons(false);
        SetStatus("割り当てたいキーを押してください（キャンセル: Esc）");

        action.Disable();

        op = action.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .OnComplete(o =>
            {
                o.Dispose();
                op = null;

                action.Enable();
                DataManager.SettingsDataSave(null, playerInput.actions);
                RefreshLabels();

                SetStatus("変更しました！");
                SetButtons(true);
            })
            .OnCancel(o =>
            {
                o.Dispose();
                op = null;

                action.Enable();
                SetStatus("キャンセルしました");
                SetButtons(true);
            });

        op.Start();
    }

    private void ResetAll()
    {
        if (playerInput == null) return;

        // バインド上書きを消す
        playerInput.actions.RemoveAllBindingOverrides();

        // SettingsData.json の bindingOverridesJson も空で保存（= デフォルトへ）
        DataManager.SettingsDataSave(null, playerInput.actions);

        RefreshLabels();
        SetStatus("デフォルトに戻しました");
    }

    private void RefreshLabels()
    {
        if (playerInput == null) return;

        submitLabel.text = Display(ACT_SUBMIT);
        prevLabel.text = Display(ACT_PREV);
        nextLabel.text = Display(ACT_NEXT);
    }

    private string Display(string actionName)
    {
        var action = GetAction(actionName);
        int i = FindFirstKeyboardBindingIndex(action);
        if (i < 0) return "-";
        return action.GetBindingDisplayString(i);
    }
}
