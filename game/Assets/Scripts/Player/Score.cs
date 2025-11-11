using UnityEngine;

public class Score : MonoBehaviour
{
  public static Score Instance { get; private set; }

  private double levelStartTime;
  private double levelEndTime;

  private int correctInfusions = 0;
  private int wrongInfusions = 0;
  private int deaths = 0;

  private bool levelEnded = false;

  private void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
  }

  private void Start()
  {
    levelStartTime = Time.time;
  }

  public void AddCorrectInfusion()
  {
    if (!levelEnded)
      correctInfusions++;
  }

  public void AddWrongInfusion()
  {
    if (!levelEnded)
      wrongInfusions++;
  }

  public void AddDeath()
  {
    if (!levelEnded)
      deaths++;
  }

  public void EndLevel()
  {
    if (levelEnded) return;

    levelEnded = true;
    levelEndTime = Time.time;

    double totalTime = levelEndTime - levelStartTime;
    double finalScore = CalculateFinalScore(totalTime);
  }

  private double CalculateFinalScore(double time)
  {
    double timeBonus = Mathf.Max(0, 1000 - (float)time * 10);
    double infusionScore = (correctInfusions * 100) - (wrongInfusions * 25);
    double deathPenalty = deaths * 150;

    return Mathf.Max(0, (float)(timeBonus + infusionScore - deathPenalty));
  }

  public double GetFinalTime() => Time.time - levelStartTime;
  public int GetCorrectInfusions() => correctInfusions;
  public int GetWrongInfusions() => wrongInfusions;
  public int GetDeaths() => deaths;
  public double GetFinalScore() => CalculateFinalScore(GetFinalTime());
}
