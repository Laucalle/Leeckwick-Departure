using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour {

    Animator anim;
    EnemyController ec;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
        ec = GetComponent<EnemyController>();

    }

    // Update is called once per frame
    void Update () {

        Vector3 direction = ec.getDir();
        bool idle = ec.IsLookingAround();
        float aux_angle;

       aux_angle = Vector2.SignedAngle(direction, new Vector2(1, 0));
        if (aux_angle < 0) aux_angle = 360 + aux_angle;

        if(aux_angle > 315 || aux_angle <= 45)
        {

            if (idle) anim.Play("IdleRight");
            else anim.Play("RunningRight");


        } else if (aux_angle > 45 && aux_angle <= 135)
        {
            if (idle) anim.Play("IdleFront");
            else anim.Play("RunningFront");

        }
        else if (aux_angle > 135 && aux_angle <= 225)
        {
            if (idle) anim.Play("IdleLeft");
            else anim.Play("RunningLeft");

        } else
        {
            if (idle) anim.Play("IdleBack");
            else anim.Play("RunningBack");
        }



    }
}
