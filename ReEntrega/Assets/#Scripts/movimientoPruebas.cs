using System;
using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class movimientoPruebas : MonoBehaviour
{

    public List<AudioClip> audioFX;
    public AudioSource audioSource;



    [HideInInspector] public float baseSpeed = 1;//Velocidad base

    public int noControlDamage;//Daño recibido/dado cuando un coche colisiona con otro mientras tiene una rueda DETRUIDA y se mueve en alguna direccion
    public int dashDamage;//Daño que causa el DASH
    public bool playerDriving;//Si el jugador esta usando este vehiculo en particular o no
    float speed = 1f, maxSpeed = 2f, minSpeed = 0.5f;//Velocidad base, velocidad maxima, velocidad minima
    public float maxVertSpeed = 1, maxHorizSpeed = 1f;

    [HideInInspector] public float xSpeed = 0f;//Velocidad base horizontal
    [HideInInspector] public float ySpeed = 1;//velocidad base vertical

    public float xAcc, yAcc;//Acelerador horizontal y acelerador vertical
    public float horizontalSpeed;//Velocidad de movimieto horizontal maxima

    public Vector2 carSlotLocation;//Posicion de este coche en los slots

    GridCreator grid;


    public GameObject sparkPrefab;//Prefab de los efectos de chispas
    public GameObject shooter;//El motor de disparo del juego, el arma 
    public GameObject cameraTarget;//Objeto que controla la camara del juego, se pone como hijo del coche que este controlado por el jugador
    public carHealth carHealth;//Componenr del script que controla la vida
    public screenShake shakeScript;//Componente del script que controla el SCREENSHAKE de la camara, la sacudida
    public Collider2D thisCollider;//Collider del CHASIS del objeto,cada coche tiene 2 colliders uno TRIGGER y otro normal.
    public GameObject playerJumPrefab;//Prefab del objeto que representa al jugador cuando salta para ir de un vehiculo a otro.
    public npcBehaviour npcBehaviourScript;

    public Transform jumpToLeft;//Posicion de donde sale el jugador cuando salta hacia la izquierda
    public Transform jumpToRight;//Posicion de donde sale el jugador cuando salta hacia la derecha

    #region Easing funciones
    //funciones de easing para poder asignarlas y cambiarlas desde el inspector.
    public Easing .Ease easeAcelerar;
    public Easing .Ease easeFrenar;
    public Easing .Ease easeAccelerarToNormal;
    public Easing .Ease easeFrenarToNormal;
    public Easing .Ease easeGiro;
    public Easing .Ease easeDash;
    public Easing .Ease easeCastDash;

    //Se asignan una funcion a la funcion renombrada equivalente 
    public Easing .Function fAcelerar;
    public Easing .Function fFrenar;
    public Easing .Function fAccelerarToNormal;
    public Easing .Function fFrenarToNormal;
    public Easing .Function fGiro;
    public Easing .Function fDash;
    public Easing .Function fCastDash;
    #endregion

    #region Counters
    //Contadores que se usan para las distintas mecanicas que usan easing
    float counterUp = 0;//Cuando estas acelerando hacia arriba
    float counterDown = 0;//Cuano estas acelerando hacia abajo (frenando)
    float counterLeft = 0;//Contador de movimiento horizontal hacia la izquierda
    float counterRight = 0;//Contador movimiento horizontal hacia la derecha
    public float counterCast = 0;//Contador del casteo de el dash/salto 

    float moveTimeX = 1f;//Valor maximo de los contadores de control horizontal
    float moveTimeY = 1f;//Valor maximo de los contadores de movimiento vertical
    float castTime = .75f;//Valor maximo del contador del dash/saltp
    #endregion



    public int collisionForce;//Fuerza aplicada en la colision de vehiculos
    public float recovContrTime;//Tiempo para que el coche vuelva a recobrar el control despues de una colision

    public bool canAccelerate = true;//Si el coche puede acelerar

    public bool canSideMove = true; //Si el coche puede moverse horizontalmente o no
    public bool sideMoving;//Si el coche SE ESTA moviendo horizontalmente
    public bool accelerating;//Si el coche esta ACELERANDO
    public bool braking;//Si el coche esta ACELERANDOs
    [HideInInspector] public bool dashRight, dashLeft;//Controlar hacia que lado se esta haciendo el DASH

    [HideInInspector] public float dashDistance = 0;//Distancia del recorrido del dash al ser casteado, se calcula en funcion del tiempo de casteo
    public float rbHorizSpeed, rbVerticalSpeed;//Velocidad del RIGIDBODY vertical y horizontal SE USA PARA EL DEBUG, NO TIENE FUNCIONALIDAD

    public bool dashing = false;//Si el coche esta haciendo un DASH
    public bool casting = false;//Si el coche esta CASTEANDO 
    public bool tireCollision = false;//Si el coche ya ha colisionado despues de haberse pinchado una rueda.
    public bool tireSliding = false;//Si el coche esta moviendose a consecuencia de tener una rueda pinchada y no haber chocado todavia

    [HideInInspector] public Rigidbody2D rb;//El componente de fisicas 2D del coche
    public int speedRelativeValue;//Valor de velocidad relativo, se usa para hacer pruebas de escalado, simplemente incrementa la velocidad relativamente NO TIENE FUNCIONALIDAD a parte de para pruebas.

    //Control del DOUBLE TAP para el DASH
    private float lastTapTimeLeft;//Tiempo desde el ultimo TAP hacia la izquierda
    private float lastTapTimeRight;//Tiempo desde el ultimo TAP hacia la derecha
    private float tapSpeed = 0.2f;//Tiempo maximo entre dos pulsaciones para ser considerado DOUBLE TAP
    private bool doubleTappedLeft = false;//Mirar si se ha tapeado hacia la izquierda 
    private bool doubleTappedRight = false;//Mirar si se ha tapeado hacia la derecha 

    [HideInInspector] public float castJumpTime;//Tiempo de casteo del salto
    [HideInInspector] public float castJumpPressTime;//Tiene el tiempo actual del sistema, se usa para comparar 
    [HideInInspector] public bool castJumpTimerRunning = false;//Si esta corriendo el temporizador del casteo del SALTO
    [HideInInspector] public Vector3 cacamTargetOGPos = new Vector3(0 , 0 , 0);//Vector que guarda las coordenadas iniciales de la camara del juego antes de empezar a hacer zoom, se usa para reestablecer la distancia original antes del zoom  de la camara
    [HideInInspector] public Vector3 camCastDashOGPos;

    public GameObject lootOnAir;
    public float tireRelativeSpeed = 0;

    private void Start()
    {
        Application .targetFrameRate = 60;//Limita los FPS del juego
        QualitySettings .vSyncCount = 0;//Quita la sincronizacion vertical

        rb = GetComponent<Rigidbody2D>();//Asigna y obtiene el componente rigidbody del coche
        grid = GameObject .Find("gridCreator") .GetComponent<GridCreator>();
        npcBehaviourScript = GetComponent<npcBehaviour>();

        //Asignacion de las funciones de EASING
        fAcelerar = Easing .GetEasingFunction(easeAcelerar);
        fFrenar = Easing .GetEasingFunction(easeFrenar);
        fAccelerarToNormal = Easing .GetEasingFunction(easeAccelerarToNormal);
        fFrenarToNormal = Easing .GetEasingFunction(easeFrenarToNormal);
        fGiro = Easing .GetEasingFunction(easeGiro);
        fDash = Easing .GetEasingFunction(easeDash);
        fCastDash = Easing .GetEasingFunction(easeCastDash);

        //Inicializa el contador de double tap a 0
        lastTapTimeRight = 0;
        lastTapTimeLeft = 0;
    }

    void Update()
    {
        if ( audioSource == null )
            audioSource = GetComponent<AudioSource>();

        if ( npcBehaviourScript == null )
            npcBehaviourScript = GetComponent<npcBehaviour>();

        if ( playerDriving )//Si el jugador esta controlando el coche
        {
            gameObject .tag = "controlando";//Se asigna al coche que controla el jugador un tag para reconocerlo
            shooter .SetActive(true);//Activa el sistema de disparo del coche
            //laneSpeed(true); //El coche del jugador no se vera influenciado por la velocidad del carril en el que esta.

            if ( counterDown < 0 )
                counterDown = 0;

            #region Player Inputs
            if ( Input .GetKeyDown(KeyCode .A) )
            {
                if ( ( Time .time - lastTapTimeLeft ) < tapSpeed )//Control del DOUBLE TAP, mira si se esta haciendo tap(girar) o Doble tap(casteo de DASH)
                {
                    if ( !accelerating && !braking )
                    {
                        doubleTappedLeft = true;
                        doubleTappedRight = false;
                    }
                }
                lastTapTimeLeft = Time .time;
            }
            if ( Input .GetKeyDown(KeyCode .D) )
            {
                if ( ( Time .time - lastTapTimeRight ) < tapSpeed )//Control del DOUBLE TAP, mira si se esta haciendo tap(girar) o Doble tap(casteo de DASH)
                {
                    if ( !accelerating && !braking )
                    {
                        doubleTappedRight = true;
                        doubleTappedLeft = false;
                    }
                }
                lastTapTimeRight = Time .time;
            }
            if ( Input .GetKeyUp(KeyCode .A) )
            {
                doubleTappedLeft = false;
                counterLeft = 0;
            }
            if ( Input .GetKeyUp(KeyCode .D) )
            {
                doubleTappedRight = false;
                counterRight = 0;
            }
            #endregion

            #region Aceleracion Vertical
            if ( canAccelerate )
                if ( Input .GetKey(KeyCode .W) && !Input .GetKey(KeyCode .S) )
                {
                    AccelerateCar(moveTimeY);
                }
            if ( Input .GetKeyUp(KeyCode .W) )//Al soltar la tecla dejamos de acelerar
            {
                accelerating = false;
            }
            #endregion

            #region Frenar / Decelerar
            if ( Input .GetKey(KeyCode .S) && !Input .GetKey(KeyCode .W) )
            {
                BrakeCar(moveTimeY);
            }
            #endregion

            #region Movimiento sin Input
            MovementCarNoInput(moveTimeY);
            #endregion

            if ( canSideMove )//El jugador puede moverse horizontalmente?
            {
                #region Movimiento Horizontal VOLANTE 
                if ( !doubleTappedRight )
                {
                    if ( Input .GetKey(KeyCode .D) )
                    {
                        HorizontalMovementCar(false , moveTimeX);
                    }
                }
                if ( !doubleTappedLeft )
                {
                    if ( Input .GetKey(KeyCode .A) )
                    {
                        HorizontalMovementCar(true , moveTimeX);
                    }
                }
                #endregion

                #region Sidemoving y movimiento horizontal

                if ( !Input .GetKey(KeyCode .A) && !Input .GetKey(KeyCode .D) )//Si el jugador no toca ninguna tecla de control del volante no se estara moviendo hacia los lados.
                {
                    if ( !casting )//Si el jugador no esta casteando el dash
                    {
                        if ( !playerTireSliding )
                        {
                            xAcc = 0;
                            sideMoving = false;
                        }
                        else
                        {
                            //sideMoving = true;
                            sideMoving = true;
                        }
                    }
                }
                if ( Input .GetKeyUp(KeyCode .A) || Input .GetKeyUp(KeyCode .D) )//Si alguna tecla de direccion esta pulsada el jugador se estara moviendo horizontalmente
                {
                    if ( !sideMoving )
                    {
                        rb .velocity = new Vector2(0 , rb .velocity .y);
                        canSideMove = true;
                    }
                }
                #endregion

                #region DASH 
                if ( !dashing ) //Si el jugador no esta haciendo un dash empieza a castear el dash
                {
                    if ( doubleTappedRight )
                        if ( Input .GetKeyDown(KeyCode .D) )
                        {
                            casting = true;
                            dashRight = true;
                            dashLeft = false;
                        }
                    if ( doubleTappedLeft )
                        if ( Input .GetKeyDown(KeyCode .A) )
                        {
                            casting = true;
                            dashRight = false;
                            dashLeft = true;
                        }
                }
                if ( casting )//Si se esta casteando
                {
                    DashCasting(castTime);
                }
                else if ( dashing )//Si no se esta casteando pero si haciendo un dash
                {
                    Dash();
                }
                //Dash
                if ( ( Input .GetKeyUp(KeyCode .D) && dashRight ) || ( Input .GetKeyUp(KeyCode .A) && dashLeft ) )//Si uno es verdadero y el otro es falso 
                {
                    if ( casting )//Si se estaba casteando la habilidad:
                    {
                        if ( counterCast > castTime / 1.8f )//Miramos si el tiempo de casteo al menos la mitad del tiempo total de casteo
                        {
                            casting = false;//Ya no esta casteando
                            dashing = true;//Ahora estara haciendo dash
                            dashDistance = ( ( 0.334f * counterCast ) / 100 );//Convertim el 30 de duracion a .1f i 60 a .2f ya que son las fuerzas calculadas optimas para la distancia deseada de dash
                            counterCast = 0;//Volvemos a poner el contador del casteo a 0 para la proxima vez
                        }
                        else//Si no se hace el casteo se cancela y el juagador pierde su velocidad horizontal momentaneamente como penalizacion 
                        {
                            casting = false;
                            dashing = false;
                            xAcc = 0;//Aceleracion horizontal
                            rb .velocity = new Vector2(0 , rb .velocity .y);//La velocidad de el motor de fisicas del coche 
                            counterCast = 0;
                        }
                    }
                }
                #endregion
            }

            #region Jump to car

            if ( Input .GetKeyDown(KeyCode .Q) )
            {
                canAccelerate = false;
                canSideMove = false;
                castJumpTimerRunning = true;
                castJumpPressTime = Time .time;
            }
            if ( Input .GetKeyUp(KeyCode .Q) )
            {
                JumpPlayer(true);
            }
            if ( Input .GetKeyDown(KeyCode .E) )
            {
                canAccelerate = false;
                canSideMove = false;
                castJumpTimerRunning = true;
                castJumpPressTime = Time .time;
            }
            if ( Input .GetKeyUp(KeyCode .E) )
            {
                JumpPlayer(false);
            }
            if ( castJumpTimerRunning )
            {
                JumpCast();
            }
            #endregion
        }
        else//NPC O ENEMIGO ESTA CONDUCIENDO
        {
            gameObject .tag = "noControlando";
            //gameObject.tag = "noControlando";//Se asigna el tag al coche que no esta siendo controlado por el jugador
            //shooter.SetActive(false);//Desactiva el arma del coche
            //laneSpeed(false);//La velocidad se ve influenciada por el carril en el que este situado el coche
        }
    }

    private void JumpCast()
    {
        castJumpTime = Time .time - castJumpPressTime;
        GameObject .Find("positionFollowPlayerCamera0") .transform .position += ( new Vector3(0 , -0.02f , 0.030f) ) / 3;//Hace zoom a la camara.
        cacamTargetOGPos -= ( new Vector3(0 , -0.02f , 0.030f) ) / 3;
        print("camara zoon");

        if ( castJumpTime > 1f )
        {
            GameObject .Find("positionFollowPlayerCamera0") .transform .position = GameObject .Find("positionFollowPlayerCamera0") .transform .position + cacamTargetOGPos;
            cacamTargetOGPos = new Vector3(0 , 0 , 0);
            canAccelerate = true;
            canSideMove = true;
            castJumpTimerRunning = false;
        }
    }

    private void JumpPlayer( bool isLeft )
    {
        GameObject .Find("positionFollowPlayerCamera0") .transform .position = GameObject .Find("positionFollowPlayerCamera0") .transform .position + cacamTargetOGPos;
        cacamTargetOGPos = new Vector3(0 , 0 , 0);
        canAccelerate = true;
        canSideMove = true;
        castJumpTimerRunning = false;

        if ( castJumpTime <= 1f && castJumpTime > 0.3f )
        {
            if ( isLeft )
            {
                GameObject jumper = Instantiate(playerJumPrefab , jumpToLeft .transform .position , Quaternion .identity);
                jumper .transform .parent = jumpToLeft;
                jumper .GetComponent<jumpPlayer>() .velocidadSalto = castJumpTime;
            }
            else
            {
                GameObject jumper = Instantiate(playerJumPrefab , jumpToRight .transform .position , Quaternion .identity);
                jumper .transform .parent = jumpToRight;
                jumper .GetComponent<jumpPlayer>() .velocidadSalto = castJumpTime;
            }
            castJumpTime = 0;
            castJumpTimerRunning = false;
            canAccelerate = true;
            canSideMove = true;
            playerDriving = false;
        }
        castJumpTime = 0;
    }

    private void Dash()
    {
        sideMoving = false;
        canSideMove = false;
        if ( dashRight )
        {
            rb .AddForce(new Vector2(150 , 0));//Aplicamos la fuerza de dash
            dashRight = false;
            dashing = true;
        }
        else if ( dashLeft )
        {
            rb .AddForce(new Vector2(-150 , 0));//Aplicamos la fuerza de dash
            dashLeft = false;
            dashing = true;
        }
        Invoke("recoverControll" , dashDistance * 2);//Llamamos a la funcion de recobrar el control del volante en el tiempo que hemos calculado mas arriba con el dashDistance y los counters.
    }

    private void DashCasting( float time )
    {
        float rate = 1 / time;
        sideMoving = true;//El jugador se esta moviendo con el volante horizontalmente
        yAcc = 0;
        counterUp = 0;
        if ( dashRight )//Si el jugador esta casteando un dash hacia la derecha
        {
            xAcc = -fCastDash(counterCast , 0 , 1 , time);//El jugador va en direccion opuesta a la de dash
            if ( counterCast < time )
                counterCast += rate * Time .deltaTime;
        }
        else if ( dashLeft )
        {
            xAcc = +fCastDash(counterCast , 0 , 1 , time);//El jugador va en direccion opuesta a la de dash
            if ( counterCast < time )
                counterCast += rate * Time .deltaTime;
        }
        if ( counterCast == time )//Si el tiempo de casteo llega a su maximo el jugador no hara dash y se quedara sin velocidad horizontal momentaneamente
        {
            xAcc = 0;
            casting = false;
            dashing = false;
            counterCast = 0;
            rb .velocity = new Vector2(0 , rb .velocity .y);
            dashLeft = false;
            dashRight = false;
        }
    }

    private void HorizontalMovementCar( bool goingLeft , float time )
    {
        float rate = 1 / time;

        sideMoving = true;//El jugador se esta moviendo hacia un lado
        if ( !goingLeft )
            xAcc = fGiro(counterRight , 0f , horizontalSpeed , time);
        else
            xAcc = fGiro(counterLeft , 0f , -horizontalSpeed , time);

        if ( !goingLeft )
        {
            if ( counterRight < time )
                counterRight += rate * Time .deltaTime;
            counterLeft = 0;//El contador de la acceleracion contraria se pone a 0 directamente
        }
        else
        {
            if ( counterLeft < time )
                counterLeft += rate * Time .deltaTime;
            counterRight = 0;
        }
    }

    private void MovementCarNoInput( float time )
    {
        float rate = 1 / time;
        //Si no se esta acelerando decelera el coche hasta vel normal (AUTOMATICAMENTE)
        if ( rbVerticalSpeed > speed && !Input .GetKey(KeyCode .W) )
        {
            yAcc = fFrenarToNormal(counterUp , 0f , maxVertSpeed , time);
            if ( counterUp > 0 )
                counterUp -= rate * Time .deltaTime;
        }
        //Si no se esta frenando acelera al coche a velocidad normal (AUTOMATICAMENTE)
        if ( rbVerticalSpeed < speed && !Input .GetKey(KeyCode .S) )
        {
            yAcc = -fAccelerarToNormal(counterDown , 0f , 0.5f , time);
            if ( counterDown > 0 )
                counterDown -= rate * Time .deltaTime;
        }
        if ( Input .GetKeyUp(KeyCode .S) )//Al soltar la tecla dejamos de acelerar
        {
            braking = false;
        }


    }

    private void BrakeCar( float time )
    {
        float rate = 1 / time;
        //Frenar desde velocidad normal
        braking = true;
        if ( rbVerticalSpeed > minSpeed && rbVerticalSpeed <= speed )
        {
            yAcc = -fFrenar(counterDown , 0f , 0.5f , time);
            if ( counterDown < time )
                counterDown += rate * Time .deltaTime;
        }
        //Frenar desde velocidad maxima
        if ( rbVerticalSpeed > speed && rbVerticalSpeed <= maxSpeed )
        {
            yAcc = fFrenar(counterUp , 0f , maxVertSpeed , time);
            if ( counterUp > 0 )
                counterUp -= rate * Time .deltaTime;
        }
    }

    private void AccelerateCar( float time )
    {
        float rate = 1 / time;
        accelerating = true;//El coche esta acelerando
        //Acelerar desde velocidad normal
        if ( rbVerticalSpeed >= 0 )//Miramos si la velocidad del componente RIGIDBODY es mas grande o igual que 0, velocidad vertical
        {
            yAcc = fAcelerar(counterUp , 0f , 1f , time);//Funcion de Easing para la acceleracion vertical yAcc
            if ( counterUp < moveTimeY )//Contador para cuantificar y transmitir el tiempo transcurrido acelerando
                counterUp += rate * Time .deltaTime;
        }
        //Acelerar desde velocidad minima
        if ( rbVerticalSpeed < speed )
        {
            yAcc = -fAcelerar(counterDown , 0f , maxVertSpeed , time);
            if ( counterDown > 0 )
                counterDown -= rate * Time .deltaTime;
        }
    }

    void FixedUpdate() //Actualiza el juego frame a frame pero este bucle se encarga de las fisicas del juego.
    {
        rbVerticalSpeed = rb .velocity .y;
        rbHorizSpeed = rb .velocity .x;

        if ( sideMoving )//Si el jugador se esta moviendo horizontalmente
        {
            if ( canAccelerate )//Si puede acelerar
            {
                rb .drag = 0;//Friccion 
                rb .velocity = speedRelativeValue * new Vector2(xAcc + tireHorizRelatSpeed , ySpeed + ( yAcc - tireVertRelatSpeed ));//Velocidad del coche
            }
            else//Si no puede acelerar
            {
                rb .drag = 1;//Friccion 
                rb .velocity = speedRelativeValue * new Vector2(xAcc + tireHorizRelatSpeed , rb .velocity .y);//Velocidad del coche
            }
        }
        else//Si el jugador no se esta moviendo horizontalmente
        {
            if ( canAccelerate )//Si puede acelerar
            {
                if ( accelerating )//Si esta Accelerando
                {
                    rb .drag = 0;//Friccion 
                    rb .velocity = speedRelativeValue * new Vector2(rb .velocity .x , ySpeed + ( yAcc - tireVertRelatSpeed ));//Velocidad del coche
                }
                else//Si no esta accelerando
                {
                    rb .drag = 1;//Friccion 
                    rb .velocity = speedRelativeValue * new Vector2(rb .velocity .x , ySpeed + ( yAcc - tireVertRelatSpeed ));//Velocidad del coche
                }
            }
            else//Si no puede acelerar
            {
                rb .drag = 1;//Friccion 
                rb .velocity = speedRelativeValue * new Vector2(xAcc + tireHorizRelatSpeed , rb .velocity .y);//Velocidad del coche
            }
        }
    }

    public void laneSpeed( bool controlling )//Control de la velocidad de los coches dependiendo del carril en el que esten
    {
        if ( controlling )//Si esta controlado por el jugador la velocidad de cada carril no afecta al jugador
        {
            ySpeed = baseSpeed;//Velocidad base vertical es la misma 
        }
        else
        {
            //Control de la velocidad dependiendo de la linea
            if ( carSlotLocation .x == 1 )
                ySpeed = baseSpeed + 0.15f;
            else if ( carSlotLocation .x == 2 )
                ySpeed = baseSpeed;
            else if ( carSlotLocation .x == 3 )
                ySpeed = baseSpeed - 0.15f;
        }
    }

    public void getNPCSlot( Vector2 slotIndex )//Accede al INDICE del SLOT de la carretera en la que se encuentra el coche
    {
        carSlotLocation = slotIndex;
    }

    bool tireDirSetted = false;
    float tireHorizRelatSpeed = 0;
    float tireVertRelatSpeed = 0;
    bool playerTireSliding = false;

    public void tireMovement( bool FL , bool FR , bool BL , bool BR , bool tiresBreaked )//Controla el estado de las ruedas del coche y el movimiento que genera su destruccion
    {   //Cambio de velocidades al destruirse una rueda
        tiresDamaged = tiresBreaked;
        //  if ( !playerDriving )

        if ( FL && FR )
        {
            tireSliding = false;
        }
        else
        {

            if ( !FL ^ !FR )
            {

                if ( gameObject .tag == "noControlando" )
                {
                    if ( !tireCollision )
                    {
                        tireSliding = true;
                        if ( !FL )
                        {
                            npcBehaviourScript .canChangeLane = false;
                            sideMoving = true;
                            xAcc = fGiro(counterLeft , 0f , -0.5f , moveTimeX);
                            if ( counterLeft < moveTimeX )
                                counterLeft++;
                        }

                        if ( !FR )
                        {
                            npcBehaviourScript .canChangeLane = false;
                            sideMoving = true;
                            xAcc = fGiro(counterRight , 0f , 0.5f , moveTimeX);
                            if ( counterRight < moveTimeX )
                                counterRight++;
                        }
                    }
                }

                if ( gameObject .tag == "controlando" )
                {
                    /*  if ( !FL )
                      {
                          tireHorizRelatSpeed = 0.1f;
                          sideMoving = true;
                          canSideMove = true;
                          playerTireSliding = true;
                      }

                      if ( !FR )
                      {
                          tireHorizRelatSpeed =-0.1f;
                          sideMoving = true;
                          playerTireSliding = true;
                          canSideMove = true;

                      }*/
                }

            }
            else if ( !FL && !FR )
            {
                if ( gameObject .tag == "noControlando" )
                {

                }
                else
                {
                    yAcc = 0;
                }
            }
        }

        if ( !BL || !BR )
        {
            if ( gameObject .tag == "noControlando" )
                baseSpeed = 0.8f;
            else
                maxVertSpeed = 0.5f;
            if ( ( !BL && !BR ) )
            {
                if ( gameObject .tag == "noControlando" )
                    baseSpeed = 0.6f;
                else
                    maxVertSpeed = 0.2f;

                if ( !FL && !FR )
                {
                    // carHealth .carDestroy();
                }
            }
        }
    }

    public void onAirLoot( dropItems .itemsToDrop weapon )
    {
        GameObject dropObject = Instantiate(lootOnAir , transform .position , transform .rotation);
        dropObject .GetComponent<dropItems>() .itemToDrop = weapon;
    }

    void recoverControll()//Devuelve el control del coche al conductor (npc o jugador)
    {
        canSideMove = true;//Puede moverse horizontalmente
        canAccelerate = true;//Puede acelerar verticalmente 
        dashDistance = 0;//Distancia de dash se pone a 0 ya que la accion ya ha terminado
        dashing = false;//Ya no se esta dasheando
        casting = false;
        counterCast = 0;
        xAcc = 0;//El """VOLANTE""" vuelve a ir recto 
        rb .velocity = new Vector2(0 , rb .velocity .y);//Se elimina la velocidad horizontal residual
    }

    bool tiresDamaged;
    float recovControlNPC;


    #region COLISIONES DEL COCHE

    void OnCollisionEnter2D( Collision2D other )
    {
        //11 LAYER DE WALLS
        if ( other .gameObject .layer == 11 )
        {
            audioSource .PlayOneShot(audioFX [4] , 1f);


            GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .softColl;
            GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;
            counterLeft = 0;
            counterRight = 0;
            canSideMove = false;
            sideMoving = false;
            xAcc = 0;

            if ( other .gameObject .tag == "WallsRight" )
            {
                rb .AddForce(new Vector2(-collisionForce * 2 , 0));
                gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage / 2);
                rb .velocity = new Vector2(rb .velocity .x , rb .velocity .y);
                if ( tireSliding )
                {
                    tireCollision = true;
                    npcBehaviourScript .canChangeLane = true;
                }
                Invoke("recoverControll" , recovContrTime);
            }
            else
            {
                rb .AddForce(new Vector2(collisionForce * 2 , 0));
                gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage / 2);
                rb .velocity = new Vector2(rb .velocity .x , rb .velocity .y);
                if ( tireSliding )
                {
                    tireCollision = true;
                    npcBehaviourScript .canChangeLane = true;
                }
                Invoke("recoverControll" , recovContrTime);
            }
        }

        //10 es igual a Cars layer
        if ( other .gameObject .layer == 10 )
        {

            audioSource .PlayOneShot(audioFX [( UnityEngine .Random .Range(5 , 7) )] , 1f);


            npcBehaviourScript .changeLane();
            Vector3 contactPoint = other .contacts [0] .point;

            GameObject hitSprite = Instantiate(sparkPrefab , new Vector3(contactPoint .x , contactPoint .y , -0.036f) , transform .rotation);
            Destroy(hitSprite , 0.3f);
            Vector3 center = other .collider .bounds .center;

            bool right = contactPoint .x > center .x;
            bool top = contactPoint .y > center .y;

            if ( other .gameObject .GetComponent<movimientoPruebas>() .dashing )
            {
                onAirLoot(dropItems .itemsToDrop .empty);
                onAirLoot(dropItems .itemsToDrop .empty);
                onAirLoot(dropItems .itemsToDrop .empty);
            }

            if ( other .gameObject .tag == "controlando" )
            {
                audioSource .PlayOneShot(audioFX [UnityEngine .Random .Range(0 , 2)]);

            }

            if ( thisCollider .bounds .center .y - other .collider .bounds .center .y >= -thisCollider .bounds .size .y && thisCollider .bounds .size .y >= thisCollider .bounds .center .y - other .collider .bounds .center .y )
            {//Centro print("centro");
                GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .softColl;
                GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;
                counterLeft = 0;
                counterRight = 0;
                canSideMove = false;
                sideMoving = false;
                xAcc = 0;

                if ( right )
                {
                    if ( dashing )
                    {
                        // other .rigidbody .AddForce(new Vector2(-collisionForce * 2 , 0));
                        rb .velocity = new Vector2(0 , rb .velocity .y);
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(dashDamage);//Dar daño al otro jugador
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = false;
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .hardColl;
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;
                    }
                    else
                    {
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage);
                        if ( other .transform .tag == "controlando" )
                            other .rigidbody .AddForce(new Vector2(-collisionForce * 5 , 0));

                        else
                            other .rigidbody .AddForce(new Vector2(-collisionForce , 0));
                        Invoke("recoverControll" , recovContrTime);

                    }

                    rb .velocity = new Vector2(rb .velocity .x , rb .velocity .y);

                }
                else
                {

                    if ( dashing )
                    {
                        //other .rigidbody .AddForce(new Vector2(collisionForce *2, 0));
                        rb .velocity = new Vector2(0 , rb .velocity .y);
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(dashDamage);//Dar daño al otro jugador
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = false;
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .hardColl;
                        GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;

                        audioSource .PlayOneShot(audioFX [8] , 1f);
                    }
                    else
                    {
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage);
                        if ( other .transform .tag == "controlando" )
                            other .rigidbody .AddForce(new Vector2(collisionForce * 5 , 0));

                        else
                            other .rigidbody .AddForce(new Vector2(collisionForce , 0));
                    }
                    rb .velocity = new Vector2(rb .velocity .x , rb .velocity .y);
                    Invoke("recoverControll" , recovContrTime);
                }
            }
            else
            {
                GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .softColl;
                GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;
                counterUp = 0;
                counterDown = 0;
                accelerating = false;
                canAccelerate = false;
                yAcc = 0;

                if ( !top )//Colision por arriba
                {
                    other .gameObject .GetComponent<Rigidbody2D>() .AddForce(new Vector2(0 , collisionForce * 2));
                    rb .AddForce(new Vector2(0 , -collisionForce * 2));
                    if ( tiresDamaged )
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage);
                    else
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage / 3);


                    rb .velocity = new Vector2(0 , rb .velocity .y);
                    Invoke("recoverControll" , recovContrTime);
                    npcBehaviourScript .changeLane();

                }
                else//Colision por abajo
                {
                    other .gameObject .GetComponent<Rigidbody2D>() .AddForce(new Vector2(0 , -collisionForce * 2));
                    rb .AddForce(new Vector2(0 , collisionForce * 2));
                    if ( tiresDamaged )
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage);
                    else
                        other .gameObject .GetComponent<carHealth>() .receiveDamage(noControlDamage / 3);

                    rb .velocity = new Vector2(0 , rb .velocity .y);
                    Invoke("recoverControll" , recovContrTime);
                    npcBehaviourScript .changeLane();

                }
            }

            if ( counterCast != 0 )
            {
                counterCast = castTime;
            }

            if ( tireSliding )
            {
                tireCollision = true;
                gameObject .GetComponent<carHealth>() .receiveDamage(dashDamage);//Daño por colision
                other .gameObject .GetComponent<carHealth>() .receiveDamage(dashDamage);
                GameObject .FindGameObjectWithTag("controlando") .GetComponent<movimientoPruebas>() .shakeScript .shakers = screenShake .shakersPrefabs .hardColl;
                GameObject .FindGameObjectWithTag("controlando") .GetComponent<movimientoPruebas>() .shakeScript .enabled = true;

                onAirLoot(dropItems .itemsToDrop .empty);
                onAirLoot(dropItems .itemsToDrop .empty);
                onAirLoot(dropItems .itemsToDrop .empty);

                audioSource .PlayOneShot(audioFX [8] , 1f);
            }
        }
    }

    private void OnCollisionStay2D( Collision2D other )
    {/*
        if ( ( thisCollider .bounds .center .x - other .collider .bounds .center .x ) > 0 )
        {
            if ( other .gameObject .tag == "noControlando" )
            {
                other .gameObject .GetComponent<Rigidbody2D>() .AddForce(new Vector2(-collisionForce , 0));
                rb .AddForce(new Vector2(collisionForce , 0));
                canSideMove = false;//Puede moverse horizontalmente
                canAccelerate = false;//Puede acelerar verticalmente 
                Invoke("recoverControll" , recovContrTime);
            }
        }
        else
        {
            if ( other .gameObject .tag == "noControlando" )
            {
                other .gameObject .GetComponent<Rigidbody2D>() .AddForce(new Vector2(0 , collisionForce));
                rb .AddForce(new Vector2(-collisionForce , 0));
                canSideMove = false;//Puede moverse horizontalmente
                canAccelerate = false;//Puede acelerar verticalmente 
                Invoke("recoverControll" , recovContrTime);
            }
        }*/
    }

    #endregion//EVENTO QUE DETECTA LA COLISION DE LOS COLIDERS DEL COCHE

}