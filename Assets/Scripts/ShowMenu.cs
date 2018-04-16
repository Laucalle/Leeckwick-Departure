using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenu : MonoBehaviour {

    bool pressed, menuActive;
    GameObject menuPanel;
    GameObject imagePanel;
    Animator anim;
    float timeTrigger;

    // Use this for initialization
    void Start () {

        pressed = false;
        menuActive = false;
        anim = GetComponent<Animator>();

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            if (child.name == "MenuPanel") menuPanel = child.gameObject;
            if (child.name == "ImagePanel") imagePanel = child.gameObject;
        }

        imagePanel.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {

        if ( !pressed && Input.anyKey)
        {
            pressed = true;
            anim.SetTrigger("AnyKey");
            timeTrigger = Time.time;
            Debug.Log("KeyPressed");
        }

        if(pressed && !menuActive && Time.time - timeTrigger >= 2.5)
        {
            menuPanel.SetActive(true);
            menuActive = true;
        }

	}
}
