using Godot;
using System;

public class ScoreLabel : Label
{
    private int _score = 0;

    public int Score {
        get => _score;
        private set
        {
            _score = value;
            Text = string.Format("Score: {0}", _score);
        }
    }

    public void OnMobSquashed()
    {
        Score += 1;
    }

    public void Clear()
    {
        Score = 0;
    }
}
