using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    // Use this for initialization

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

            transform.parent = GameObject.FindGameObjectWithTag("controlando").transform;
            transform.position = GameObject.FindGameObjectWithTag("controlando").transform.position + new Vector3(-0.15f , 0.22f, 0);//OFFSET DE LA CAMARA 

    }
}
