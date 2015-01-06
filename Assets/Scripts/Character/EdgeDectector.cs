using UnityEngine;
using System.Collections;

public class EdgeDectector : MonoBehaviour {

    public GameObject player;

	void OnTriggerStay2D(Collider2D collider)
    {

        if(collider.gameObject.layer == LayerMask.NameToLayer("Black") ||
            collider.gameObject.layer == LayerMask.NameToLayer("White"))
            player.GetComponent<Character>().canChangeColor = true;
    }
}
