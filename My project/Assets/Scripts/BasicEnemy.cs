using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using UnityEngine.UIElements;
using System;
using Random = UnityEngine.Random;

public class BasicEnemy : MonoBehaviour
{
    public NavMeshAgent agente;
    public Transform[] Patrulhas;
    [SerializeField] private Transform atual;
    public bool indo1 = true;
    public bool perseguindo = false;
    public Transform player;
    public AudioSource grito;
    private float cd_grito = 10;

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.Find("player").transform;
        atual = Patrulhas[Random.Range(0, Patrulhas.Length)];
        cd_grito = 10;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!perseguindo)
        {
            if (agente.remainingDistance <= agente.stoppingDistance && !agente.pathPending)
            {
                
                atual.position = agente.destination;
                Debug.Log("[qfaxas] chegou " + atual);
                agente.SetDestination(Patrulhas[escolhe_ponto()].position);


            }
            
        }
        else { agente.SetDestination(player.position); }


    }

    
    private void Update()
    {
        if (cd_grito > 0)
        {
            cd_grito -= Time.deltaTime;
            Debug.Log(cd_grito);
        }
    }
    

    private int escolhe_ponto()
    {
        Debug.Log("[]qfaxas Escolhe novo");
        int novo = Random.Range(0,Patrulhas.Length);
        if (atual.position == Patrulhas[novo].position)
        {
            return escolhe_ponto();
        }
        return novo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            perseguindo = true;
            if (cd_grito <= 0.01f)
            {
                grito.Play();
                cd_grito = 10;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            perseguindo = false;
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
