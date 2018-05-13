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
        passport -= 1;
        agents -= 1;
    }

    public void FeedAgents()
    {
        food -= 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
