using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player3D : MonoBehaviour
{
    
    public float moveSpeed;
    public float sensi;

    [SerializeField] Animator _animator;

    [Tooltip("Colocar o mapa de input inteiro aq")]
    public InputActionAsset Inputs;


    private InputAction m_moveAction;
    private InputAction m_lookAction;
    private InputAction m_menuAction;
    private InputAction m_Lidar;
    private InputAction m_mao1;
    private InputAction m_mao2;

    [SerializeField] private AudioSource passo;
    [SerializeField] private float cd_passo = 2;

    private Rigidbody rb;

    private Vector2 moveValue;
    private Vector2 mouseValue;

    private Camera _cam;
    private float offcam = 0f;

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
        m_Lidar = InputSystem.actions.FindAction("Lidar");
        m_mao1 = InputSystem.actions.FindAction("Arma1");
        m_mao2 = InputSystem.actions.FindAction("Arma2");
        _cam = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {

        if (moveValue != new Vector2(0, 0))
        {
            if (cd_passo > 0) cd_passo -= Time.deltaTime;
            else
            {
                cd_passo = 2;
                passo.Play();
            }
        }
        rb.MovePosition(rb.position + transform.forward * moveValue.y * moveSpeed * Time.deltaTime);
        rb.MovePosition(rb.position + transform.right * moveValue.x * moveSpeed * Time.deltaTime);

        

        if (m_menuAction.WasPressedThisFrame())
        { 
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("SemLuz");
        }

    }

    private void Update()
    {
        if (m_Lidar.IsPressed()) _animator.SetBool("isCharging", true);
        else _animator.SetBool("isCharging", false);

        if (m_Lidar.IsPressed() && _animator.GetBool("Arma2"));

        if (m_mao1.IsPressed())
        {
            _animator.SetBool("isLooking", true);
            _animator.SetBool("isFiring", false);
        }

        if (m_mao2.IsPressed())
        {
            _animator.SetBool("isLooking", false);
            _animator.SetBool("isFiring", true);
        }


        moveValue = m_moveAction.ReadValue<Vector2>();
        mouseValue = m_lookAction.ReadValue<Vector2>();
        Quaternion gira = Quaternion.Euler(0f, mouseValue.x * sensi * Time.deltaTime, 0f);
        rb.MoveRotation(rb.rotation * gira);
        offcam -= mouseValue.y * sensi * Time.deltaTime;
        offcam = Mathf.Clamp(offcam, -70f, 70f);
        _cam.transform.localRotation = Quaternion.Euler(offcam, 0f, 0f);
    }

}
