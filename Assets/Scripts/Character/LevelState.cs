using UnityEngine;
using System.Collections;
using XInputDotNetPure;
using System;
using InControl;


[ExecuteInEditMode]
public class LevelState : MonoBehaviour {

    public float deathTime = 0.5f;
    public bool isDead = false;
    public Vector3 checkpoint;
    public GameObject character;

	Animator animator;
	private float deathTimer;

	private void RestartLevel() 
    {
		Application.LoadLevel(Application.loadedLevel);
	}

	public void Death() 
    {
		isDead = true;
		animator.SetTrigger("IsDead");
	}


    void Start() 
    {
        deathTimer = deathTime;
		animator = GetComponent<Animator> ();
		checkpoint = new Vector2 (11, 1);
    }

    void Update() 
    {
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		} 
    }

   
}
