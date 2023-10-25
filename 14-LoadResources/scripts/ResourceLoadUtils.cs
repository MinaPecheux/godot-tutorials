using Godot;
using System;

public static class ResourceLoadUtils
{
    public static void LoadImage(Sprite3D sprite, string imgPath)
    {
        sprite.Texture = GD.Load<Texture2D>(imgPath);
    }

    public static void LoadAudioFile(AudioStreamPlayer player, string audioPath)
    {
        player.Stream = GD.Load<AudioStreamOggVorbis>(audioPath);
        player.Play();
    }

    public static void Load3DModel(Node3D parent, string modelPath)
    {
        // (clean-up previous child, if any)
        if (parent.GetChildCount() > 0)
            parent.GetChild(0).Free();

        PackedScene s = GD.Load<PackedScene>(modelPath);
        Node3D model = s.Instantiate<Node3D>();
        parent.AddChild(model);
    }
}
