using System .Collections;
using System .Collections .Generic;
using UnityEngine;
using UnityEngine .SceneManagement;
using UnityEngine .Audio;

public class engineButton : MonoBehaviour
{

    public startMenuController menuController;
    public List<AudioClip> audioFX;

    public AudioSource engineSound;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if ( gameObject .name == "EngineExterior" )
            menuController .engineEnabled = !menuController .engineEnabled;

        if ( menuController .engineEnabled )
        {
            if ( gameObject .name == "PlayMESH" )
            {
                Invoke("loadGame" , 2f);
                engineSound .PlayOneShot(audioFX [0]);
                menuController .menuAnimator .SetBool("enteringGame" , true);
            }

            if ( gameObject .name == "<<MESH" )
            {
                if ( menuController .radioIndex == 1 )
                {
                    menuController .radioIndex = 3;
                    menuController .videoDisplay .clip = menuController .videos [4];
                    menuController .videoDisplay .playbackSpeed = 2f;
                    menuController .clipChanged = false;
                }
                else
                {
                    if ( menuController .radioIndex == 0 )
                    {
                        menuController .radioIndex = 3;
                        menuController .videoDisplay .clip = menuController .videos [4];
                        menuController .videoDisplay .playbackSpeed = 2f;
                        menuController .clipChanged = false;
                    }
                    else
                    {
                        menuController .radioIndex--;
                        menuController .videoDisplay .clip = menuController .videos [4];
                        menuController .videoDisplay .playbackSpeed = 2f;
                        menuController .clipChanged = false;
                    }
                }
            }

            if ( gameObject .name == ">>MESH" )
            {
                if ( menuController .radioIndex == 3 )
                {
                    menuController .radioIndex = 1;
                    menuController .videoDisplay .clip = menuController .videos [4];
                    menuController .videoDisplay .playbackSpeed = 2f;
                    menuController .clipChanged = false;
                }
                else
                {
                    if ( menuController .radioIndex == 0 )
                    {
                        menuController .radioIndex = 1;
                        menuController .videoDisplay .clip = menuController .videos [4];
                        menuController .videoDisplay .playbackSpeed = 2f;
                        menuController .clipChanged = false;
                    }
                    else
                    {
                        menuController .radioIndex++;
                        menuController .videoDisplay .clip = menuController .videos [4];
                        menuController .videoDisplay .playbackSpeed = 2f;
                        menuController .clipChanged = false;
                    }
                }
            }

            if ( gameObject .name == "vol-" )
            {
                if ( menuController .musicPlayer .musicVolume > 0f )
                    menuController .musicPlayer .musicVolume -= 0.1f;

            }

            if ( gameObject .name == "vol+" )
            {
                if ( menuController .musicPlayer .musicVolume < 1f)
                menuController .musicPlayer .musicVolume += 0.1f;


            }

            if ( gameObject .name == "mute" )
            {
                if ( menuController .musicPlayer .musicVolume == 0 )
                menuController .musicPlayer .musicVolume = 1;
                else
                    menuController .musicPlayer .musicVolume = 0;


            }

        }
    }

    void loadGame()
    {
        menuController .gameObject .SetActive(false);

        SceneManager .LoadScene("HotlineVaporwave" , LoadSceneMode .Single);
    }

    private void OnMouseEnter()
    {
        if ( menuController .engineEnabled )
        {
            //  if ( gameObject .name == "<<MESH" )
            gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [7];
            // if ( gameObject .name == ">>MESH" )
            //  gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [7];
            //if ( gameObject .name == "PlayMESH" )
            // gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [7];

        }

    }

    private void OnMouseExit()
    {
        //  if ( gameObject .name == "<<MESH" )
        gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [0];
        //  if ( gameObject .name == ">>MESH" )
        // gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [0];
        // if ( gameObject .name == "PlayMESH" )
        //  gameObject .GetComponent<MeshRenderer>() .material = menuController .materials [0];
    }
}
