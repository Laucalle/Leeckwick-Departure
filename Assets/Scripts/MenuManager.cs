using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour {

    public GameControl control;
    public Text agents;
    public Text passports;
    public Text food;
    public Button send;
    public Button feed;
    public Button start;

    // Use this for initialization
    void Start () {

        agents.text = ""+ GameControl.control.agents;
        passports.text = "" + GameControl.control.passport;
        food.text = "" + GameControl.control.food;
        if (GameControl.control.passport < 1) send.interactable = false;
        if (GameControl.control.food < 1) feed.interactable = false;
        if (GameControl.control.agents < 1) start.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendAgent()
    {
        GameControl.control.SendAgent();
        agents.text = "" + GameControl.control.agents;
        passports.text = "" + GameControl.control.passport;

        if (GameControl.control.passport < 1) send.interactable = false;
        if (GameControl.control.agents < 1) start.interactable = false;

    }

    public void FeedAgents()
    {
        GameControl.control.FeedAgents();
        food.text = "" + GameControl.control.food;

        if (GameControl.control.food < 1) feed.interactable = false;
        if (GameControl.control.agents < 1) start.interactable = false;
    }
}
