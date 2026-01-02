using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputRouter : MonoBehaviour
{
    public Vector2Int MoveDir { get; private set; }
    public bool SubmitPressed { get; private set; }
    public bool InventoryPressed { get; private set; }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        var v = ctx.ReadValue<Vector2>();

        // ローグ向け：斜め入力は強い方を採用
        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            MoveDir = new Vector2Int(v.x > 0 ? 1 : (v.x < 0 ? -1 : 0), 0);
        else
            MoveDir = new Vector2Int(0, v.y > 0 ? 1 : (v.y < 0 ? -1 : 0));
    }

    public void OnSubmit(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) SubmitPressed = true;
    }

    public void OnInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) InventoryPressed = true;
    }

    public void Consume()
    {
        MoveDir = Vector2Int.zero;
        SubmitPressed = false;
        InventoryPressed = false;
    }
}
