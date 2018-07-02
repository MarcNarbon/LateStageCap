using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slotsControl : MonoBehaviour
{

    public Collider2D tileCollider;
    // Use this for initialization
    public bool occupied;
    public int xSlot, ySlot;
    public List<GameObject> Cars;
    public int carAmmount = 0;
    public List<GameObject> carreteras;
    public GameObject sideRoadLeft;
    public GameObject sideRoadRight;

    public Material playerOnSlot;

    bool destroyable = false;

    public Vector2 playerSlot;
    public Vector2 carSlot;

    public int lightSpawnRate;
    int lightCounter = 0;

    void Start()
    {
        tileCollider = GetComponentInChildren<Collider2D>();

        switch (xSlot)
        {
            case 0:
                carreteras[0].gameObject.SetActive(true);
                sideRoadLeft.SetActive(true);
                sideRoadRight.SetActive(false);
                if ((ySlot + 3 ) % 5 == 0)
                    sideRoadLeft.transform.GetChild(0).gameObject.SetActive(true);

                break;
            case 1:
                carreteras[1].gameObject.SetActive(true);
                sideRoadLeft.SetActive(false);
                sideRoadRight.SetActive(false);
                break;
            case 2:
                carreteras[2].gameObject.SetActive(true);
                sideRoadLeft.SetActive(false);
                sideRoadRight.SetActive(false);
                break;
            case 3:
                carreteras[3].gameObject.SetActive(true);
                sideRoadLeft.SetActive(false);
                sideRoadRight.SetActive(false);
                break;
            case 4:
                carreteras[4].gameObject.SetActive(true);
                sideRoadLeft.SetActive(false);
                sideRoadRight.SetActive(true);

                if (ySlot % 5 == 0)
                    sideRoadRight.transform.GetChild(0).gameObject.SetActive(true);

                break;
            default:
                break;

        }
        carSlot = new Vector2(xSlot, ySlot);

    }

    // Update is called once per frame
    void Update()
    {
        //if()

        if (carAmmount == 0)
        {

            occupied = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Chasis")
        {
            occupied = true;

            Cars .Insert(carAmmount, other.gameObject);
            carAmmount++;
            //print(other.gameObject.transform.parent.tag);
            if (other.gameObject.transform.parent.tag == "controlando")
            {

                playerSlot = new Vector2(xSlot, ySlot);

                gameObject.transform.parent.GetComponentInParent<GridCreator>().getPlayerSlotPosition(playerSlot);
                other.gameObject.transform.parent.GetComponent<movimientoPruebas>().getNPCSlot(carSlot);

                //gameObject.GetComponentInChildren<MeshRenderer>().material = playerOnSlot;

            }
            else if (other.gameObject.transform.parent.tag == "noControlando")
            {

                carSlot = new Vector2(xSlot, ySlot);
                other.gameObject.transform.parent.GetComponent<movimientoPruebas>().getNPCSlot(carSlot);

            }
        }

        if (other.name == "destructor")
        {
            Destroy(transform .parent .gameObject);

        }
        if (other.tag == "destructor")
        {
            Destroy(transform.parent.gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (carAmmount == 0)
        {

            occupied = false;
        }

        if (other.tag == "Chasis")
        {
            Cars.Remove(other.gameObject);
            carAmmount--;
        }


        if (other.name == "destructor")
        {
            Destroy(transform .parent .gameObject);

        }
        if (other.tag == "destructor")
        {
            Destroy(transform .parent .gameObject);
        }

        if (other.gameObject.transform.parent.tag == "controlando")
        {
           
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {


    }
    
    public void OnBecameVisible()
    {
        //if(Time.time > 3f)
        destroyable = true;

    }


    void waitDestroy() {

        Destroy(gameObject);

    }

    public void OnBecameInvisible()
    {



        if (destroyable)
        {
            if ( transform .position .y < GameObject .FindGameObjectWithTag("controlando") .transform .position .y )
                Invoke("waitDestroy",3f); 
        }

    
    }


}

