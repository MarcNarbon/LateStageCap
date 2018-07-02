using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class pauseMenu : MonoBehaviour {

    public  bool gamePaused = false;

    public GameObject uiMenuPause;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (gamePaused)
            {

                Resume();
            }
            else {

                Pause();
            }


        }


	}

    public void Resume() {
        Cursor.visible = false;
        uiMenuPause.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Pause() {
        Cursor.visible = true;
        uiMenuPause.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;

    }

    public void ExitGame() {
        Time .timeScale = 1f; 
        SceneManager .LoadScene("menuInicio",LoadSceneMode.Single);
    }

    public void RestartGame()
    {
        if (Time.time > 1f)
        {
            Time .timeScale = 1f;
            SceneManager .LoadScene("HotlineVaporwave",LoadSceneMode.Single);

        }
    }

}
