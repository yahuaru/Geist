using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;


[ExecuteInEditMode]
public class LevelState : MonoBehaviour {

    public float deathTime = 0.5f;
    public bool isDead = false;
    public Vector3 checkpoint;
    public GameObject character;

	Animator animator;
	private float deathTimer;

	private void RestartLevel() {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void Death() {
		isDead = true;
		animator.SetTrigger("IsDead");
	}


    void Start() {
        deathTimer = deathTime;
		animator = GetComponent<Animator> ();
		checkpoint = new Vector2 (11, 1);
    }

    void Update() {

        if (isDead) {	

            character.rigidbody2D.velocity = Vector2.zero;
            deathTimer -= Time.deltaTime;
            
			if (deathTimer < 0) {
                deathTimer = deathTime;
				isDead = false;

				//RestartLevel();
				//character.GetComponent<Character>().gravityScale = Mathf.Abs(character.GetComponent<Character>().gravityScale);  
				character.GetComponent<Character>().forceChangeColor = character.GetComponent<Character>().IsWhite;
				character.transform.position = checkpoint;


            }
        }

		if (Input.GetKeyDown("r")) {
			RestartLevel();
		}
		if(Input.GetKeyDown("q")){
			Application.Quit();
		}

        
    }

   
}
