using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BasicEnemy : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform Patrol1;
    public Transform Patrol2;
    public bool indo1 = true;
    public bool perseguindo = false;
    public Transform player;

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.Find("player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!perseguindo)
        {
            if (agente.remainingDistance <= agente.stoppingDistance && !agente.pathPending)
            {
                indo1 = !indo1;
                if (indo1)
                {
                    agente.SetDestination(Patrol1.position);
                }
                else
                {
                    agente.SetDestination(Patrol2.position);
                }

            }
            
        }
        else { agente.SetDestination(player.position); }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agente.SetDestination(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            agente.SetDestination(other.transform.position);
        }
    }

    private bool checKArea()
    {

        /* carao mane
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100f, ~0))
        {
            Debug.Log("bateu");
            if (hit.collider.CompareTag("Player")) return true; 
        }
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);

        if (Physics.Raycast(transform.position, Quaternion.Euler(0,-15,0) * transform.forward, out RaycastHit hit2, 100f, ~0))
        {
            
            if (hit2.collider.CompareTag("Player")) return true;
        }
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -15, 0) * transform.forward *10, Color.green);

        if (Physics.Raycast(transform.position, Quaternion.Euler(0, 15, 0) * transform.forward, out RaycastHit hit3, 100f, ~0))
        {

            if (hit3.collider.CompareTag("Player")) return true;
        }
        Debug.DrawRay(transform.position, Quaternion.Euler(0, 15, 0)* transform.forward * 10, Color.green);

        */
        return false;
    }

}
