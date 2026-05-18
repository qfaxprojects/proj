using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed;

    Rigidbody2D rb;
    InputAction moveAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        Debug.Log("[qfaxas]" + moveAction);
        Debug.Log("[qfaxas]" + rb);
    }

    private void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.velocity = new Vector2(moveValue.x * moveSpeed * Time.deltaTime, moveValue.y * moveSpeed * Time.deltaTime);
        Debug.Log("[qfaxas]" + moveValue);
    }

}
