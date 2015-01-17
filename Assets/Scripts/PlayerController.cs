using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public float movementSpeed;
	public float jumpHeight;

	
	private float smoothedMovementFactor = 20;
	private float gravityScale = 3;
	private bool isInAir = false;
	private Vector2 newVelocity;
	private Vector2 playerMovement;
	private bool isGrounded;



	void Movement() {
		newVelocity = rigidbody2D.velocity;
		newVelocity.x = Mathf.Lerp(newVelocity.x, Input.GetAxis("Horizontal") * movementSpeed, Time.deltaTime * smoothedMovementFactor);
		rigidbody2D.velocity = newVelocity;
	}
	
	
	void Jump() {
		if(Input.GetButtonDown("Jump") && !isInAir){
			rigidbody2D.AddForce(Vector2.up * jumpHeight);
		}
	}

	void AddGravityForce() {
		rigidbody2D.AddForce (Vector3.down * Physics2D.gravity.magnitude * gravityScale);
	}

	void Update(){

		Movement();
		
		Jump ();
		
	}
	
	void FixedUpdate () {
		AddGravityForce ();
	}


}
