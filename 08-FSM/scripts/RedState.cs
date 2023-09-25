using Godot;
using System;

public partial class RedState : State
{
    
    public override void Enter() {
        GetNode<Timer>("Timer").Start();
        GetNode<Node3D>("Light").Visible = true;
    }
    
    public override void Exit() {
        GetNode<Timer>("Timer").Stop();
        GetNode<Node3D>("Light").Visible = false;
    }
        
    public override void Ready() {		
        Events.RedButtonClicked += _OnRedButtonClicked;
    }
    
    private void _OnTimerTimeout()
    {
        fsm.TransitionTo("Green");
    }

    private void _OnRedButtonClicked(object sender, EventArgs e) {
        fsm.TransitionTo("Red");
    }
}
