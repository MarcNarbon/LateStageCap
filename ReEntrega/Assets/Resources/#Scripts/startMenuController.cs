using System .Collections;
using System .Collections .Generic;
using UnityEngine;
using UnityEngine .UI;
using UnityEngine .Video;
using UnityEngine .SceneManagement;


public class startMenuController : MonoBehaviour
{
    public List<AudioClip> audioFX;
    public AudioSource engineOn;
    public AudioSource engineSound;


    public Animator menuAnimator;

    public List<VideoClip> videos;

    public SoundManager musicPlayer;
    public VideoPlayer videoDisplay;

    public GameObject ScreenRadio;
    public GameObject ScreenBars;
    public GameObject carInteriorLights;
    public GameObject engineExterior;
    public GameObject engineInterior;

    public Text textMeshRadio;

    public List<GameObject> lightButtons;

    public List<Material> materials;

    public bool lightsOn = false;
    public bool engineEnabled = false;
    public int radioIndex = 0;

    private static bool created = false;

    void Awake()
    {
        if ( !created )
        {
            created = true;
        }
    }

    // Use this for initialization
    void Start()
    {


    }

    private IEnumerator coroutine;

    bool engineSounds = false;

    public bool clipChanged = false;
    // Update is called once per frame
    void Update()
    {

        if ( lightsOn )
            openLights();
        else
            closeLights();

        if ( engineEnabled )
            enableEngine();
        else
            disableEngine();

        if ( !engineEnabled )
        {
            if ( Input .GetKey("escape") )
                Application .Quit();
        }


        if ( radioIndex == 0 )
        {
            textMeshRadio .text = ( "Select radio channel" );
            videoDisplay .clip = videos [0];
            musicPlayer .currentList = 0;
        }
        else
        {
            if ( radioIndex == 2 )
            {
                textMeshRadio .text = ( "Asteroid Blues" );
                videoDisplay .loopPointReached += changeVideo;
                musicPlayer .currentList = 2;
            }
            else if ( radioIndex == 3 )
            {
                textMeshRadio .text = ( "Ridin' N' Da Projects" );
                videoDisplay .loopPointReached += changeVideo;
                musicPlayer .currentList = 1;
            }
            else if ( radioIndex == 1 )
            {
                textMeshRadio .text = ( "420 | 現代のコンピュ" );
                videoDisplay .loopPointReached += changeVideo;
                musicPlayer .currentList = 3;
            }
        }
    }


    IEnumerator ChangeAfterDelay( VideoPlayer vp )
    {
        if ( !clipChanged )
        {
      
            if ( radioIndex == 2 )
            {
                videoDisplay.clip = videos [1];
                musicPlayer .audioSource .Stop();
                musicPlayer .GetRandomClip();

            }
            else if ( radioIndex == 1 )
            {
                videoDisplay .clip = videos [3];
                musicPlayer .audioSource .Stop();
                musicPlayer .GetRandomClip();
            }
            else if ( radioIndex == 3 )
            {
                videoDisplay .clip = videos [2];
                musicPlayer .audioSource .Stop();
                musicPlayer .GetRandomClip();
            }
            clipChanged = true;

        }

        yield return null;


    }

    void changeVideo( VideoPlayer vp )
    {
        videoDisplay .playbackSpeed = 1f;

        if ( !clipChanged )
        {
            StartCoroutine(ChangeAfterDelay(videoDisplay));

        }


    }


    void disableEngine()
    {
        menuAnimator .SetBool("on" , false);
        lightsOn = false;
        radioIndex = 0;
        /* engineExterior .GetComponent<MeshRenderer>() .material = materials [5];
         engineInterior .GetComponent<MeshRenderer>() .material = materials [3];*/

        if ( engineSounds )
        {
            engineOn .Stop();
            engineSound .Stop();
            engineSounds = false;
        }



    }

    void enableEngine()
    {
        menuAnimator .SetBool("on" , true);
        lightsOn = true;
        /*engineExterior .GetComponent<MeshRenderer>() .material = materials [2];
        engineInterior .GetComponent<MeshRenderer>() .material = materials [1];*/
        if ( !engineSounds )
        {
            engineOn .Play();
            engineSound .clip = audioFX [0];
            engineSound .PlayDelayed(.2f);
            engineSound .loop = true;
            engineSounds = true;
        }


    }

    void closeLights()
    {
        for ( int i = 0 ; i < lightButtons .Capacity ; ++i )
        {
            // lightButtons [i] .GetComponent<MeshRenderer>() .material = materials [0];

        }
    }

    void openLights()
    {
        for ( int i = 0 ; i < lightButtons .Capacity ; ++i )
        {
            // lightButtons [i] .GetComponent<MeshRenderer>() .material = materials [1];

        }
    }


}
