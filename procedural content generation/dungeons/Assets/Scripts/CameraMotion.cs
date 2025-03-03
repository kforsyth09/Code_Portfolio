
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraMotion : MonoBehaviour
// {
//     public Light flashlight;
//     private float moveSpeed = 10f;  
//     private float turnSpeed = 100f; 
//     private Vector3 firstPos = new Vector3(55, 2, -3);
//     private Vector3 firstRot = new Vector3(0, 0, 0);
//     private Vector3 thirdPos;
//     private Vector3 thirdRot = new Vector3(90, 0, 0);
//     private bool firstPerson = false;

//     void Start() {
//         thirdPos = transform.position;
//     }

//     void Update() {
//         if (firstPerson) {
//             handleFirstPerson();
//         } else {
//             handleThirdPerson();
//         }

//         if (Input.GetKeyDown(KeyCode.Space)) {
//             if (firstPerson) {
//                 transform.position = thirdPos;
//                 transform.eulerAngles = thirdRot;
//                 flashlight.transform.position = thirdPos;
//                 firstPerson = false;
//             } else {
//                 transform.position = firstPos;
//                 transform.eulerAngles = firstRot;
//                 firstPerson = true;
//             }
//         }
//     }

//     private void handleFirstPerson() {
//         float moveX = Input.GetAxis("Horizontal");  
//         float moveZ = Input.GetAxis("Vertical");    
//         Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
//         transform.position += move;
//         flashlight.transform.position = transform.position ;
//         float turn = moveX * turnSpeed * Time.deltaTime;
//         transform.Rotate(0, turn, 0);
//         flashlight.transform.rotation = transform.rotation;
//     }

//     private void handleThirdPerson() {
//         float moveX = Input.GetAxis("HorizontalArrow");  
//         float moveZ = Input.GetAxis("VerticalArrow");   
//         Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
//         transform.position += move;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    public Light flashlight;
    public Terrain terrain;
    private float moveSpeed = 10f;  
    private float turnSpeed = 100f; 
    private Vector3 firstPos = new Vector3(55, 2, -3);
    private Vector3 firstRot = new Vector3(0, 0, 0);
    private Vector3 thirdPos;
    private Vector3 thirdRot = new Vector3(90, 0, 0);
    private bool firstPerson = false;

    void Start() {
        thirdPos = transform.position;
    }

    void Update() {
        if (firstPerson) {
            handleFirstPerson();
        } else {
            handleThirdPerson();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (firstPerson) {
                transform.position = thirdPos;
                transform.eulerAngles = thirdRot;
                flashlight.transform.position = thirdPos;
            } else {
                transform.position = firstPos;
                transform.eulerAngles = firstRot;
            }
            firstPerson = !firstPerson;
        }
    }

    private void handleFirstPerson() {
        float moveX = Input.GetAxis("Horizontal");  
        float moveZ = Input.GetAxis("Vertical");    
        Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
        float terrainHeight = terrain.SampleHeight(transform.position);
        transform.position = new Vector3(transform.position.x, terrainHeight + 0.1f, transform.position.z);
        flashlight.transform.position = transform.position;
        flashlight.transform.rotation = transform.rotation;
        float turn = moveX * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }

    private void handleThirdPerson() {
        float moveX = Input.GetAxis("HorizontalArrow");  
        float moveZ = Input.GetAxis("VerticalArrow");   
        Vector3 move = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;
        transform.position += move;
    }
}

