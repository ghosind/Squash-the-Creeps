using Godot;
using System;

public class Main : Node
{
    [Export]
    public PackedScene MobScene;

    public override void _Ready()
    {
        GD.Randomize();
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
        GetNode<Timer>("MobTimer").Stop();
    }
}
