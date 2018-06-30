using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class jumpPlayer : MonoBehaviour
{


    public float velocidadSalto;
    Transform parentPos;
    public Transform jumpToLeft;
    public Transform jumpToRight;
    public GameObject fleshExplosionPrefab;
    public GameObject cameraTarget;
    // Use this for initialization
    float detaTimeOG;
    pauseMenu menuPausa;



    void Start()
    {
        menuPausa = FindObjectOfType<pauseMenu>() .GetComponent<pauseMenu>();

        velocidadSalto = velocidadSalto / 100;

        if ( transform .parent .name == "jumperLeft" )
        {
            velocidadSalto = -1.4f * velocidadSalto;

        }
        else if ( transform .parent .name == "jumperRight" )
        {
            velocidadSalto = 1.4f * velocidadSalto;
            transform .localScale *= -1;

        }

        Time .timeScale = 0.1f;


    }

    // Update is called once per frame

    void Update()
    {
        if( menuPausa ==  null)
            menuPausa = FindObjectOfType<pauseMenu>() .GetComponent<pauseMenu>();


        transform .position += new Vector3(velocidadSalto , 0 , 0.003f);
        velocidadSalto -= velocidadSalto / 10;

        if ( transform .position .z > 0 )
        {
            playerDestroy(false);
            Time .timeScale = 1.0f;
            GameObject .Find("cameraTarget") .transform .parent = gameObject .transform;
            GameObject .Find("cameraTarget") .transform .position = transform .position;
        }
    }

    public void playerDestroy( bool alive )
    {

        transform .position = new Vector3(transform .position .x , transform .position .y , -0.03f);

        if ( alive )
        {
            gameObject .transform .DetachChildren();
            Destroy(gameObject);
        }
        else
        {
            gameObject .transform .DetachChildren();
            transform .GetComponentInParent<jumpPlayer>() .enabled = false;
            Instantiate(fleshExplosionPrefab , new Vector3(transform .position .x , transform .position .y , -0.0345f) , transform .rotation);
            GameObject .Find("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .explosion;
            GameObject .Find("cameraTarget") .GetComponent<screenShake>() .enabled = true;
            transform .GetComponentInParent<Collider2D>() .enabled = false;

            transform .parent .DetachChildren();


            menuPausa .Invoke("Pause" , 1f);
        }
    }

    private void OnTriggerEnter2D( Collider2D other )
    {

        Time .timeScale = 1.0f;
        if ( other .gameObject .layer == 10 && other .gameObject .GetComponent<movimientoPruebas>() .playerDriving == false )
        {
            //gameObject.transform.parent = other.transform;
            other .gameObject .GetComponent<movimientoPruebas>() .playerDriving = true;

            GameObject .Find("cameraTarget") .transform .parent = other .gameObject .GetComponent<movimientoPruebas>() .transform;
            GameObject .Find("cameraTarget") .transform .position = other .gameObject .GetComponent<movimientoPruebas>() .transform .position;

            GetComponentInParent<carHealth>() .Invoke("carDestroy" , 0.3f);//Para motivar que el jugador no se lance demasiado cerca del otro coche.


            playerDestroy(true);

        }





    }


}


