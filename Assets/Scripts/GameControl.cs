using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    public static GameControl control;

    public int passport;
    public int food;
    public int agents;

	// Use this for initialization
	void Awake () {

        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;

        } else if(control != this)
        {
            Destroy(gameObject);
        }
		
	}

    public void SendAgent()
    {
        control.passport -= 1;
        control.agents -= 1;
    }

    public void FeedAgents()
    {
        control.food -= 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
