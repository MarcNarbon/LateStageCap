using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{

    public List<GameObject> smokeTrail;
    public carHealth carHealth;
    public SpriteRenderer sprHealthBar;


    // Use this for initialization
    void Start()
    {

        carHealth = GetComponentInParent<carHealth>();

    }

    // Update is called once per frame
    void Update()
    {

        if (carHealth.health == carHealth.maxHealth)
        {
            sprHealthBar.enabled = false;
        }
        else
        {
            sprHealthBar.enabled = true;

        }

        if (carHealth.health >= 0)
        {

            barResize(carHealth.health);


        }

        smokeTrailControll();


    }
    void smokeTrailControll()
    {
        if (carHealth.health < ((carHealth.maxHealth * 90) / 100))
        {
            smokeTrail[0].SetActive(true);
        }
        if (carHealth.health < ((carHealth.maxHealth * 80) / 100))
        {
            smokeTrail[1].SetActive(true);
        }
        if (carHealth.health < ((carHealth.maxHealth * 70) / 100))
        {
            smokeTrail[2].SetActive(true);

        }
        if (carHealth.health < ((carHealth.maxHealth * 60) / 100))
        {
            smokeTrail[3].SetActive(true);
            smokeTrail[4].SetActive(true);
        }
        if (carHealth.health < ((carHealth.maxHealth * 50) / 100))
        {
            smokeTrail[5].SetActive(true);
            smokeTrail[6].SetActive(true);
        }
        if (carHealth.health < ((carHealth.maxHealth * 40) / 100))
        {
            smokeTrail[6].SetActive(true);
            smokeTrail[7].SetActive(true);
           
            smokeTrail[8].SetActive(true);


        }

    }

    void barResize(float health)
    {
        transform.localScale = new Vector3(health / 100, 1, 1);

    }

}
