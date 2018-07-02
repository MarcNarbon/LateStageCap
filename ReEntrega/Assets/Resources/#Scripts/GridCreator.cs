using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{

    public float slotSpaceY, slotSpaceX;
    public int slotsX, slotsY;
    public Transform slot;
    public GameObject [ , ] gridSlots;
    public Transform carPrefab;
    public Vector2 playerSlotIndex;
    public Vector2 gridPos;
    bool spawned = false;
    int spawnedCounter = 0;
    public int enemyAmmount;
    public int spawnAmmount;
    bool spawnCars = true;

    bool playerSpawned = false;

    public void InitializeGrid( Vector2 spawnGridPos )
    {
        gridSlots = new GameObject [slotsX , slotsY];

        for ( int x = 0 ; x < slotsX ; x += 1 )
            for ( int y = 1 ; y < slotsY ; ++y )
            {
                GameObject gridArray = Instantiate(slot .gameObject , new Vector3(( ( x + spawnGridPos .x ) * slotSpaceX ) , ( ( y + spawnGridPos .y ) * slotSpaceY ) , 0) , slot .rotation);
                gridArray .GetComponent<slotsControl>() .xSlot = x;
                gridArray .GetComponent<slotsControl>() .ySlot = y;

                gridArray .transform .parent = gameObject .transform;//Pasa los objetos que creas a ser hijos del gridCreator
                gridSlots [x , y] = gridArray .gameObject;//Asignas los objetos a un array de transforms

                if ( GameObject .FindGameObjectWithTag("noControlando") == null )
                    if ( spawnCars )
                    {
                        if ( ( x == 1 ) || ( x == 2 ) || ( x == 3 ) )
                        {
                            if ( Random .Range(0 , 100) <= spawnAmmount )
                            {
                                GameObject car = Instantiate(carPrefab .gameObject , gridSlots [x , y] .transform .position , Quaternion .identity , gameObject .transform);
                                car .GetComponent<npcBehaviour>() .laneOfCar = x;
                                car .name = ( "X " + x + " | Y " + y );

                                if ( Random .Range(0 , 100) <= enemyAmmount )
                                {
                                    car .GetComponent<npcBehaviour>() .isEnemy = true;
                                    if ( Random .Range(0 , 100) < 50 )
                                        car .GetComponent<npcBehaviour>() .enemyDasher = true;
                                    else
                                        car .GetComponent<npcBehaviour>() .enemyShooter = true;
                                }

                                if ( !playerSpawned )
                                {
                                    car .GetComponent<movimientoPruebas>() .playerDriving = true;
                                    playerSpawned = true;
                                }
                            }
                        }
                    }
            }
        spawnCars = false;
    }

    public void getPlayerSlotPosition( Vector2 playerSlot )
    {

        playerSlotIndex = playerSlot;

    }

    public int spawnCarWaveRate;

    public void seamlessSpawner()
    {
        if ( spawnedCounter % spawnCarWaveRate == 0 )
        {
            spawnCars = true;
        }

        if ( spawnedCounter == 0 )
        {
            if ( !spawned )
            {
                InitializeGrid(new Vector2(0 , spawnedCounter * ( slotsY - 1 )));
                spawnedCounter++;

            }
        }
        else
        {
            if ( playerSlotIndex .y == ( slotsY - 5 ) )
            {
                if ( !spawned )
                {
                    spawned = true;
                    InitializeGrid(new Vector2(0 , spawnedCounter * ( slotsY - 1 )));
                    spawnedCounter++;
                }
            }
            else
            {
                spawned = false;
            }
        }

    }

    // Use this for initialization
    void Start()
    {




    }

    // Update is called once per frame
    void Update()
    {

        seamlessSpawner();

    }
}
