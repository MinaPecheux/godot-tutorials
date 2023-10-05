using Godot;
using System;

public partial class SplashScreen : ColorRect
{
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Timeout +=
            () => GetNode<SceneLoader>("/root/SceneLoader").ChangeToScene("main_menu.tscn");
    }
}
