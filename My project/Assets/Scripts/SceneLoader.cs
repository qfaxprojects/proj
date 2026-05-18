using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public string nome;

    private void OnEnable()
    {
        Muda_Cena(nome);
    }

    public void Muda_Cena(string aaa)
    {
        SceneManager.LoadScene(aaa);
    }
}
