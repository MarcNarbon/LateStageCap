using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class mapGenerator : MonoBehaviour
{

    public GameObject ciudadCarga;
    public GameObject ciudadDescarga;
    public Transform playerPos;

    public Vector3 firstSpawnPosition;
    public Vector3 secondSpawnPosition;
    public float lastYPos;

    public bool firstSpawned = false;
    public bool secondSpawned = false;


    // Use this for initialization
    void Start()
    {
        playerPos = GameObject .FindGameObjectWithTag("controlando") .transform;
        firstSpawnPosition = ciudadCarga .transform .position;
        secondSpawnPosition = ciudadDescarga .transform .position;
        Invoke("getPlayerLastPos" , 0.1f);
        spawnCity();

    }

    void getPlayerLastPos()
    {
        lastYPos = playerPos .position .y;
    }

    bool isEndMap;

    void spawnCity()
    {
        if ( firstSpawned || secondSpawned )
        {
            if ( firstSpawned && secondSpawned )
            {
                if ( !isEndMap )
                {
                    ciudadCarga .transform .position += new Vector3(0 , 108f , 0);
                    isEndMap = true;
                }
                else
                {
                    ciudadDescarga .transform .position += new Vector3(0 , 108f , 0);
                    isEndMap = false;
                }

            }
            else if ( firstSpawned && !secondSpawned )
            {
                ciudadDescarga .SetActive(true);
                secondSpawned = true;

            }

        }
        else if ( !firstSpawned && !secondSpawned )
        {
            ciudadCarga .SetActive(true);
            firstSpawned = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject .FindGameObjectWithTag("controlando") .transform;



        if ( playerPos .position .y >= lastYPos + 52f )
        {
            getPlayerLastPos();
            spawnCity();

        }


    }
}
