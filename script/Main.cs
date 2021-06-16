using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene MobScene;

    public override void _Ready()
    {
        GD.Randomize();

        StartGame();
    }

    public void OnMobTimerTimeout()
    {
        Mob mob = MobScene.Instance<Mob>();

        var mobSpawnLocation = GetNode<PathFollow>("SpawnPath/SpawnLocation");
        mobSpawnLocation.UnitOffset = GD.Randf();

        Vector3 playerPosition = GetNode<Player>("Player").Transform.origin;

        AddChild(mob);
        mob.Initialize(mobSpawnLocation.Translation, playerPosition);
        mob.Connect(nameof(Mob.Squashed), GetNode<ScoreLabel>("UserInterface/ScoreLabel"), nameof(ScoreLabel.OnMobSquashed));
    }

    public void OnPlayerHit()
    {
        EndGame();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Retry").Visible)
        {
            GetTree().ReloadCurrentScene();
        }
    }

    private void StartGame()
    {
        GetNode<Control>("UserInterface/Retry").Hide();
        GetNode<AudioStreamPlayer>("MusicPlayer").Play();
    }

    private void EndGame()
    {
        GetNode<Timer>("MobTimer").Stop();
        GetNode<Control>("UserInterface/Retry").Show();
        GetNode<AudioStreamPlayer>("MusicPlayer").Stop();

        GetNode<AudioStreamPlayer>("GameOverMusicPlayer").Play();
    }
}
