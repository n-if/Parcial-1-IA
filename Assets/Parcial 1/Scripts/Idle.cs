using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Idle : IState
{
    FSM _fsm;

    private float _timer;
    private float _restTime;

    
    public Idle(FSM fsm, float restTime)
    {
        _fsm = fsm;
        _restTime = restTime;
    }

    public void OnEnter()
    {
        _timer = 0;
        Debug.Log("Enter Idle");
    }

    public void OnExit()
    {
        Debug.Log("Exit Idle");
        
    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;

        if (_timer > _restTime)
            _fsm.ChangeState("Patrol");
        

    }
}
