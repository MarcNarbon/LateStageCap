using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class translatePoint : MonoBehaviour {

    public float speed;

	// Use this for initialization
	void Start () {
        Destroy(gameObject , 10f);
	}
	
	// Update is called once per frame
	void Update () {
        transform .position += new Vector3(0,speed,0);		




	}
}
