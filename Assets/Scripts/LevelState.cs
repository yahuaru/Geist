using UnityEngine;
using System.Collections;

public class LevelState : MonoBehaviour {

	public void Death()
    {
        Application.LoadLevel(Application.loadedLevel);
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
        if (Input.GetButtonDown("NextLevel"))
        {
            NextLevel();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LoadLevel(0);
        }
    }
}
