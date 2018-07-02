using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class wheels : MonoBehaviour
{

    public int wheelHealth;
    public GameObject tireDestroy;
    public GameObject thisWheel;
    // Use this for initialization
    void Start()
    {

        tireDestroy = GetComponentInParent<tireController>() .tireDestroyPrefab;


    }

    // Update is called once per frame
    void Update()
    {
        if ( gameObject .GetComponent<MeshRenderer>() .enabled )

            if ( wheelHealth <= 0 )
            {
                wheelDestroy();
            }

    }

    void wheelDestroy()
    {
        gameObject .GetComponent<BoxCollider2D>() .enabled = false;
        thisWheel .SetActive(false);

        transform .GetChild(0) .gameObject .SetActive(true);
        transform .GetChild(1) .gameObject .SetActive(true);
        transform .GetChild(2) .gameObject .SetActive(true);

        // Destroy(gameObject);
        if ( gameObject .name == "frontLeft" )
        {
            gameObject .GetComponentInParent<tireController>() .wheelFrontL = false;
            GameObject tireSprite = Instantiate(tireDestroy , transform .parent .GetChild(0) .position , Quaternion .identity);
            tireSprite .transform .parent = transform .parent;
            Destroy(tireSprite , 0.2f);
            GetComponent<wheels>() .enabled = false;
        }
        else if ( gameObject .name == "frontRight" )
        {
            gameObject .GetComponentInParent<tireController>() .wheelFrontR = false;
            GameObject tireSprite = Instantiate(tireDestroy , transform .parent .GetChild(1) .position , Quaternion .identity);
            tireSprite .transform .parent = transform .parent;
            Destroy(tireSprite , 0.2f);
            GetComponent<wheels>() .enabled = false;

        }
        else if ( gameObject .name == "backRight" )
        {
            gameObject .GetComponentInParent<tireController>() .wheelBackR = false;
            GameObject tireSprite = Instantiate(tireDestroy , transform .parent .GetChild(3) .position , Quaternion .identity);
            tireSprite .transform .parent = transform .parent;
            Destroy(tireSprite , 0.2f);
            GetComponent<wheels>() .enabled = false;
        }
        else if ( gameObject .name == "backLeft" )
        {
            gameObject .GetComponentInParent<tireController>() .wheelBackL = false;
            GameObject tireSprite = Instantiate(tireDestroy , transform .parent .GetChild(2) .position , Quaternion .identity);
            tireSprite .transform .parent = transform .parent;
            Destroy(tireSprite , 0.2f);
            GetComponent<wheels>() .enabled = false;

        }

    }



    private void OnTriggerEnter2D( Collider2D other )
    {
        if ( other .gameObject .tag == "Bullet" && gameObject .tag == "Tire" )
        {

            wheelHealth -= other .GetComponent<bullet>() .bulletDamage;

        }
    }
}
