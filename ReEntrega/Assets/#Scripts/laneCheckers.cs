using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laneCheckers : MonoBehaviour
{

    public List<GameObject> cars;
    public bool occupied;
    int carAmmount = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (carAmmount == 0)
        {
            occupied = false;
        }
        else {
            occupied = true;
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Chasis")
        {

            cars.Insert(carAmmount, other.gameObject);
            carAmmount++;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {


        if (other.tag == "Chasis")
        {
            cars.Remove(other.gameObject);
            carAmmount--;
        }

    }
    }