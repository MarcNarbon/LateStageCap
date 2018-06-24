using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class dropItems : MonoBehaviour
{

    public Transform lootReferencePoint;

    [HideInInspector] public enum itemsToDrop { softPistol, hardPistol, shotgun, shotgunAmmo, rifleAmmo, pistolAmmo , empty};//En orden igual a la lista de sprites que asignamos y creamos en el inspector
    public itemsToDrop itemToDrop;
    public List<Sprite> items;
    public float dropMaxDist;
    public float speed;
    Vector3 dropPosition;
    public GameObject landPoint;
    public GameObject smoke;
    GameObject landPointInstantiate;
    bool itemOnAir;
    public float upSpeed;
    public float itemMaxHeight;
    bool itemGoingUp;
    bool itemTouchedFloor;
    public float floorLevel;
    public float pickUpLevel;
    int ammoAmmount;
    public GameObject playerInventory;

    SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        Transform referencePoint = Instantiate(lootReferencePoint , transform .position , transform .rotation);
        gameObject .transform .parent = referencePoint;

        if ( itemToDrop == itemsToDrop .empty )
        {
            itemToDrop = ( itemsToDrop )Random .Range(3 , 5);

            if ( Random .Range(0 , 5) == 0 )
            {
                itemOnAir = true;
                itemTouchedFloor = false;
                dropPosition = Random .insideUnitCircle * dropMaxDist;
                itemGoingUp = true;
                landPointInstantiate = Instantiate(landPoint , dropPosition + transform .parent .position , transform .rotation , referencePoint);
                spriteRenderer = GetComponent<SpriteRenderer>();

                spriteRenderer .sprite = items [( int )itemToDrop];

            }
            else
            {
                Destroy(gameObject);

            }

        }
        else
        {
            itemOnAir = true;
            itemTouchedFloor = false;
            dropPosition = Random .insideUnitCircle * dropMaxDist;
            itemGoingUp = true;
            landPointInstantiate = Instantiate(landPoint , dropPosition + transform .parent .position , transform .rotation , referencePoint);
            spriteRenderer = GetComponent<SpriteRenderer>();

            spriteRenderer .sprite = items [( int )itemToDrop];

        }



        //ammoAmmount = 

    }

    // Update is called once per frame
    void Update()
    {
        transform .Rotate(2 ,0 ,8);
        playerInventory = GameObject .Find("Inventory");


        if ( !itemTouchedFloor )
        {
            if ( itemOnAir )
            {
                if ( transform .position != dropPosition + transform .parent .position )
                {

                    float step = speed * Time .deltaTime;
                    transform .position = Vector3 .MoveTowards(transform .position , dropPosition + transform .parent .position , step);
                    if ( itemGoingUp )
                    {
                        transform .position += new Vector3(0 , 0 , -upSpeed);

                        if ( transform .position .z < itemMaxHeight )
                            itemGoingUp = false;
                    }

                    //landPointInstantiate.transform.position = new Vector3(dropPosition.x + transform.parent.position.x, dropPosition.y + transform.parent.position.y , floorLevel);//SOMBRA EN EL SITIO DE ATERRIZAJE
                    landPointInstantiate .transform .position = new Vector3(transform .position .x , transform .position .y , floorLevel);

                }

            }

            if ( transform .position .z > landPointInstantiate .transform .position .z )
            {
                itemTouchedFloor = true;

            }

        }
        else
        {
            DestroyItem();

        }

    }

    void DestroyItem()
    {

        GameObject smokeDestroy = Instantiate(smoke , new Vector3(transform .position .x , transform .position .y , transform .position .z) , transform .rotation);
        Destroy(gameObject);
        Destroy(landPointInstantiate);
        Destroy(smokeDestroy , 0.5f);

    }

    private void OnTriggerStay2D( Collider2D other )
    {
        if ( transform .position .z > pickUpLevel && !itemGoingUp )
        {
            spriteRenderer .color = Color .white;

            if ( Input .GetKeyDown(KeyCode .Space) )
            {
                if ( other .transform .parent .tag == "controlando" )
                {

                    if ( ( int )itemToDrop >= 0 && ( int )itemToDrop < 3 )
                    {

                        if ( playerInventory .GetComponent<inventory>() .weaponFirst == shooter .weaponType .empty )
                            playerInventory .GetComponent<inventory>() .weaponFirst = ( shooter .weaponType )itemToDrop;
                        else
                        if ( playerInventory .GetComponent<inventory>() .weaponSecond == shooter .weaponType .empty )
                            playerInventory .GetComponent<inventory>() .weaponSecond = ( shooter .weaponType )itemToDrop;
                        else
                        {

                            if ( playerInventory .GetComponent<inventory>() .firstWeaponSelected )
                                playerInventory .GetComponent<inventory>() .weaponFirst = ( shooter .weaponType )itemToDrop;
                            else
                                playerInventory .GetComponent<inventory>() .weaponSecond = ( shooter .weaponType )itemToDrop;

                        }
                        if ( ( int )itemToDrop == 2 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(Random .Range(2 , 12) , 0 , 0);
                        else if ( ( int )itemToDrop == 1 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(0 , Random .Range(2 , 12) , 0);
                        else if ( ( int )itemToDrop == 0 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(0 , 0 , Random .Range(12 , 34));




                        DestroyItem();

                    }
                    else if ( ( int )itemToDrop > 2 && ( int )itemToDrop <= 5 )
                    {

                        if ( ( int )itemToDrop == 3 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(Random .Range(4 , 18) , 0 , 0);
                        else if ( ( int )itemToDrop == 4 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(0 , Random .Range(4 , 16) , 0);
                        else if ( ( int )itemToDrop == 5 )
                            playerInventory .GetComponent<inventory>() .ammo += new Vector3(0 , 0 , Random .Range(18 , 48));

                        DestroyItem();
                    }



                }

            }

        }
    }

}

