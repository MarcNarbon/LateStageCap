using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehaviour : MonoBehaviour
{
    public int health;
    private int cooldown = 50;
    public float baseSpeed;
    public float diferential;
    float acceleration;
    bool hitted;
    private Rigidbody2D rb;

    float _doubleTapTimeA;
    float _doubleTapTimeD;

    //acceleracion progresiva
    public float acc;
    float progresiveAcc = 0.5f;
    public float maxSpeed = 60.0f;
    private float curSpeed = 0.0f;

    public float dashSpeed;
    float dashTimer = 0;
    public bool controlando;
    public GameObject camara;
    private GameObject target;

    char lastdash = 'n';

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        acceleration = acc;
        GameObject target = transform.GetChild(0).gameObject;
        if (controlando)
        {

        }
        else
        {

        }
    }

    void Update()
    {
        if (controlando)
        {
            //camara.GetComponent<CameraFollow>().target = target.transform;

            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            gameObject.tag = "controlando";
            baseSpeed = 35;

        }
        else
        {
            gameObject.tag = "noControlando";

            if (lastdash == 'n')
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            baseSpeed = 50;
        }


        if (cooldown > 0)
        {
            cooldown--;
            print(cooldown);
        }

    }

    void FixedUpdate()
    {
        //Velocidad sin input
        rb.AddForce(Vector2.up * baseSpeed);

        if (controlando)
        {
            if (cooldown == 0)
            {

                //#region doubleTapD

                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (Time.time < _doubleTapTimeD + .3f)
                    {
                        if (dashTimer <= 1f)
                        {
                            rb.AddForce(Vector2.left * dashSpeed);
                            cooldown = 30;
                            lastdash = 'l';
                        }
                        dashTimer += 1f;
                    }
                    else { dashTimer = 0; }
                    _doubleTapTimeD = Time.time;
                }

                //#endregion

                //#region doubleTapA

                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (Time.time < _doubleTapTimeA + .3f)
                    {
                        if (dashTimer <= 1f)
                        {
                            rb.AddForce(Vector2.right * dashSpeed);
                            cooldown = 30;
                            lastdash = 'r';


                        }
                        dashTimer += 1f;
                    }
                    else { dashTimer = 0; }
                    _doubleTapTimeA = Time.time;
                }

                //#endregion
            }


            if (Input.GetKeyDown(KeyCode.LeftShift))
            {

                acceleration = acceleration * 2;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                acceleration = acc;
            }

            //Volante direccional
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(Vector2.up * curSpeed);
                curSpeed += progresiveAcc;
                if (curSpeed > maxSpeed)
                    curSpeed = maxSpeed;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                curSpeed = 0.0f;
            }


            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector2.down * acceleration / 2.5f);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector2.left * acceleration / diferential);
            }
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector2.right * acceleration / diferential);
            }

        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        CarBehaviour otherScript = other.gameObject.GetComponent<CarBehaviour>();
        health--;

        if (lastdash == 'l')
        {
            rb.AddForce(Vector2.right * dashSpeed / 3);
            lastdash = 'n';
        }
        else if (lastdash == 'r')
        {
            rb.AddForce(Vector2.left * dashSpeed / 3);
            lastdash = 'n';
        }

        if (controlando)
        {
            if (otherScript.health == 3)
            {
                controlando = false;
                otherScript.controlando = true;

            }

            if (health <= 0)
            {
                Destroy(gameObject, 0);
            }


        }
        else
        {



        }





        //targetGameObject.SetActive(!targetGameObject.activeSelf);

        //targetGameObject.transform.parent = other.transform;
        //CameraFollow camaraScript = camara.GetComponent<CameraFollow>();
        //camaraScript.target.position = collision.gameObject.transform.position;

    }
}



