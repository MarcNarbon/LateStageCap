using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class npcBehaviour : MonoBehaviour
{


    public Transform Chasis;
    float aimTimer = 0;
    int npcCastTime = 90;
    public GameObject grid;
    public GameObject laneCheckLeft;
    public GameObject laneCheckRight;

    public Material lighsOn;
    public Material brakeLights;

    public shooter shooterScript;
    public movimientoPruebas movScript;
    Vector2 npcPos;
    public int laneOfCar;
    public bool isEnemy;
    public bool enemyDasher = false;
    public bool enemyShooter = false;

    bool swichingLane;
    int prevLane;
    public bool canDash;
    public Transform playerPos;

    bool playerDetected = false;
    bool weaponOutLeft = false;
    bool weaponOutRight = false;

    bool castingLeft = false, castingRight = false;
    bool occupiedLeft, occupiedRight;


    bool canUseLeftGun = true;
    bool canUseRightGun = true;

    private void Awake()
    {


    }

    void Start()
    {
        swichingLane = false;
        canDash = true;
        movScript = GetComponent<movimientoPruebas>();
        grid = GameObject .FindWithTag("gridCreator");
        playerPos = GameObject .FindGameObjectWithTag("controlando") .GetComponent<Transform>();
        movScript .shooter .SetActive(false);//Desactiva el arma del coche
        Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsA") .GetComponent<Renderer>() .material = lighsOn;
        Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsB") .GetComponent<Renderer>() .material = lighsOn;
        shooterScript = GetComponentInChildren<shooter>();
    }

    public bool canChangeLane = true;

    void Update()
    {


        if ( playerPos == null )
            playerPos = GameObject .FindGameObjectWithTag("controlando") .GetComponent<Transform>();

        if ( movScript == null )
            movScript = GetComponent<movimientoPruebas>();
        if ( grid == null )
            grid = GameObject .FindWithTag("gridCreator");

        if ( laneCheckRight .GetComponent<laneCheckers>() .occupied )
            occupiedRight = true;
        else
            occupiedRight = false;

        if ( laneCheckLeft .GetComponent<laneCheckers>() .occupied )
            occupiedLeft = true;
        else
            occupiedLeft = false;

        if ( swichingLane )
            if ( occupiedLeft || occupiedRight )
                if ( prevLane != 0 )
                    laneOfCar = prevLane;

        if ( !movScript .casting )
        {
            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsA") .GetComponent<Renderer>() .material = lighsOn;
            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsB") .GetComponent<Renderer>() .material = lighsOn;
        }

        if ( !movScript .playerDriving )
        {
            movScript .laneSpeed(false);

            if( canChangeLane )
            if ( !movScript .dashing )
                carAlignToLane();

            if ( Random .Range(0f , 100f) < 0.05 )
                changeLane();

            gameObject .tag = "noControlando";//Se asigna el tag al coche que no esta siendo controlado por el jugador

            if ( isEnemy )
            {
                if ( enemyDasher )
                    if ( Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) < 0.5f )//dashing y casting
                    {
                        if ( !playerDetected )
                        {
                            playerDetected = true;
                            movScript .casting = true;

                            if ( playerPos .localPosition .x < transform .localPosition .x )
                            {
                                castingLeft = true;
                                castingRight = false;
                            }
                            else if ( playerPos .localPosition .x > transform .localPosition .x )
                            {
                                castingLeft = false;
                                castingRight = true;
                            }
                        }
                        if ( canDash )
                        {
                            castDashNPC();
                        }
                    }

                if ( enemyShooter )
                {
                    if ( Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) < 0.5f && Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) > -0.5f )//APUNTAR, SACAR ARMA Y DISPARAR
                    {


                        if ( canUseRightGun )
                            if ( !weaponOutRight )
                                if ( playerPos .localPosition .x < transform .localPosition .x )
                                {
                                    movScript .shooter .SetActive(true);//Desactiva el arma del coche
                                    movScript .shooter .GetComponent<shooter>() .enemyAiming = true;

                                    movScript .shooter .GetComponent<shooter>() .EnemyAiming(true , playerPos);
                                    movScript .shooter .GetComponent<shooter>() .enemyAiming = true;

                                    if ( Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) < 0.4f )
                                        movScript .shooter .GetComponent<shooter>() .shootBullet();

                                    aimTimer = Time .time;
                                    weaponOutLeft = true;
                                }
                                else
                                {
                                    if ( ( aimTimer + 4f ) < Time .time )
                                    {
                                        HideEnemyGun();
                                        aimTimer = 0;
                                    }
                                }

                        if ( canUseLeftGun )
                            if ( !weaponOutLeft )
                                if ( playerPos .localPosition .x > transform .localPosition .x )
                                {
                                    movScript .shooter .SetActive(true);//Desactiva el arma del coche
                                    movScript .shooter .GetComponent<shooter>() .enemyAiming = true;

                                    movScript .shooter .GetComponent<shooter>() .EnemyAiming(false , playerPos);
                                    movScript .shooter .GetComponent<shooter>() .enemyAiming = true;

                                    if ( Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) < 0.3f )
                                        movScript .shooter .GetComponent<shooter>() .shootBullet();
                                    aimTimer = Time .time;
                                    weaponOutRight = true;
                                }
                                else
                                {
                                    if ( ( aimTimer + 4f ) < Time .time )
                                    {
                                        HideEnemyGun();
                                        aimTimer = 0;
                                    }
                                }
                    }
                    else
                    {
                        HideEnemyGun();
                    }
                }

            }

        }
        else
        {
            isEnemy = false;
            enemyDasher = false;
            enemyShooter = false;
            movScript .laneSpeed(true);//El coche no se ve influenciado por la velocidad de la carretera


        }
    }

    public void HideEnemyGun()
    {
        movScript .shooter .GetComponent<shooter>() .enemyAiming = false;
        movScript .shooter .SetActive(false);//Desactiva el arma del coche
        weaponOutRight = false;
        weaponOutLeft = false;
    }

    void dashAgain()
    {
        canDash = true;
        playerDetected = false;
    }

    void castDashNPC()
    {
        if ( movScript .casting )
        {
            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsA") .GetComponent<Renderer>() .material = brakeLights;
            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsB") .GetComponent<Renderer>() .material = brakeLights;

            if ( castingRight )//Si el jugador esta casteando un dash hacia la derecha
            {
                movScript .sideMoving = true;//El jugador se esta moviendo con el volante horizontalmente
                movScript .xAcc = -movScript .fCastDash(movScript .counterCast , 0f , 0.1f , npcCastTime);//El jugador va en direccion opuesta a la de dash
                if ( movScript .counterCast < npcCastTime )
                    movScript .counterCast++;



            }
            else if ( castingLeft )
            {
                movScript .sideMoving = true;//El jugador se esta moviendo con el volante horizontalmente
                movScript .xAcc = +movScript .fCastDash(movScript .counterCast , 0f , 0.1f , npcCastTime);//El jugador va en direccion opuesta a la de dash

                if ( movScript .counterCast < npcCastTime )
                    movScript .counterCast++;



            }

            if ( movScript .counterCast == npcCastTime )
            {
                canDash = false;
                Invoke("dashAgain" , 5f);
                movScript .Invoke("recoverControll" , 0f);
                movScript .casting = false;//Ya no esta casteando
                movScript .dashing = false;//Ahora estara haciendo dash
                movScript .sideMoving = false;//El jugador se esta moviendo con el volante horizontalmente

                Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsA") .GetComponent<Renderer>() .material = lighsOn;
                Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("RedLightsB") .GetComponent<Renderer>() .material = lighsOn;

            }

            if ( Mathf .Abs(playerPos .localPosition .x - transform .localPosition .x) < 0.4f )
                if ( Mathf .Abs(playerPos .localPosition .y - transform .localPosition .y) < 0.07f )
                {
                    //Si esta dentro del alcance del dash del enemigo
                    movScript .sideMoving = false;
                    movScript .casting = false;//Ya no esta casteando
                    movScript .dashing = true;//Ahora estara haciendo dash


                }
        }
        else if ( movScript .dashing )//Si no se esta casteando pero si haciendo un dash
        {


            movScript .sideMoving = false;

            if ( playerPos .localPosition .x < transform .localPosition .x )
                movScript .rb .AddForce(new Vector2(-150 , 0));//Aplicamos la fuerza de dash

            if ( playerPos .localPosition .x > transform .localPosition .x )
                movScript .rb .AddForce(new Vector2(150 , 0));//Aplicamos la fuerza de dash

            movScript .Invoke("recoverControll" , 0.1f);
            Invoke("dashAgain" , 5f);
            canDash = false;
            castingRight = false;
            castingLeft = false;
        }
    }

    public void changeLane()
    {

        if ( laneOfCar == 1 )
        {

            {
                laneOfCar = 2;
                prevLane = 1;
                swichingLane = true;
            }
        }

        if ( laneOfCar == 2 )
        {

            {
                laneOfCar = 3;
                prevLane = 2;
                swichingLane = true;

            }

            {
                laneOfCar = 1;
                prevLane = 2;
                swichingLane = true;

            }
        }

        if ( laneOfCar == 3 )
        {

            {
                laneOfCar = 2;
                prevLane = 3;
                swichingLane = true;

            }
        }




    }

    void carAlignToLane()
    {

        // print("X" + npcPos.x + " Y " + npcPos.y + "Space " + Mathf.Abs(transform.localPosition.x - grid.GetComponent<GridCreator>().gridSlots[laneOfCar, (int)npcPos.y].transform.localPosition.x));

        //if (grid.GetComponent<GridCreator>().gridSlots[laneOfCar, (int)npcPos.y] != null)
        if ( Mathf .Abs(transform .localPosition .x - grid .GetComponent<GridCreator>() .gridSlots [laneOfCar , ( int )npcPos .y] .transform .localPosition .x) < 0.005 )
        {
            movScript .sideMoving = false;
            movScript .xAcc = 0f;
            swichingLane = false;

            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteL") .gameObject .SetActive(false);
            Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteR") .gameObject .SetActive(false);



        }
        else
        {

            //if (grid.GetComponent<GridCreator>().gridSlots[laneOfCar, (int)npcPos.y] != null)
            {
                if ( transform .localPosition .x - grid .GetComponent<GridCreator>() .gridSlots [laneOfCar , ( int )npcPos .y] .transform .localPosition .x > 0 )//Mover hacia la derecha
                {
                    if ( !occupiedLeft )
                    {
                        if ( movScript .canSideMove && movScript .xAcc == 0 )
                        {
                            {


                                movScript .sideMoving = true;

                                if ( Mathf .Abs(transform .localPosition .x - grid .GetComponent<GridCreator>() .gridSlots [laneOfCar , ( int )npcPos .y] .transform .localPosition .x) < 0.04 )
                                    movScript .xAcc = -0.002f;
                                else
                                {
                                    movScript .xAcc = -0.1f;
                                    Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteL") .gameObject .SetActive(true);
                                }

                            }
                        }
                    }
                    else
                    {
                        Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteL") .gameObject .SetActive(false);
                        movScript .sideMoving = false;
                        movScript .xAcc = 0;
                        if ( prevLane != 0 )

                            laneOfCar = prevLane;
                        dashAgain();

                    }
                }
                else
                if ( transform .localPosition .x - grid .GetComponent<GridCreator>() .gridSlots [laneOfCar , ( int )npcPos .y] .transform .localPosition .x < 0 )//Mover hacia la izquierda
                {
                    if ( !occupiedRight )
                    {
                        if ( movScript .canSideMove && movScript .xAcc == 0 )
                        {

                            {

                                movScript .sideMoving = true;

                                if ( Mathf .Abs(transform .localPosition .x - grid .GetComponent<GridCreator>() .gridSlots [laneOfCar , ( int )npcPos .y] .transform .localPosition .x) < 0.04 )
                                    movScript .xAcc = 0.002f;
                                else
                                {
                                    movScript .xAcc = 0.1f;
                                    Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteR") .gameObject .SetActive(true);
                                }


                            }
                        }
                    }
                    else
                    {
                        Chasis .GetComponent<chasisManager>() .currentCar .transform .Find("intermitenteR") .gameObject .SetActive(false);
                        movScript .sideMoving = false;
                        movScript .xAcc = 0;

                        if ( prevLane != 0 )
                            laneOfCar = prevLane;

                    }
                }
            }
        }



    }

    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other .gameObject .tag == "slot" )
        {
            npcPos = other .GetComponent<slotsControl>() .carSlot;
        }
    }

    private void OnCollisionEnter2D( Collision2D other )
    {
        if ( !movScript .playerDriving )
        {
            if ( other .gameObject .tag == "noControlando" )
            {
                movScript .canSideMove = false;
                movScript .sideMoving = false;
                movScript .xAcc = 0;
                movScript .Invoke("recoverControll" , 0.5f);
                //print("chasis colision");

                changeLane();
            }

            if ( other .gameObject .tag == "controlando" )
            {
                if ( other .gameObject .GetComponent<movimientoPruebas>() .dashing )
                {
                    if ( enemyShooter )
                    {
                        if ( weaponOutLeft && canUseLeftGun )
                        {
                            movScript .onAirLoot(( dropItems .itemsToDrop )shooterScript .enemyWeapon);
                            shooterScript .enemyWeapon = shooter .weaponType .empty;
                            weaponOutLeft = false;
                            canUseLeftGun = false;
                            HideEnemyGun();

                        }
                        else if ( weaponOutRight && canUseRightGun )
                        {
                            movScript .onAirLoot(( dropItems .itemsToDrop )shooterScript .enemyWeapon);
                            shooterScript .enemyWeapon = shooter .weaponType .empty;
                            weaponOutRight = false;
                            canUseRightGun = false;
                            HideEnemyGun();

                        }
                    }

                }

            }
        }


    }
}
