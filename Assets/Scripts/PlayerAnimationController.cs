using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {

    Animator anim;

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            anim.SetTrigger("Forward");
        } else if (Input.GetKey(KeyCode.S))
        {
            anim.SetTrigger("Backward");
        } else if (Input.GetKey(KeyCode.A))
        {
            anim.SetTrigger("Left");
        } else if (Input.GetKey(KeyCode.D))
        {
            anim.SetTrigger("Right");
        } else
        {
            anim.SetTrigger("Idle");
        }



    }
}
