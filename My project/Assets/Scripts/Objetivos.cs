using UnityEngine;

public class Objetivos : MonoBehaviour
{
    [SerializeField] private GameObject Porta;
    private int chaves = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objetivo"))
        {
            if (other.gameObject != Porta)
            {
                chaves += 1;
                Destroy(other.gameObject);
            }
            if (chaves == 3)
            { 
                Destroy(Porta);
            }
        
        }
    }
}
