using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip [ ] jazz;
    public AudioClip [ ] trap;
    public AudioClip [ ] rare;
    public AudioSource audioSource;
    public startMenuController menuController;


    public int currentList = 0;
    public float musicVolume = 0.5f;

    private static bool created = false;

    void Awake()
    {

            DontDestroyOnLoad(gameObject);
            created = true;


    }

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        audioSource .loop = false;
        Random .seed = System .Environment .TickCount;

    }

    public AudioClip GetRandomClip()
    {
        if ( currentList == 1 )
            return trap [Random .Range(0 , trap .Length)];
        if ( currentList == 2 )
            return jazz [Random .Range(0 , jazz .Length)];
        if ( currentList == 3 )
            return rare [Random .Range(0 , rare .Length)];
        else
            return null;
    }

    // Update is called once per frame
    void Update()
    {
        if ( !audioSource .isPlaying )
        {
            audioSource .clip = GetRandomClip();
            audioSource .Play();
        }

        if ( !menuController .engineEnabled )
        {
            audioSource .Stop();
        }

        if ( audioSource .volume != musicVolume )
            audioSource .volume = musicVolume;

    }
}
