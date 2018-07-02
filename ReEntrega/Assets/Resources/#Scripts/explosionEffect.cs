using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionEffect : MonoBehaviour {

    Collider2D explosionArea;
    public int explosionDamage;
    // Use this for initialization
    public List<AudioClip> audioFX;
    public AudioSource audioSource;

    void Start () {
        audioSource = GetComponent<AudioSource>();

        explosionArea = GetComponent<Collider2D>();
        audioSource .PlayOneShot(audioFX [1],1f);
        audioSource .PlayOneShot(audioFX [2] , 1f);

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<carHealth>().receiveDamage(explosionDamage);//Dar daño al otro jugador
        GameObject.Find("cameraTarget").GetComponent<screenShake>().shakers = screenShake.shakersPrefabs.hardColl;
        GameObject.Find("cameraTarget").GetComponent<screenShake>().enabled = true;
    }

}
