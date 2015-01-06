using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Player") {
			Application.LoadLevel("Stage-Demo");
		}
	}
}
