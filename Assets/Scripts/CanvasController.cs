using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public GameObject canvas;
    public GameObject fadeImage;
    public GameObject pauseMenu;
    public GameObject camera;

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
        canvas.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + 10);
        pauseMenu.SetActive(true);
        canvas.SetActive(true);
        fadeImage.SetActive(true);

    }

    public void Resume()
    {
        canvas.transform.position = new Vector3(0, 0, 0);
        Time.timeScale = 1;
        canvas.SetActive(false);
        fadeImage.SetActive(false);
    }
}
