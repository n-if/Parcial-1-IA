using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float destructonRadius;


    private void Update()
    {
        Collider[] collidersEnRango = Physics.OverlapSphere(transform.position, destructonRadius);
        foreach (var col in collidersEnRango)
        {
            GameObject obj = col.gameObject;
            if (obj.GetComponent<Boid>())
                Destroy(gameObject);
        }
    }
}
