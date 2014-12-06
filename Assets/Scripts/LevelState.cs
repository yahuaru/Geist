using UnityEngine;
using System.Collections;

public class LevelState : MonoBehaviour {

    static int currentLevel = 0;

	public void Death()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    public void NextLevel()
    {
        Application.LoadLevel(currentLevel + 1);
    }

    public void LoadLevel(int i)
    {
        Application.LoadLevel(i);
    }
}
