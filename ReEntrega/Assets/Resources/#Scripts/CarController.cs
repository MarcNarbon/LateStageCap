using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    float _doubleTapTimeA;
    float _doubleTapTimeD;

    public float dashSpeed;
    float counter = 0;
    public Transform arrowIndicator;
    Transform arrowTemporal;

    public GridCreator script;
    public float speed;
    public int playerX, playerY;
    float posX, posZ;

    private Rigidbody2D rb;

    public bool controlling;
    public int health;
    int random;

    public jumpPlayer jumpPlayer;
    public Material[] playerSkin;

    void Start()
    {
        posX = transform.position.x;
        posZ = transform.position.z;

        script = FindObjectOfType<GridCreator>();

    }
    //Inputs de movimiento y accion del jugador

    void nextGrid() {//Inicia un grid delante del jugador

        //script.InitializeGrid(script.slotSpace * (script.slotsY - 1));//Iniciamos otro grid
        playerY = 0;
    }

    void playerInputs()
    {
        //////////////////////////
        //Movimiento del jugador//
        //////////////////////////

        if (Input.GetKey(KeyCode.W))
        {
            if (script.gridSlots[playerX, playerY + 1].GetComponent<slotsControl>().occupied == false)//Si el slot que el jugador va a moverse esta libre el jugador se movera alli
            {
                playerY++;
            }

          //  if (transform.position.z == ((script.slotSpace * 1.5f) * (script.slotsY - 2)))//Miramos si esta en el penultimo slot Y 
            {
                nextGrid();
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (script.gridSlots[playerX, playerY - 1].GetComponent<slotsControl>().occupied == false)//Si el slot que el jugador va a moverse esta libre el jugador se movera alli
            {
                playerY--;
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (script.gridSlots[playerX - 1, playerY].GetComponent<slotsControl>().occupied == false)//Si el slot que el jugador va a moverse esta libre el jugador se movera alli
            {
                playerX--;
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (script.gridSlots[playerX + 1, playerY].GetComponent<slotsControl>().occupied == false)//Si el slot que el jugador va a moverse esta libre el jugador se movera alli
            {
                playerX++;
            }
        }

        script.gridSlots[playerX, playerY].GetComponent<slotsControl>().occupied = true;//Si el jugador se esta moviendo/coche el slot objetivo al que van se pone en ocupado para que ningun otro coche vaya hacia el y colisionen

        ////////////////////
        //Dash del jugador//
        ////////////////////


        if (Input.GetKeyDown(KeyCode.E))
        {
            arrowTemporal = Instantiate(arrowIndicator, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), arrowIndicator.transform.rotation);
            arrowTemporal.transform.parent = gameObject.transform;
        }

        if (Input.GetKey(KeyCode.E))
        {
            counter += Time.deltaTime * 1;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (counter < 1 && counter > 0)
            {
                Destroy(arrowTemporal.gameObject, 0f);
            }
            Destroy(arrowTemporal.gameObject, 0f);
            if (counter > 1 && counter < 2)
            {
                GetComponentInChildren<Animator>().SetTrigger("DashRight");
            }
            Destroy(arrowTemporal.gameObject, 0f);
            if (counter > 2 && counter < 3)
            {
                GetComponentInChildren<Animator>().SetTrigger("DashRight");
            }
            Destroy(arrowTemporal.gameObject, 0f);
            counter = 0;
        }

        /////////////////////
        //Salto del jugador//
        /////////////////////

        if (Input.GetKeyDown(KeyCode.F))
        {
            arrowTemporal = Instantiate(arrowIndicator, new Vector3(transform.position.x, transform.position.y + 4, transform.position.z), arrowIndicator.transform.rotation);
            arrowTemporal.transform.parent = gameObject.transform;
        }

        if (Input.GetKey(KeyCode.F))
        {
            counter += Time.deltaTime * 1;
        }


        if (Input.GetKeyUp(KeyCode.F))
        {
            if (counter < 1 && counter > 0)
            {
                Destroy(arrowTemporal.gameObject, 0f);

            }
            Destroy(arrowTemporal.gameObject, 0f);
            if (counter > 1 && counter < 2)
            {
                jumpPlayer jumper = Instantiate(jumpPlayer, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                jumper.transform.parent = gameObject.transform;
            }
            Destroy(arrowTemporal.gameObject, 0f);
            if (counter > 2 && counter < 3)
            {
                jumpPlayer jumper = Instantiate(jumpPlayer, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
                jumper.transform.parent = gameObject.transform;
            }
            Destroy(arrowTemporal.gameObject, 0f);
            counter = 0;

        }
    }

    //Comportamiento de coche generico
    void npcBehaviour()
    {

        //MOVIMIENTO ALEATORIO CASILLAR
         random = Random.Range(0,500);//Provabilidad 1 entre 400 de que se mueva (cada frame se tiran los dados)
        if (random == 1)
        {
            print(random);
            random = Random.Range(0, 4);

            if (random == 0)
            {
                if (script.gridSlots[playerX, playerY + 1].GetComponent<slotsControl>().occupied == false)
                {
                    playerY++;
                }
            }
            else if (random == 1)
            {
                if (script.gridSlots[playerX, playerY - 1].GetComponent<slotsControl>().occupied == false)
                {
                    playerY--;
                }
            }
            else if (random == 2)
            {
                if (script.gridSlots[playerX + 1, playerY].GetComponent<slotsControl>().occupied == false)
                {
                    playerX++;
                }
            }
            else if (random == 3)
            {
                if (script.gridSlots[playerX - 1, playerY].GetComponent<slotsControl>().occupied == false)
                {
                    playerX--;
                }
            }
        }

            script.gridSlots[playerX, playerY].GetComponent<slotsControl>().occupied = true; //Si el jugador se esta moviendo/coche el slot objetivo al que van se pone en ocupado para que ningun otro coche vaya hacia el y colisionen



    }

    //Destruccion del coche
    void carDestroy()
    {
        Destroy(gameObject);
    }

    void Update()
    {

        float step = speed * Time.deltaTime;//Velocidad de transicion de carril
        transform.position = Vector3.MoveTowards(transform.position, script.gridSlots[playerX, playerY].transform.position, step);//El coche va hacia la direccion del slot que defina playerX, playerY

        if (controlling)//Si el coche esta siendo controlado por el jugador
        {
            gameObject.GetComponentInChildren<Animator>().enabled = true;
            tag = "controlando";//El tag de este objeto se asigna 

            if (posX == transform.position.x && posZ == transform.position.z)//Si el jugador no se esta moviendo
            {
                //Movimiento del jugador
                playerInputs();
            }
        }
        else//Controlado por la maquina, NPC
        {
            gameObject.GetComponentInChildren<Animator>().enabled = false;
            tag = "noControlando";//El tag de este objeto se asigna 
            if (posX == transform.position.x && posZ == transform.position.z) {
                //Control de la IA
                npcBehaviour();
            }

        }

        //Al terminar todas las acciones se asignan las coordenadas del coche actuales para luego ver si el coche se esta moviendo o no
        posX = transform.position.x;
        posZ = transform.position.z;

        //Cuando el coche llega a 0 de vida
        if (health <= 0)
        {
            carDestroy();
        }
    }

    private void OnCollisionEnter(Collision other)//Colision con otro coche, en la embestida se aplica daño
    {
        if (other.gameObject.layer == 13 && other.gameObject.GetComponent<CarController>().controlling == false)
        {
            print("Child Colision");
            other.gameObject.GetComponent<CarController>().health -= 1;
            print("Player health:" + health);
            print("Other player health:" + other.gameObject.GetComponent<CarController>().health);

        }



    }

}
