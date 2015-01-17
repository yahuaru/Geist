using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject SpawnObject;
    public float Duration = 0.5f;
    private float _durationTimer;
    public int WavesByX = 30;
    public int Waves = 10;
    public float DistanceX = 1.0f;
    private List<Vector3> _positionsList;
    public float LeftBound = -7.0f;
    private ParallaxBackground _background;
    public float LowerBound = -5;

	// Use this for initialization
	void Start ()
	{
        _background = GetComponent<ParallaxBackground>();
	    _positionsList = new List<Vector3>();
	    _durationTimer = Duration;
	    Vector3 nextPosition = new Vector3(LeftBound, 0.0f, 0.0f);
        for (int index = 0; index < WavesByX; index++)
	    {
            nextPosition.y = Random.Range(LowerBound, LowerBound + 2.5f);
	        _positionsList.Add(nextPosition);
            GameObject obj = (GameObject)Instantiate(SpawnObject, nextPosition, Quaternion.identity);
	        obj.transform.parent = gameObject.transform;
	        nextPosition.x += DistanceX;
	    }
	}

    // Update is called once per frame
	void Update ()
	{
	    _durationTimer -= Time.deltaTime;
	    if (_durationTimer < 0 && Waves > 0)
	    {
	        Waves--;
	        _durationTimer = Duration;
	        foreach (var pos in _positionsList)
	        {
	            Vector3 newPos = pos;
                newPos.x += Random.Range(-DistanceX / 2, DistanceX / 2);
                newPos.y = Random.Range(LowerBound, LowerBound + 2.5f);
                GameObject obj = (GameObject)Instantiate(SpawnObject, newPos, Quaternion.identity);
                obj.transform.parent = gameObject.transform;

	            
	        }
	        if (_background != null) _background.SetLayers();
	    }
	}
}
