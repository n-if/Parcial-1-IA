using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Idle : IState
{
    FSM _fsm;

    private float _timer;
    private float _restTime;
    Hunter _hunter;
    
    public Idle(FSM fsm, float restTime, Hunter hunter)
    {
        _fsm = fsm;
        _restTime = restTime;
        _hunter = hunter;

    }

    public void OnEnter()
    {
        _timer = 0;
        
        
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        _timer += Time.deltaTime;

        if(_hunter.Velocity.magnitude != 0)
            _hunter.AddForce(_hunter.CalculateSteering(-_hunter.Velocity));

        if (_timer > _restTime)
        {
            _timer = 0;
            _hunter.stamina = _hunter.maxStamina;
            _fsm.ChangeState("Patrol");
        }
        

    }
}
