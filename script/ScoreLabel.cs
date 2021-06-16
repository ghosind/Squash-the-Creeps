using Godot;
using System;

public class ScoreLabel : Label
{
    public int Score { get; private set; } = 0;

    public void OnMobSquashed()
    {
        Score += 1;
        Text = string.Format("Score: {0}", Score);
    }
}
