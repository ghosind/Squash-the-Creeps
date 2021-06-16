using Godot;
using System;

public class HighestScoreLabel : Label
{
    public int HighestScore { get; private set; } = 0;

    public void SetHighestScore(int score)
    {
        HighestScore = score;
        Text = string.Format("Highest Score: {0}", HighestScore);
    }
}
