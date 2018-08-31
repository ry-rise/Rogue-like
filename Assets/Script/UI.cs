using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    [SerializeField] private GameObject LevelText;
    [SerializeField] private GameObject FloarText;
	void Start () {
        FloarText.AddComponent<Text>();
	}
	
	void Update () {
		
	}

}
