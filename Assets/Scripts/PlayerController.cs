using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: running key, makes noise

public class PlayerController : MonoBehaviour {
    [SerializeField]
    float speed;

    private Rigidbody2D rb2d;
	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public bool isAudible() {
        return true;
    }
    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Vector2 movement = new Vector2(transform.position.x + (moveHorizontal * speed), transform.position.y + (moveVertical * speed));
        rb2d.velocity = movement*speed;
        //rb2d.MovePosition(movement);
        //rb2d.AddForce(movement*speed);
        //transform.position = new Vector2(transform.position.x+(moveHorizontal*speed), transform.position.y + (moveVertical*speed));
    }
}
