using UnityEngine;
using System.Collections;

public class TrackingEye : MonoBehaviour {
	
	public GameObject player;
	public float speed = 5f;
	private Vector3 target;
	public Transform origin;
	
	void Start () {
		target = player.transform.position;
	}
	
	void Update () {

		target.z = transform.position.z;
		
		transform.position = Vector3.MoveTowards(origin.position, target, speed * Time.deltaTime);
	}   
}