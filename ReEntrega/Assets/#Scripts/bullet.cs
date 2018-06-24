using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public float bulletSpeed;
    public GameObject hitSprPrefab;
    public GameObject bulletPrefab;
    public int bulletDamage;
    Vector3 direction;
    Quaternion rotation;
    // Use this for initialization
    public List<AudioClip> audioFX;
    public AudioSource audioSource;

    void Start () {
        Invoke("destroyBullet", 2f);
        if(gameObject.name == "BulletPistol(Clone)")
        transform.Rotate(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        // rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        // direction = rotation * transform.localPosition;
        if (gameObject.name == "BulletShell(Clone)")
            transform.Rotate(Random.Range(-10, 10), 0, Random.Range(-10, 20));

    }
	
	// Update is called once per frame
	void Update () {
        audioSource = GetComponent<AudioSource>();

        transform .position += transform.right * Time.deltaTime * bulletSpeed;
        // transform.Translate(direction * Time.deltaTime * bulletSpeed);
    }

    void destroyBullet() {


        GameObject hitSprite =  Instantiate(hitSprPrefab, new Vector3(transform.position.x, transform.position.y, -0.036f), transform.rotation);

        Destroy(hitSprite, 0.3f);
        transform .GetComponentInChildren<SpriteRenderer>() .enabled = false;
        Destroy(gameObject,0.2f);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {


        //Instantiate(bulletPrefab,transform.localPosition,Quaternion.identity,other.transform);

        if ( other .gameObject .layer == 10 )
        {//layer coches

            other .gameObject .GetComponent<carHealth>() .receiveDamage(bulletDamage);
            audioSource .PlayOneShot(audioFX [0]);

        }
        else
        {
            audioSource .PlayOneShot(audioFX [1]);

        }



        destroyBullet();
    }

}
