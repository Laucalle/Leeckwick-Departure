using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    public GameObject PauseMenu;
    public GameObject fadeImage;

	// Use this for initialization
	void Start () {

        PauseMenu.SetActive(false);
        fadeImage.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKey(KeyCode.Escape))
        {
            Pause();
            PauseMenu.SetActive(true);
        }
		
	}

    public void Pause()
    {
        Time.timeScale = 0;
        fadeImage.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        fadeImage.SetActive(false);
    }
}
