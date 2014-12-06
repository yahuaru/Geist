using UnityEngine;
using System.Collections;

public class ZoneDetector : MonoBehaviour {

    public GameObject player;

	void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("White"))
        {
            player.GetComponent<Character>().onWhiteZone = false;
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Black"))
        {
            player.GetComponent<Character>().onWhiteZone = true;
        }
    }
}
