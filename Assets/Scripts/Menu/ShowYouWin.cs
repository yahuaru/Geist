using UnityEngine;
using System.Collections;

public class ShowYouWin : MonoBehaviour
{

    public GameObject youWinImage;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
                        youWinImage.SetActive(true);
        }
    }
}
