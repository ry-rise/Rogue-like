using UnityEngine;
using UnityEngine.UI;

public class FloorMoveManager : MonoBehaviour
{
    public static int viewFloorNumber;
	[SerializeField] private Text floorNumberText;

    private FadeManager fadeManager;
    private void Start()
    {
        viewFloorNumber=GameManager.GetFloorNumber();
        fadeManager = gameObject.GetComponent<FadeManager>();
        fadeManager.isFadeIn = true;
		floorNumberText.text=$"{viewFloorNumber}F";
    }

}
