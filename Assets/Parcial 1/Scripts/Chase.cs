using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : IState
{
    FSM _fsm;

    Transform target;
    Hunter _hunter;
    float _maxVelocity;
    public Chase(FSM fsm, Hunter hunter, float maxVelocity)
    {
        _fsm = fsm;
        _hunter = hunter;
        _maxVelocity = maxVelocity;
    }

    public void OnEnter()
    {
        //Debug.Log("Chase on Enter");
    }

    public void OnUpdate()
    {
        _hunter.stamina -= Time.deltaTime;
        if (_hunter.stamina < 0)
            _fsm.ChangeState("Idle");

        var boid = _hunter.GetClosestBoid();
        if (boid)
        {
            target = boid.transform;
            _hunter.AddForce(_hunter.CalculateSteering(Pursuit(target.position + boid.Velocity)));
        }
        else
            _fsm.ChangeState("Patrol");

        if((target.position - _hunter.transform.position).magnitude < 0.5f)
        {
            boid.Die();
            Debug.Log("Killed Boid");
        }
    }

    Vector3 Pursuit(Vector3 target)
    {
        var desired = target - _hunter.transform.position;
        desired.Normalize();
        desired *= _maxVelocity;

        return desired;
    }

    public void OnExit()
    {
        //Debug.Log("Chase on exit");
    }
}
