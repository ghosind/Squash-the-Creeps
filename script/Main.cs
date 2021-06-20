using Godot;
using System;

public class Main : Node
{
    private static string SAVE_FILE_PATH = "user://save_game.dat";

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

        LoadGame();

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
        SaveGame();

        timer.Stop();
        retryControl.Show();
        musicPlayer.End();

        GetTree().CallGroup("mobs", "queue_free");
    }

    private void SaveGame()
    {
        var saveGame = new File();
        saveGame.Open(SAVE_FILE_PATH, File.ModeFlags.Write);

        saveGame.Store32((uint) highestScoreLabel.HighestScore);

        saveGame.Close();
    }

    private void LoadGame()
    {
        var saveGame = new File();
        if (!saveGame.FileExists(SAVE_FILE_PATH))
        {
            return;
        }

        saveGame.Open(SAVE_FILE_PATH, File.ModeFlags.Read);

        highestScoreLabel.SetScore((int) saveGame.Get32());

        saveGame.Close();
    }
}
