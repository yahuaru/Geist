using UnityEngine;
using System.Collections;

public class ZoneDetector : MonoBehaviour {
    private int inWhiteZones = 0;
    private int inBlackZones = 0;

	void OnTriggerExit2D(Collider2D objCollider2D)
    {
        if (objCollider2D.gameObject.layer == LayerMask.NameToLayer("White"))
        {
            inWhiteZones--;
        }
        if (objCollider2D.gameObject.layer == LayerMask.NameToLayer("Black"))
        {
            inBlackZones--;
        }
    }

    void OnTriggerEnter2D(Collider2D objCollider2D)
    {
        if (objCollider2D.gameObject.layer == LayerMask.NameToLayer("White"))
        {
            inWhiteZones++;
        }
        if (objCollider2D.gameObject.layer == LayerMask.NameToLayer("Black"))
        {
            inBlackZones++;
        }
    }

    public bool isInWhiteZone()
    {
        return inWhiteZones > 0 && inBlackZones == 0;
    }

    public bool isInBlackZone()
    {
        return inBlackZones > 0 && inWhiteZones == 0;
    }
}
