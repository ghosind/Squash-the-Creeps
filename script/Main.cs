using Godot;
using System;

public class Main : Node
{
    private MusicPlayer musicPlayer;

    private Timer timer;

    private HighestScoreLabel highestScoreLabel;

    private ScoreLabel scoreLabel;

    private Control retryControl;

    private Player player;

    [Export]
    public PackedScene MobScene;

    public override void _Ready()
    {
        musicPlayer = GetNode<MusicPlayer>("MusicPlayer");
        timer = GetNode<Timer>("MobTimer");
        retryControl = GetNode<Control>("UserInterface/Retry");
        player = GetNode<Player>("Player");
        scoreLabel = GetNode<ScoreLabel>("UserInterface/ScoreLabel");
        highestScoreLabel = GetNode<HighestScoreLabel>("UserInterface/HighestScoreLabel");

        GD.Randomize();

        StartGame();
    }

    public void OnMobTimerTimeout()
    {
        Mob mob = MobScene.Instance<Mob>();
        mob.AddToGroup("mobs");

        var mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        mobSpawnLocation.UnitOffset = GD.Randf();

        Vector3 playerPosition = player.Transform.origin;

        AddChild(mob);
        mob.Initialize(mobSpawnLocation.Translation, playerPosition);
        mob.Connect(nameof(Mob.Squashed), scoreLabel, nameof(ScoreLabel.OnMobSquashed));
    }

    public void OnPlayerHit()
    {
        EndGame();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && retryControl.Visible)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        player.Translation = Vector3.Zero;
        player.Show();

        scoreLabel.Clear();
        retryControl.Hide();
        musicPlayer.Play();
        timer.Start();
    }

    private void EndGame()
    {
        highestScoreLabel.SetScore(scoreLabel.Score);

        timer.Stop();
        retryControl.Show();
        musicPlayer.End();

        GetTree().CallGroup("mobs", "queue_free");
    }
}
