using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class cursor : MonoBehaviour {



    Vector2 mousePos;


	// Use this for initialization
	void Start () {

        Cursor.visible = false;


    }

    // Update is called once per frame
    void Update() {

        // mousePos = Input.mousePosition;
       




    }

    private void OnGUI()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.000017f));
    }
}
