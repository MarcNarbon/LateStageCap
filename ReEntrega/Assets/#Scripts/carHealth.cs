using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carHealth : MonoBehaviour {

    public float health;
     public float maxHealth;
    public GameObject explosionPrefab;
    public GameObject lootInstantiate;
    // Use this for initialization
    pauseMenu menuPausa;

    void Start() {
         maxHealth = health;
        menuPausa = FindObjectOfType<pauseMenu>() .GetComponent<pauseMenu>();

    }

    // Update is called once per frame
    void Update() {

        if (health <= 0) {
            carDestroy();
            GameObject.FindGameObjectWithTag("cameraTarget").GetComponent<screenShake>().enabled = false;
            GameObject.FindGameObjectWithTag("cameraTarget").GetComponent<screenShake>().shakers = screenShake.shakersPrefabs.explosion;
            GameObject.FindGameObjectWithTag("cameraTarget").GetComponent<screenShake>().enabled = true;
        }
        
        
    }

    bool exploded = false;

    GameObject explosionContainer;
    public void carDestroy() {

        if(gameObject.tag == "controlando")
            menuPausa .Invoke("Pause" , 1f);

        if ( !exploded )
        {
            Invoke("Explosion" , 0.2f);
            explosionContainer = Instantiate(explosionPrefab , new Vector3(transform .position .x , transform .position .y , -0.1f) , transform .rotation,gameObject.transform);
            Destroy(explosionContainer , 2f);
            exploded = true;
        }
    }

    void Explosion() {
        explosionContainer .transform .parent .DetachChildren();
        Destroy(gameObject);
        GameObject .FindGameObjectWithTag("cameraTarget") .GetComponent<screenShake>() .shakers = screenShake .shakersPrefabs .explosion;
        GameObject .FindGameObjectWithTag("cameraTarget") .GetComponent<screenShake>() .enabled = true;

        Instantiate(lootInstantiate , transform .position , transform .rotation);
        Instantiate(lootInstantiate , transform .position , transform .rotation);
        Instantiate(lootInstantiate , transform .position , transform .rotation);
        Instantiate(lootInstantiate , transform .position , transform .rotation);

    }

    public void receiveDamage(int damage) {

        health -= damage;

       
    }
}
