using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 netForce;
    public GameObject instance;
    // important values found in GameObject: position, rotation (orientation)
    public Vector3 velocity;
    public List<GameObject> trail = new List<GameObject>();

    public Boid() {
        netForce = new Vector3(0f, 0f, 0f);
        velocity = new Vector3(
            Random.Range(-10f, 10f), 
            Random.Range(-10f, 10f),
            Random.Range(-10f, 10f));
        // velocity = velocity.normalized;
    }
}
