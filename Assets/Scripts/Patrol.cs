using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{

    FSM _fsm;

    public Patrol(FSM fsm)
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
