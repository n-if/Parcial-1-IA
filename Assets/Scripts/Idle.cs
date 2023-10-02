using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Idle : IState
{
    FSM _fsm;

    public Idle(FSM fsm)
    {
        _fsm = fsm;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
