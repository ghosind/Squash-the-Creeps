using Godot;
using System;

public class MusicPlayer : Node
{
    public void Play()
    {
        GetNode<AudioStreamPlayer>("MusicPlayer").Play();
    }

    public void End()
    {
        GetNode<AudioStreamPlayer>("MusicPlayer").Stop();
        GetNode<AudioStreamPlayer>("GameoverMusicPlayer").Play();
    }
}
