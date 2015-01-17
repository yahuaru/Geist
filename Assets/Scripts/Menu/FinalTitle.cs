using UnityEngine;
using System.Collections;

public class FinalTitle : MonoBehaviour
{


    public GameObject chararcter;
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;
    public SpriteRenderer specialThanks;
    public float opacitySpeed = 5;


    private Color tempColor;
    private bool becomeNotOpacity;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            chararcter.GetComponent<CharacterController>().enabled = false;
            var v = chararcter.rigidbody2D.velocity;
            v.x = 0;
            chararcter.rigidbody2D.velocity = v;
            chararcter.GetComponent<Character>().SwapColors();
            becomeNotOpacity = true;
            tempColor = sprite1.color;
        }
    }

    void FixedUpdate()
    {
        if (becomeNotOpacity)
        {
            tempColor.a += (Time.deltaTime * opacitySpeed / 100);
            sprite1.color = tempColor;
            sprite2.color = tempColor;
            specialThanks.color = tempColor;
            
        }
    }
}
