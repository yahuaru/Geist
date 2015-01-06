using UnityEngine;
using System.Collections;

public class ChangeableObject : MonoBehaviour {

    public float changeTime = 10.0f;
    private float changeTimer;
    bool reverse = false;
	// Use this for initialization
	void Start () {
        changeTimer = changeTime;
	}
	
	// Update is called once per frame
	void Update () {
        if(reverse)
        {
            changeTimer += Time.deltaTime;
        }
        else
        {
            changeTimer -= Time.deltaTime;
        }
        if(changeTimer <= 0)
        {
            changeTimer = changeTime;

        }
        
	}
}
