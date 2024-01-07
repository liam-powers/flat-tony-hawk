using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Singleton;
    public int Score;
    private TMP_Text scoreDisplay;

    void Start()
    {
        Singleton = this;
        scoreDisplay = GetComponent<TMP_Text>();
        ScorePointsInternal(0);
    }

    public static void AddPoints(int points)
    {
        Singleton.ScorePointsInternal(points);
    }

    public static void SubtractPoints(int points)
    {
        Singleton.ScorePointsInternal(-points);
    }

    private void ScorePointsInternal(int delta)
    {
        Score += delta;

        if (Score < 0)
        {
            Score = 0;
        }

        scoreDisplay.text = "Score: " + Score;
    }
}
