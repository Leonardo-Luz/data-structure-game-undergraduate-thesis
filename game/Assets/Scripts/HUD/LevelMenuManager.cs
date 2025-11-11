using UnityEngine;
using TMPro;

public class LevelMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text successCombText;
    [SerializeField] private TMP_Text wrongCombText;
    [SerializeField] private TMP_Text deathsText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text completionTimeText;

    [Header("Dialogue Lines (for localization)")]
    [SerializeField] private Dialogue successComb;
    [SerializeField] private Dialogue wrongComb;
    [SerializeField] private Dialogue deaths;
    [SerializeField] private Dialogue score;
    [SerializeField] private Dialogue completionTime;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void UpdateEndHUD()
    {
        if (Score.Instance == null) return;

        string GetLocalizedText(Dialogue data)
        {
            DialogueLine line = data.lines[0];
            return LanguageManager.Instance.GetLanguage() == Language.English
                ? line.englishText
                : line.portugueseText;
        }

        int correct = Score.Instance.GetCorrectInfusions();
        int wrong = Score.Instance.GetWrongInfusions();
        int death = Score.Instance.GetDeaths();
        double totalTime = Score.Instance.GetFinalTime();

        double finalScore = Score.Instance.GetFinalScore();

        successCombText.text = $"{GetLocalizedText(successComb)}: {correct}";
        wrongCombText.text = $"{GetLocalizedText(wrongComb)}: {wrong}";
        deathsText.text = $"{GetLocalizedText(deaths)}: {death}";
        scoreText.text = $"{GetLocalizedText(score)}: {finalScore:F2}";
        completionTimeText.text = $"{GetLocalizedText(completionTime)}: {totalTime:F2}s";

        gameObject.SetActive(true);
    }
}
