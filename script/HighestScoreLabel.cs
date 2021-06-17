using Godot;
using System;

public class HighestScoreLabel : Label
{
    private int _highestScore = 0;

    public int HighestScore
    {
        get => _highestScore;
        private set
        {

            _highestScore = value;
            Text = string.Format("Highest Score: {0}", _highestScore);
        }
    }

    public void SetScore(int score)
    {
        if (score > HighestScore)
        {
            HighestScore = score;
        }
    }
}
