using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Character : MonoBehaviour
{
    public bool canChangeColor = false;
    bool wantToChangeColor = false;
    public bool onWhiteZone = true;
    public bool isBlack = true;
    public float gravityScale = 1.0f;
    SpriteRenderer sprite;

    

    //CharacterController2D controller;
    //NonPhysicsPlayerTester behaviour;
   
    //int blackLayer = 512;
    //int whiteLayer = 256;
    //int nothingLayer = 0;

    bool canJump = false;
    

    private Vector2 newVelocity;
    public float horizontalSpeed = 10.0f;
    public float jumpSpeed = 10.0f;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D.gravityScale = 0.0f;
        //controller = GetComponent<CharacterController2D>();
        //behaviour = GetComponent<NonPhysicsPlayerTester>();
        //controller.platformMask = blackLayer; 
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), !isBlack);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), isBlack);
    }

    // Update is called once per frame
    void Update()
    {
        newVelocity = rigidbody2D.velocity;
        if (canChangeColor)
        {
            if (Input.GetButtonDown("SwapColors") && !wantToChangeColor)
            {
                Debug.Log("Swaped");
                wantToChangeColor = true;
                isBlack = !isBlack;
                transform.position = transform.position;    
                if(isBlack)
                {
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), isBlack);
                }
                else
                {
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), !isBlack);
                }

                //controller.collisionState.becameGroundedThisFrame = controller.collisionState.above;
                //controller.platformMask = nothingLayer;
            }

            
        }

        if (wantToChangeColor && ((!onWhiteZone && !isBlack) || (onWhiteZone && isBlack)))
        {
            wantToChangeColor = false;
            //behaviour.gravity = -behaviour.gravity;
            gravityScale = -gravityScale;
            jumpSpeed = -jumpSpeed;
            if (isBlack)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);    
                //controller.platformMask.value = whiteLayer; 
                sprite.color = Color.black;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), !isBlack);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);    
                //controller.platformMask.value = blackLayer; 
                sprite.color = Color.white;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), isBlack);
            }
        }

        int layerMask = LayerMask.NameToLayer("Player");
        RaycastHit2D[] hits;
        if(isBlack)
        {
            hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 0.5f);
        }
        else
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.up, 0.5f);
        }
        foreach(RaycastHit2D hit in hits)
        {
            if(isBlack && hit.collider.gameObject.layer == LayerMask.NameToLayer("Black")
                || !isBlack && hit.collider.gameObject.layer == LayerMask.NameToLayer("White"))
            {
                if (isBlack && Vector2.Dot(hit.normal, Vector2.up) < 0.891
                    || !isBlack && Vector2.Dot(hit.normal, -Vector2.up) < 0.891)
                {
                    return;
                }
            }
        }

        Debug.DrawRay(transform.position, -Vector2.up * 0.5f, Color.green);
        

        newVelocity.x = Input.GetAxis("Horizontal") * horizontalSpeed;
        rigidbody2D.velocity = newVelocity;

        if (Input.GetButtonDown("Jump") && canJump)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.AddForce(Vector3.down * Physics2D.gravity.magnitude * gravityScale);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        
        if(collider.gameObject.layer == LayerMask.NameToLayer("Black") 
            || collider.gameObject.layer == LayerMask.NameToLayer("White"))
        {
            canJump = true;
        }
    }

    void Jump()
    {
        canJump = false;
        rigidbody2D.AddForce(Vector2.up * jumpSpeed);
    }
}
