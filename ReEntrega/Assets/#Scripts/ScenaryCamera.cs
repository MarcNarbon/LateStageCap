using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaryCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		


	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, -Camera.main.transform.position.y/50 + transform.position.z);
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + Camera.main.transform.rotation.z, 0);
	}
}
