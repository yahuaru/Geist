using UnityEngine;
using System.Collections;

public class Envy : MonoBehaviour {

    private LevelState levelState;
	// Use this for initialization
	void Start () {
        levelState = GameObject.FindObjectOfType<LevelState>();
	}
	
	void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            levelState.Death();
        }
    }
}
