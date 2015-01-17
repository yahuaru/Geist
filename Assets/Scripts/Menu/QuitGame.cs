using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag == "Player") {
			Application.Quit();
		}
	}
}
