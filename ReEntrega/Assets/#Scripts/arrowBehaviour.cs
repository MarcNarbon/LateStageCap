using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowBehaviour : MonoBehaviour {

    public Material good, neutral, bad;
    MeshRenderer mesh;
    float counter = 0; 
    public int  counterSpeed;
    
	// Use this for initialization
	void Start () {
        mesh = transform.GetComponent<MeshRenderer>();


	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime * counterSpeed;

        if (counter < 1 && counter > 0)
            mesh.material = bad;

        if (counter > 1 && counter < 2)
            mesh.material = neutral;

        if (counter > 2 && counter < 3)
            mesh.material = good;
        if (counter > 3) {

            Destroy(gameObject, 0f);

        }
    }
}
