using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor : MonoBehaviour
{
    public List<GameObject> objectsToDestroy;
    public Collider2D destructorCollider;
    int objectCount;
    // Use this for initialization
    void Start()
    {
        objectCount = 0;
        destructorCollider = GetComponent<Collider2D>();
        objectsToDestroy.Capacity = objectCount;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {


        Destroy(other.gameObject);

    }

    private void OnTriggerExit2D(Collider2D other)
    {

       
     
 
      
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(other .gameObject);


    }

}
