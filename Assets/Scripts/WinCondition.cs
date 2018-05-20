using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour {

    public GameControl gc;
    public int agentsInHere;
    public GameObject winMenu;
    public Text food, agents, passports;
    public AudioSource Visa;
    public AudioSource Rocket;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ganaste");
        gc.food += collision.gameObject.GetComponent<LocalInventory>().GetFood();
        gc.passport += collision.gameObject.GetComponent<LocalInventory>().GetPassports();
        gc.agents += agentsInHere;
        winMenu.SetActive(true);
        Time.timeScale = 0;
        food.text = collision.gameObject.GetComponent<LocalInventory>().GetFood() + " " + food.text;
        passports.text = collision.gameObject.GetComponent<LocalInventory>().GetPassports() + " " + passports.text;
        agents.text = agentsInHere + " " + agents.text;
        Visa.Pause();
        Rocket.Play();
    }

}
