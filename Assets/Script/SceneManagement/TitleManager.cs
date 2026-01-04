using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button textButton;
    [SerializeField] private Canvas buttonCanvas;
    [SerializeField] private Text fadeText;
    [Header("Input")]
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private string actionMapName = "GamePlay";
    [SerializeField] private string submitActionName = "Submit";
    private InputAction submit;
    private bool opened = false;
    private bool flag = false;

    private void OnEnable()
    {
        //入力をバインド
        if (actions != null)
        {
            var map = actions.FindActionMap(actionMapName, throwIfNotFound: false);
            submit = map?.FindAction(submitActionName, throwIfNotFound: false);
            map?.Enable();
        }
    }

    private void OnDisable()
    {
        //Title抜けるときにDisable（他シーンと干渉しにくい）
        submit?.actionMap?.Disable();
    }

    private void Start()
    {
        startButton.onClick.AddListener(SceneChanger.ToStart);
        settingButton.onClick.AddListener(SceneChanger.ToSettings);
        exitButton.onClick.AddListener(SceneChanger.ToExit);
        textButton.onClick.AddListener(CanvasSwitch);
    }

    private void Update()
    {
        if (opened) return;
        if (submit != null && submit.triggered)
        {
            CanvasSwitch();
            flag = true;
        }
    }

    private void CanvasSwitch()
    {
        if (textButton != null) textButton.gameObject.SetActive(false);
        if (buttonCanvas != null) buttonCanvas.gameObject.SetActive(true);
    }
}
