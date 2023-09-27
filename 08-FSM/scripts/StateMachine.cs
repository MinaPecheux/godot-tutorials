using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    [Export] public NodePath initialState;
    
    private Dictionary<string, State> _states;
    private State _currentState;
    
    public override void _Ready()
    {
        _states = new Dictionary<string, State>();
        foreach (Node node in GetChildren()) {
            if (node is State s) {
                _states[node.Name] = s;
                s.fsm = this;
                s.Ready();
                s.Exit(); // reset
            }
        }
        
        _currentState = GetNode<State>(initialState);
        _currentState.Enter();
    }

    public override void _Process(double delta)
    {
        _currentState.Update((float)delta);
    }
    
    public override void _PhysicsProcess(double delta)
    {
        _currentState.PhysicsUpdate((float)delta);
    }
    
    public override void _UnhandledInput(InputEvent @event)
    {
        _currentState.HandleInput(@event);
        @event.Dispose();
    }
    
    public void TransitionTo(string key) {
        if (!_states.ContainsKey(key) || _states[key] == _currentState)
            return;
        _currentState.Exit();
        _currentState = _states[key];
        _currentState.Enter();
    }
}
