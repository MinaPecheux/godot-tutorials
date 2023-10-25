using Godot;
using System;
using System.IO;

public partial class ResourceLoadDemo : Node3D
{
    private const string _ROOT_PATH = "res://art/resources";

    private Sprite3D _imageDisplay;
    private Node3D _audioDisplay;
    private AudioStreamPlayer _audioPlayer;
    private Node3D _modelDisplay;

    public override void _Ready()
    {
        _imageDisplay = GetNode<Sprite3D>("ImageDisplay");
        _audioDisplay = GetNode<Node3D>("AudioDisplay");
        _audioPlayer = _audioDisplay.GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        _modelDisplay = GetNode<Node3D>("ModelDisplay");
    }

    private void _OnImageBtnPressed(string path)
    {
        ResourceLoadUtils.LoadImage(_imageDisplay, Path.Join(_ROOT_PATH, path));
        _ToggleVisuals(0);
    }

    private void _OnAudioBtnPressed(string path)
    {
        ResourceLoadUtils.LoadAudioFile(_audioPlayer, Path.Join(_ROOT_PATH, path));
        _ToggleVisuals(1);
    }

    private void _OnModelBtnPressed(string path)
    {
        ResourceLoadUtils.Load3DModel(_modelDisplay, Path.Join(_ROOT_PATH, path));
        _ToggleVisuals(2);
    }

    private void _ToggleVisuals(int mode)
    {
        // mode: 0 = image, 1 = audio, 2 = 3D model
        switch (mode) {
            case 0:
                _imageDisplay.Visible = true;
                _audioDisplay.Visible = false;
                _modelDisplay.Visible = false;
                break;
            case 1:
                _imageDisplay.Visible = false;
                _audioDisplay.Visible = true;
                _modelDisplay.Visible = false;
                break;
            case 2:
                _imageDisplay.Visible = false;
                _audioDisplay.Visible = false;
                _modelDisplay.Visible = true;
                break;
            default:
                break;
        }
    }
}
