using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {


    public GameObject fadeImage;
    public GameObject pauseMenu;


    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Pause();
        }

    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        fadeImage.SetActive(true);

    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        fadeImage.SetActive(false);
    }
}
