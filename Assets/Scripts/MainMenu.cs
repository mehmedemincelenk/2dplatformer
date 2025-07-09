using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadBolum1() { SceneManager.LoadScene("StartCutscene"); }
    public void LoadBolum2() { SceneManager.LoadScene("2_Bolum"); }
    public void LoadBolum3() { SceneManager.LoadScene("3_Bolum"); }
    public void LoadFinalBolumu() { SceneManager.LoadScene("Boss Scene"); }
}
