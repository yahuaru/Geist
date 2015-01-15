using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ZoneDetector))]
public class DangerStar : MonoBehaviour
{

    private ZoneDetector zoneDetector;
    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start ()
	{
	    zoneDetector = GetComponent<ZoneDetector>();
	    spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (zoneDetector.isInBlackZone())
	    {
	        spriteRenderer.color = Color.white;
	    }
        else if (zoneDetector.isInWhiteZone())
        {
            spriteRenderer.color = Color.black;   
        }
	}
}
