using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour 
{
    [SerializeField] private Button startButton;
	[SerializeField] private Button settingButton;
	[SerializeField] private Button exitButton;
	void Start () 
	{
		startButton.onClick.AddListener(SceneChanger.FromTitleToStart);
		settingButton.onClick.AddListener(SceneChanger.FromTitleToSettings);
		exitButton.onClick.AddListener(SceneChanger.FromTitleToExit);
	}
}
