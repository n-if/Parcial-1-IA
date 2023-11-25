using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    FSM _fsm;

    private Vector3 _velocity;
    public float maxVelocity;
    public float maxForce;
    public float chaseRadius;

    
    public float restTime;
    public Transform[] waypoints;




    private void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Idle", new Idle(_fsm, restTime));
        _fsm.CreateState("Patrol", new Patrol(_fsm, waypoints, transform, maxVelocity, maxForce, chaseRadius));
        _fsm.CreateState("Chase", new Chase(_fsm));

        _fsm.ChangeState("Idle");
    }

    private void Update()
    {
        _fsm.Execute();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.green;
        foreach (var point in waypoints)
        {
            Gizmos.DrawWireSphere(point.position, 0.5f); 
        }
    }
}
