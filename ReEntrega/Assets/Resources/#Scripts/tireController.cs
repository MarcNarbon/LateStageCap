using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tireController : MonoBehaviour
{

    public GameObject tireDestroyPrefab;


    public bool wheelFrontL = true;
    public bool wheelFrontR = true;
    public bool wheelBackL = true;
    public bool wheelBackR = true;

    bool tireState;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (!wheelBackL || !wheelBackR || !wheelFrontL || !wheelFrontR) {

            tireState = true;

        }
        else {

            tireState = false;
        }

        gameObject.GetComponentInParent<movimientoPruebas>().tireMovement(wheelFrontL, wheelFrontR, wheelBackL, wheelBackR, tireState);


        



    }
}
