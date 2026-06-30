using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public InputActionReference move;
    public GameObject botao;
    public GameObject Teste_diag;

    Rigidbody rb;
    Animator animator;
    InputAction moveAction;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        Debug.Log("[qfaxas]" + moveAction);
        Debug.Log("[qfaxas]" + rb);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            botao.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            botao.SetActive(false);
        }
    }



    private void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(moveValue.x * moveSpeed*2, moveValue.y * moveSpeed, moveValue.y * moveSpeed);
        if (moveValue != Vector2.zero)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
        if (InputSystem.actions.FindAction("Interact").triggered)
        {
            Teste_diag.SetActive(!Teste_diag.activeInHierarchy);
            if (Teste_diag.activeInHierarchy) SceneManager.LoadScene("SemLuz");
        }
    }

}
