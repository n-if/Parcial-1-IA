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

    public float stamina { get; set; }

    public float restTime;
    public float maxStamina;
    public Transform[] waypoints;

    public Vector3 Velocity { get { return _velocity; } }


    private void Start()
    {
        _fsm = new FSM();

        _fsm.CreateState("Idle", new Idle(_fsm, restTime, this));
        _fsm.CreateState("Patrol", new Patrol(_fsm, waypoints, transform, this));
        _fsm.CreateState("Chase", new Chase(_fsm, this, maxVelocity));

        stamina = maxStamina;
        _fsm.ChangeState("Patrol");
    }

    private void Update()
    {
        _fsm.Execute();

        if (_velocity.magnitude > 0.05f)
        {
          transform.position += _velocity * Time.deltaTime;
          transform.forward = _velocity;
        }

    }

    private float _closestDistance;
    private Boid _closestBoid;
    public Boid GetClosestBoid()
    {
        Collider[] collidersEnRango = Physics.OverlapSphere(transform.position, chaseRadius);

        _closestDistance = Mathf.Infinity;
        _closestBoid = null;

        foreach (var col in collidersEnRango)
        {
            GameObject obj = col.gameObject;

            if (obj.GetComponent<Boid>())
            {
                float distance = (obj.transform.position - gameObject.transform.position).magnitude;
                if (distance < _closestDistance)
                {
                    _closestDistance = distance;
                    _closestBoid = obj.GetComponent<Boid>();
                }
            }
        }
        return _closestBoid;
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;
        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }

    public Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering;
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
