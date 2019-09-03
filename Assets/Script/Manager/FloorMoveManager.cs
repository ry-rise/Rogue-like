using UnityEngine;
using UnityEngine.UI;

public class FloorMoveManager : MonoBehaviour
{
    public int viewFloorNumber{get;set;}
	[SerializeField] private Text floorNumberText;

    private FadeManager fadeManager;
    private void Start()
    {
        viewFloorNumber=GameManager.GetFloorNumber();
        floorNumberText.text=$"{viewFloorNumber}F";
        fadeManager = gameObject.GetComponent<FadeManager>();
        fadeManager.isFadeIn = true;
        //fadeManager.StartFadeIn();
        SceneChanger.ToStart();
    }

}
