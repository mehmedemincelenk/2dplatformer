using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SiyahEkranManager : MonoBehaviour
{
    public CanvasGroup fadeGroup;
    public TMP_Text messageText;
    public Button tekrarDeneButton;
    public Image backgroundImage;

    string[] mesajlar = {
    "En cetin savas, gorunmeyen dusmanla; insanin kendi karanligiyladir.",
    "Bu son degil.",
    "Geri donus yok. Sadece ileri.",
    "Kilicini birakma, yol bitmedi.",
    "Henuz bitmedi.",
    "Seninle konusan o ses, senin olmadigini fark ettigin anda zayiflar.",
    "Daha kotulerini atlattin.",
    "Boyle bitemez.",
    "Zafer, sabredenin ve asla vazgecmeyenin oduludur.\n-Napoleon"
};


    void Start()
    {
        fadeGroup.alpha = 0;
        fadeGroup.interactable = false;
        fadeGroup.blocksRaycasts = false;

        backgroundImage.color = new Color(0, 0, 0, 1); // Siyah, tam opak

        messageText.text = mesajlar[Random.Range(0, mesajlar.Length)];
        string hedefSahne = PlayerPrefs.GetString("GeldigiSahne", "1_Bolum");
        tekrarDeneButton.onClick.AddListener(() => SceneManager.LoadScene(hedefSahne));

        StartCoroutine(FadeIn());
    }

    System.Collections.IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t; // Metin ve buton yava��a g�r�n�r
            backgroundImage.color = new Color(0, 0, 0, 1 - t); // Arka plan yava��a �effafla��r
            yield return null;
        }
        fadeGroup.alpha = 1;
        fadeGroup.interactable = true;
        fadeGroup.blocksRaycasts = true;
        backgroundImage.color = new Color(0, 0, 0, 0); // Tam �effaf
    }
}
