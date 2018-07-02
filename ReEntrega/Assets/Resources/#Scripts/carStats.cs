using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carStats : MonoBehaviour
{

    public int health;
    CarBehaviour carBehaviour;

    // Use this for initialization
    void Start()
    {
        carBehaviour = gameObject.GetComponent<CarBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {

        if (carBehaviour.isActiveAndEnabled)
        {
            carBehaviour.controlando = true;

        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {


    }
}
