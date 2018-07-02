using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionScene : MonoBehaviour {


    bool sceneStarted = false;
    Scene gameScene;
    // Use this for initialization
    void Start () {
        sceneStarted = false;
      
    }
    private void Awake()
    {


    }

    void LoadSceneMode() {


    }

	// Update is called once per frame
	void Update () {

       
            sceneStarted = true;
            SceneManager.LoadScene(1);
            SceneManager.UnloadScene(0);
        

    }
}
