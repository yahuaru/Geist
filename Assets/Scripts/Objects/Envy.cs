using UnityEngine;
using System.Collections;

public class Envy : MonoBehaviour {
	
	void Start () {
    }
	
	void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character != null)
        {
            character.Death();
        }
    }
}
