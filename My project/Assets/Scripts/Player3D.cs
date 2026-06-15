using UnityEngine;
using UnityEngine.InputSystem;

public class Player3D : MonoBehaviour
{
    
    public float moveSpeed;
    public float sensi;

    [Tooltip("Colocar o mapa de input inteiro aq")]
    public InputActionAsset Inputs;

    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_menuAction;

    private Rigidbody rb;


    private void OnEnable()
    {
        Inputs.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        Inputs.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        m_moveAction = InputSystem.actions.FindAction("Move");
        m_lookAction = InputSystem.actions.FindAction("Look");
        m_menuAction = InputSystem.actions.FindAction("Menu");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Vector2 moveValue = m_moveAction.ReadValue<Vector2>();
        Vector2 mouseValue = m_lookAction.ReadValue<Vector2>();

        rb.MovePosition(rb.position + transform.forward * moveValue.y * moveSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + transform.right * moveValue.x * moveSpeed * Time.deltaTime);

        Quaternion gira = Quaternion.Euler(0f, mouseValue.x * sensi * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * gira);

        if (m_menuAction.WasPressedThisFrame())
        { 
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
