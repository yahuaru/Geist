using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[ExecuteInEditMode]
public class LevelState : MonoBehaviour {

    public float deathTime = 0.5f;
    public bool isDead = false;

    private float deathTimer;

	public void Death()
	{
	    isDead = true;
	}

    void Start()
    {
        deathTimer = deathTime;
    }

    public void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }

    public void LoadLevel(int i)
    {
        Application.LoadLevel(i);
    }

    void Update()
    {

        if (Application.isEditor)
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);    
        }

        if (Input.GetButtonDown("NextLevel"))
        {
            NextLevel();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadLevel(0);
        }
        if (isDead)
        {
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
            deathTimer -= Time.deltaTime;
            if (deathTimer < 0)
            {
                deathTimer = deathTime;
                isDead = false;
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);    
                LoadLevel(Application.loadedLevel);
            }
        }
    }
}
