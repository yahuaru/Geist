using UnityEngine;
using System.Collections;

public class HorizontalMove : MonoBehaviour {

	public float speed = 0;
	public float timer = 0;

	private float speed_ = 0;
	private float timer_ = 0;
	
	
	void Start() {
		timer_ = timer;
		speed_ = speed;
	}
	
	void FixedUpdate () {	
		if (timer_ > 0) {
			transform.Translate (Vector2.right * speed_ * Time.deltaTime);
			timer_ -= Time.deltaTime;
		}
		else {
			timer_ = timer;
			speed_ = -speed_;
		}
	}
}
