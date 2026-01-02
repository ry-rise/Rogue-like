using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject InventoryScreen;
    [SerializeField] private GameObject Header;
    [SerializeField] private Text LevelText;
    [SerializeField] private Text FloorText;
    [SerializeField] private Text HPText;
    [SerializeField] private Text SatietyText;
    [SerializeField] private Text StateText;
    private Player player;
    private PlayerInputRouter input;
    private bool isQuitting = false;
    private void Awake()
    {
        // Inspector未設定チェック（必要最低限）
        if (InventoryScreen == null) Debug.LogError("[UIManager] InventoryScreen が未設定");
        if (Header == null) Debug.LogError("[UIManager] Header が未設定");
        if (LevelText == null) Debug.LogError("[UIManager] LevelText が未設定");
        if (FloorText == null) Debug.LogError("[UIManager] FloorText が未設定");
        if (HPText == null) Debug.LogError("[UIManager] HPText が未設定");
        if (SatietyText == null) Debug.LogError("[UIManager] SatietyText が未設定");
        if (StateText == null) Debug.LogError("[UIManager] StateText が未設定");

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("[UIManager] Player タグのオブジェクトが見つかりません");
            enabled = false;
            return;
        }

        player = playerObj.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("[UIManager] Player コンポーネントが見つかりません");
            enabled = false;
            return;
        }

        input = player.GetComponent<PlayerInputRouter>();
        if (input == null)
        {
            Debug.LogError("[UIManager] PlayerInputRouter が見つかりません");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        StateTextChanger();
    }

    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void Update()
    {
        if (isQuitting) return;
        if (!enabled) return;
        if (player == null) return;

        if (LevelText != null) LevelText.text = $"LV:{player.Level.ToString()}";
        if (FloorText != null) FloorText.text = $"{GameManager.GetFloorNumber().ToString()}F";
        if (HPText != null) HPText.text = $"HP:{player.HP.ToString()}/{player.MaxHP.ToString()}";
        if (SatietyText != null) SatietyText.text = $"空腹度:{player.Satiety.ToString()}";

        //状態表示
        if (GameManager.Instance != null && GameManager.Instance.turnManager == GameManager.TurnManager.StateJudge)
        {
            StateTextChanger();
        }

        //Iキーを押すとインベントリが表示/非表示
        if (input.InventoryPressed)
        {
            if (InventoryScreen == null || Header == null) return;

            bool open = !InventoryScreen.activeSelf;
            InventoryScreen.SetActive(open);
            Header.SetActive(!open);
            if (GameManager.Instance != null) GameManager.Instance.GamePause = open;

            input.Consume(); //トグルが1回で止まれないようにクリア
        }
    }

    private void StateTextChanger()
    {
        if (StateText == null || player == null) return;
        switch (player.state)
        {
            case MoveObject.STATE.NONE:
                StateText.text = "状態異常なし";
                break;
            case MoveObject.STATE.PARALYSIS:
                StateText.text = "麻痺";
                break;
            case MoveObject.STATE.POISON:
                StateText.text = "毒";
                break;
        }
    }
}
