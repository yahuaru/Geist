using UnityEngine;
using System.Collections;
using XInputDotNetPure;

//using XInputDotNetPure;

[ExecuteInEditMode]
public class LevelState : MonoBehaviour {

    public float deathTime = 0.5f;
    public bool isDead = false;

    private float deathTimer;
    public Vector3 checkpoint;
    public GameObject character;

	public void Death()
	{
	    isDead = true;
        character.GetComponent<Animator>().SetTrigger("IsDie");
	}

    void Start()
    {
        deathTimer = deathTime;
        checkpoint = character.transform.position;
    }

    void Update()
    {

        if (Application.isEditor)
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);    
        }
        if (isDead)
        {
            character.rigidbody2D.velocity = Vector2.zero;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                deathTimer = deathTime;
                isDead = false;
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);    
                character.transform.position = checkpoint;
                character.GetComponent<Character>().forceChangeColor = !character.GetComponent<Character>().isBlack;
            }
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);    
        }
        if (Input.GetButton("Restart"))
        {
            //Destroy(character);
            //Instantiate(character, checkpoint, Quaternion.identity);
            Application.LoadLevel(Application.loadedLevel);
        }
        if (Input.GetButton("NextLevel"))
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }

    public void LoadLevel(int n)
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
