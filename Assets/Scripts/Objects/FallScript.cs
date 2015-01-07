using UnityEngine;
using System.Collections;

public class FallScript : MonoBehaviour {
	
	public GameObject envy;
	public float time = 0;
	public float speed = 0;

	private Vector2 position_;
	private float speed_ = 0;
	private float time_ = 0;
	
	void Start () {
		speed_ = speed;
		time_ = time;
		position_ = transform.position;
	}

	void Update () {
		transform.Translate (-Vector2.up * speed_ * Time.deltaTime);
		time_ -= Time.deltaTime;

		if(time_ < 0)
		{
		    transform.position = position_;
		    time_ = time;
		}
	}
}