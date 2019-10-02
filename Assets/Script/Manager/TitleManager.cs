using UnityEngine;
using UnityEngine.UI;
using InputKey;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button loadButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button textButton;
    [SerializeField] private Canvas buttonCanvas;
    [SerializeField] private Text fadeText;
    private bool flag = false;
    private void Start()
    {
        startButton.onClick.AddListener(SceneChanger.ToStart);
        settingButton.onClick.AddListener(SceneChanger.ToSettings);
        exitButton.onClick.AddListener(SceneChanger.ToExit);
        textButton.onClick.AddListener(CanvasSwitch);
    }

    private void Update()
    {
        if (flag == false)
        {
            if (InputManager.GridInputKeyDown(KeyCode.Space))
            {
                CanvasSwitch();
                flag = true;
            }
        }
    }

    private void CanvasSwitch()
    {
        textButton.gameObject.SetActive(false);
        buttonCanvas.gameObject.SetActive(true);

    }
}
