using Godot;
using System;

public partial class GreenState : State
{
    
    public override void Enter() {
        GetNode<Timer>("Timer").Start();
        GetNode<Node3D>("Light").Visible = true;
    }
    
    public override void Exit() {
        GetNode<Timer>("Timer").Stop();
        GetNode<Node3D>("Light").Visible = false;
    }
    
    private void _OnTimerTimeout()
    {
        fsm.TransitionTo("Orange");
    }

}
