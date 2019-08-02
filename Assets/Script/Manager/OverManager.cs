using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverManager : MonoBehaviour
{
    [SerializeField] private Button OKButton;
    private void Start()
    {
        OKButton.onClick.AddListener(SceneChanger.FromOverToTitle);
    }
	
}
