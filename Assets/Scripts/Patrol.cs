using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{

    FSM _fsm;
    Transform[] _waypoints;
    Transform _hunterTransform;
    float _maxVelocity, _maxForce, _chaseRadius;

    public Patrol(FSM fsm, Transform[] waypoints, Transform hunterTr, float maxVel, float maxForce, float chaseRadius)
    {
        _fsm = fsm;
        _waypoints = waypoints;

        _hunterTransform = hunterTr;
        _maxVelocity = maxVel;
        _maxForce = maxForce;
        _chaseRadius = chaseRadius;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Patrol");
    }

    public void OnUpdate()
    {
        Collider[] boidsInRange = Physics.OverlapSphere(_hunterTransform.position, _chaseRadius);
        if (boidsInRange.Length != 0)
            _fsm.ChangeState("Chase");
        else
        {

        }
        //pregunto si está en rango --> _fsm.ChangeState("Chase")
        //else --> Patrullo



    }

    public void OnExit()
    {
        Debug.Log("Exit Patrol");
    }

   

}
