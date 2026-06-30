using UnityEngine;

public class Objetivos : MonoBehaviour
{
    [SerializeField] private GameObject Porta;
    [SerializeField] private GameObject Porta2;
    private int chaves = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objetivo"))
        {
            if (other.gameObject != Porta && other.gameObject != Porta2)
            {
                chaves += 1;
                //Destroy(other.gameObject);
                AudioSource novo = other.gameObject.GetComponent<AudioSource>();
                Debug.Log("[qfaxas] som = " + novo);
                novo.Play();
                Destroy(other.gameObject, 2);
            }
            if (chaves == 3)
            { 
                Destroy(Porta);
            }
        
        }
    }
}
