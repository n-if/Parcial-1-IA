using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : IState
{

    FSM _fsm;
    Transform[] _waypoints;
    Transform _hunterTransform;
    

    Hunter _hunter;
    int currentWaypoint = 0;


    public Patrol(FSM fsm, Transform[] waypoints, Transform hunterTr, Hunter hunter)
    {
        _fsm = fsm;
        _waypoints = waypoints;

        _hunterTransform = hunterTr;

        _hunter = hunter;
    }

    public void OnEnter()
    {
        //Debug.Log("Enter Patrol");
    }

    public void OnUpdate()
    {
        _hunter.stamina -= Time.deltaTime;
        if (_hunter.stamina < 0)
            _fsm.ChangeState("Idle");


        if (_hunter.GetClosestBoid())
            _fsm.ChangeState("Chase");

        else
        {
            Vector3 dir = _waypoints[currentWaypoint].position - _hunterTransform.position;
            if (dir.magnitude <= 0.5f)
            {
                currentWaypoint++;
                if (currentWaypoint >= _waypoints.Length)
                    currentWaypoint = 0;
            }
            _hunter.AddForce(_hunter.CalculateSteering(dir));

        }
    }

    public void OnExit()
    {
        //Debug.Log("Exit Patrol");
    }



}
