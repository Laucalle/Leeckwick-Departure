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

    // Use this for initialization
    void Start () {

        agents.text = ""+control.agents;
        passports.text = "" + control.passport;
        food.text = "" + control.food;
        if (control.passport < 1) send.interactable = false;
        if (control.food < 1) feed.interactable = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SendAgent()
    {
        control.SendAgent();
        agents.text = "" + control.agents;
        passports.text = "" + control.passport;

        if (control.passport < 1) send.interactable = false;

    }

    public void FeedAgents()
    {
        control.FeedAgents();
        food.text = "" + control.food;

        if (control.food < 1) feed.interactable = false;
    }
}
