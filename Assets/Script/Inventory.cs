using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Image itemImage = null;
    [SerializeField] private Text itemName = null;
    [SerializeField] private Text itemValue = null;
    [SerializeField] private ItemDataBase itemDataBase = null;
    private PlayerInput playerInput;
    private InputAction prevItemAction;
    private InputAction nextItemAction;
    private ItemData itemData;
    private int itemNumber;

    private void Awake()
    {
        // Playerが後から生成される可能性があるので、まずは拾えるなら拾う
        TryBind();
    }

    private void OnEnable()
    {
        // 生成タイミングが遅いとAwakeで見つからないので、表示タイミングでも拾う
        TryBind();
    }

    private void TryBind()
    {
        if (playerInput != null && prevItemAction != null && nextItemAction != null) return;

        playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput == null) return;

        prevItemAction = playerInput.actions["PrevItem"];
        nextItemAction = playerInput.actions["NextItem"];
    }

    void Start()
    {
        itemNumber = 0;
        SetInventory();
    }

    void Update()
    {
        if (prevItemAction == null || nextItemAction == null)
        {
            TryBind();
            return;
        }

        //z:前へ
        if (prevItemAction.triggered)
        {
            if (itemNumber > 0) itemNumber -= 1;
            SetInventory();
        }

        //x:次へ
        else if (nextItemAction.triggered)
        {
            int max = itemDataBase.GetItemLists().Count - 1;
            if (itemNumber < max) itemNumber += 1;
            SetInventory();
        }
    }

    void SetInventory()
    {
        itemData = itemDataBase.GetItemLists()[itemNumber];
        itemImage.sprite = itemData.GetItemImage();
        itemName.text = itemData.GetItemName();
        itemValue.text = itemData.GetItemValue();
    }
}
