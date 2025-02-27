using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public float delayBeforeLoading = 3f; // Tempo di attesa prima del cambio scena
    public string nextSceneName = "SampleScene";  // Nome della prossima scena

    void Start()
    {
        Invoke("LoadNextScene", delayBeforeLoading);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}