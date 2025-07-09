using TMPro;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{

    [SerializeField] string[] cutsceneTexts;
    [SerializeField] TextMeshProUGUI cutsceneText;
    [SerializeField] int cutsceneIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextChanger();
        if (cutsceneIndex == 1)
        {
            GetComponent<SceneTo>().ChangeScene();
        }
    }

    void TextChanger()
    {
        cutsceneText.text = cutsceneTexts[cutsceneIndex];
        if (Input.GetMouseButtonDown(0))
        {
            cutsceneIndex++;
        }
    }
}
