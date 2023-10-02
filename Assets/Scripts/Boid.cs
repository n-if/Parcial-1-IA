using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private Vector3 _velocity;
    public float maxVelocity;
    public float maxForce;
    public float separationRadius, viewRadius;

    private void Start()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        AddForce(new Vector3(randomX, 0, randomZ) * maxVelocity);



        GameManager.Instance.boids.Add(this);

    }

    private void Update()
    {
        Flocking();
        transform.position = GameManager.Instance.ApplyBounds(transform.position + _velocity * Time.deltaTime);

        transform.forward = _velocity;
    }

    private void Flocking()
    {
        AddForce(Separation(GameManager.Instance.boids, separationRadius) * GameManager.Instance.separationWeight);
        AddForce(Alignment(GameManager.Instance.boids, viewRadius) * GameManager.Instance.alignmentWeight);
        AddForce(Cohesion(GameManager.Instance.boids, viewRadius) * GameManager.Instance.cohesionWeight);
    }

    private Vector3 Separation(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;

        foreach (var boid in boids)
        {
            var dir = boid.transform.position - transform.position;

            if (dir.magnitude > separationRadius || boid == this)
                continue;

            desired -= dir;
        }

        if (desired == Vector3.zero)
            return desired;

        desired.Normalize();
        desired *= maxVelocity;


        return CalculateSteering(desired);

    }

    private Vector3 Cohesion(List<Boid> boids, float radius)
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in boids)
        {
            var dist = Vector3.Distance(transform.position, item.transform.position);

            if(dist > radius || item == this)
                continue;

            desired += item.transform.position;
            count++;
        }

        if (count <= 0)
            return Vector3.zero;

        desired /= count;

        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    private Vector3 Alignment(List<Boid> boids, float radius)
    {
        var desired = Vector3.zero;
        int count = 0;



        foreach (var boid in boids)
        {
            if (boid == this)
                continue;

            if (Vector3.Distance(transform.position, boid.transform.position) <= radius)
            {
                desired += boid._velocity;
                count++;
            }


        }
        
        if (count <= 0)
            return Vector3.zero;

        desired /= count;
        desired.Normalize();
        desired *= maxVelocity;

        return CalculateSteering(desired);
    }

    public void AddForce(Vector3 dir)
    {
        _velocity += dir;
        _velocity = Vector3.ClampMagnitude(_velocity, maxVelocity);
    }

    private Vector3 CalculateSteering(Vector3 desired)
    {
        var steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, maxForce);
        return steering;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        
    }
}
