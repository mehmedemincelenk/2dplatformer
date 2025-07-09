using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTo : MonoBehaviour
{
    [Header("Geçiş yapılacak sahne adı")]
    [SerializeField] private string sceneToLoad;

    // Sahne geçişini başlatmak için public metod
    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("SceneToLoad boş bırakılmış!");
        }
    }
}
