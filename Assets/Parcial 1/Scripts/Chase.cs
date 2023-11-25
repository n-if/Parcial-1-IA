using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    FSM _fsm;

    public Chase(FSM fsm)
    {
        _fsm = fsm;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }
}
