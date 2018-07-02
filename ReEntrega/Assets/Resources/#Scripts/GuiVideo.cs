using UnityEngine;
using UnityEngine .UI;
using System .Collections;

public class GuiVideo : MonoBehaviour
{

    void Start()
    {
        MovieTexture movie = GetComponent<RawImage>() .texture as MovieTexture;
        //AudioSource sound = GetComponent<AudioSource>();
        //sound .clip = movie .audioClip;
        movie .Play();
        movie .loop = true;
        //sound .Play();
    }
}
