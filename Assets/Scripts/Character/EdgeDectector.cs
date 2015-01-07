using UnityEngine;
using System.Collections;

public class EdgeDectector : MonoBehaviour
{

    private bool nearZoneEdge = false;

    public bool NearZoneEdge
    {
        get { return nearZoneEdge; }
    }

    void OnTriggerStay2D(Collider2D objCollider)
	{
        nearZoneEdge = (objCollider.gameObject.layer == LayerMask.NameToLayer("Black")) || 
            (objCollider.gameObject.layer == LayerMask.NameToLayer("White"));
	}
}
