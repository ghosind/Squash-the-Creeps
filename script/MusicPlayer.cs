using Godot;
using System;

public class MusicPlayer : Node
{
    private AudioStreamPlayer musicPlayer;

    private AudioStreamPlayer gameoverPlayer;

    public override void _Ready()
    {
        base._Ready();

        musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        gameoverPlayer = GetNode<AudioStreamPlayer>("GameoverMusicPlayer");
    }

    public void Play()
    {
        musicPlayer?.Play();
    }

    public void End()
    {
        musicPlayer?.Stop();
        gameoverPlayer?.Play();
    }
}
