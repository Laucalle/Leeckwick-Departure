﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    float _speed;
    [SerializeField]
    float _speedIncrement;

    bool _run;
    bool _audible;
    public Room _currentRoom;
    public GameControl gc;
    public GameObject looseMenu;
    public AudioSource Visa;
    public AudioSource Slottskogen;

    private Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        _run = false;
        _audible = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isAudible() {
        return _audible;
    }

    public Room getRoom()
    {
        return _currentRoom;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<EnemyController>() != null)
        {
            Debug.Log("Perdiste");
            GameControl.control.agents -= 1;
            Visa.Pause();
            Slottskogen.Play();
            Time.timeScale = 0;
            looseMenu.SetActive(true);
        }

    }

    private void FixedUpdate()
    {
        float actualSpeed;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if (Input.GetKey("left shift") && movement.magnitude != 0)
        {
            actualSpeed = _speed * _speedIncrement;
            _run = true;
            _audible = true;
        }
        else
        {

            actualSpeed = _speed;
            _run = false;
            _audible = false;
        }

        rb2d.velocity = movement.normalized * actualSpeed;

    }
}
