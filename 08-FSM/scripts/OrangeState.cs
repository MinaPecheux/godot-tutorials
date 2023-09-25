using Godot;
using System;

public partial class OrangeState : State
{
    private FastNoiseLite _noise = new FastNoiseLite();
    private Light3D _light;
    private float _energy;
    private const float _MAX_ENERGY = 100000000;
    
    public override void Enter() {
        GetNode<Timer>("Timer").Start();
        GetNode<Node3D>("Light").Visible = true;
    }
    
    public override void Exit() {
        GetNode<Timer>("Timer").Stop();
        GetNode<Node3D>("Light").Visible = false;
    }
    
    public override void Ready() {
        _light = GetNode<Light3D>("Light");
        
        // setup noise
        GD.Randomize();
        _noise.Frequency = 1f;
    }
    
    public override void Update(float delta) {
        _energy += 0.5f;
        if (_energy > _MAX_ENERGY) _energy = 0f;
        _light.LightEnergy = _noise.GetNoise1D((_energy + 1) / 4f) + 0.5f;
    }
    
    private void _OnTimerTimeout()
    {
        fsm.TransitionTo("Red");
    }

}
