using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeOffScreen : MonoBehaviour {


    public Material colorFade;
    public float speedFade;
    float alpha = 1.0f;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

        gameObject.GetComponent<MeshRenderer>().material = colorFade;
        colorFade.color = new Color(0, 0, 0, alpha);
        alpha -= speedFade * Time.deltaTime;
        if (alpha <= 0) {
            transform.parent = null;
            Destroy(this.gameObject);
        }
            

	}
}
