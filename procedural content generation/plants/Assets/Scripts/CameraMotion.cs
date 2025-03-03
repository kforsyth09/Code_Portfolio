using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    private float moveSpeed = 10f;  
    private float turnSpeed = 100f; 

    void Update() {
        float moveX = Input.GetAxis("Horizontal");  
        float moveZ = Input.GetAxis("Vertical");    
        Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
        float turn = moveX * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }
}
