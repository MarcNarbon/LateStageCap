using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector2 offset;
    GameObject car;


    private Vector2 targetPos;

    void Start()
    {
        
        

    }


    // Use this for initialization
    void Update () {
        car = GameObject.FindGameObjectWithTag("controlando");
        target = car.transform.GetChild(0).gameObject.transform;

    }
	
	// Update is called once per frame
	

     void FixedUpdate()
    {
        

        Vector2 desiredPosition = target.position;
        Vector2 smoothedPosition = Vector2.Lerp(desiredPosition, transform.position, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target);
        


    }


}

 