using UnityEngine;
using System.Collections;

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

        
        if (isDead)
        {
            character.rigidbody2D.velocity = Vector2.zero;
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                deathTimer = deathTime;
                isDead = false;
                character.transform.position = checkpoint;
                if (character.GetComponent<Character>().IsWhite)
                {
                    character.GetComponent<Character>().forceChangeColor = true;
                    character.GetComponent<Character>().SwapColors();
                }
            }
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
