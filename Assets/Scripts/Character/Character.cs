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

    public float runDamping = 20.0f;
    public float airDamping = 5.0f;

    private Vector2 newVelocity;
    public float horizontalSpeed = 10.0f;
    public float jumpSpeed = 10.0f;

    private ZoneDetector _zoneDetector;
    private GameObject _floor;
    private Vector3 _floorPrevPos;
    private bool isInAir = false;

    public bool forceChangeColor = false;

    void Start()
    {
        _zoneDetector = GetComponentInChildren<ZoneDetector>();
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D.gravityScale = 0.0f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), !isBlack);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), isBlack);
    }

    // Update is called once per frame
    void Update()
    {
        newVelocity = rigidbody2D.velocity;
        if (canChangeColor)
        {
            if (Input.GetButtonDown("SwapColors") && !wantToChangeColor || forceChangeColor)
            {
                forceChangeColor = false;
                wantToChangeColor = true;
                isBlack = !isBlack;
                transform.position = transform.position;
                if (isBlack)
                {
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"),
                        isBlack);
                }
                else
                {
                    Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"),
                        !isBlack);
                }
            }


        }

        if (wantToChangeColor &&
            ((_zoneDetector.isInBlackZone() && !isBlack) || (_zoneDetector.isInWhiteZone() && isBlack)))
        {
            wantToChangeColor = false;
            gravityScale = -gravityScale;
            jumpSpeed = -jumpSpeed;
            if (isBlack)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                sprite.color = Color.black;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), !isBlack);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                sprite.color = Color.white;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), isBlack);
            }
        }


        if (isInAir)
        {
            _floor = null;
        }

        if (_floor != null && _floorPrevPos != _floor.transform.position)
        {
            transform.position += _floor.transform.position - _floorPrevPos;
        }

        isInAir = true;
        int layerMask = LayerMask.NameToLayer("Player");
        RaycastHit2D[] hits;
        if (isBlack)
        {
            hits = Physics2D.RaycastAll(transform.position, -Vector2.up, 0.5f);
        }
        else
        {
            hits = Physics2D.RaycastAll(transform.position, Vector2.up, 0.5f);
        }
        foreach (RaycastHit2D hit in hits)
        {
            if (isBlack && hit.collider.gameObject.layer == LayerMask.NameToLayer("Black")
                || !isBlack && hit.collider.gameObject.layer == LayerMask.NameToLayer("White"))
            {
                isInAir = false;
                _floor = hit.collider.gameObject;
                _floorPrevPos = _floor.transform.position;

                if (isBlack && Vector2.Dot(hit.normal, Vector2.up) < 0.891
                    || !isBlack && Vector2.Dot(hit.normal, -Vector2.up) < 0.891)
                {
                    return;
                }
            }
        }

        float smoothedMovementFactor = !isInAir ? runDamping : airDamping;

        newVelocity.x = Mathf.Lerp(newVelocity.x, Input.GetAxis("Horizontal") * horizontalSpeed, Time.deltaTime * smoothedMovementFactor);
        rigidbody2D.velocity = newVelocity;

        if (Input.GetButtonDown("Jump") && !isInAir)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.AddForce(Vector3.down * Physics2D.gravity.magnitude * gravityScale);
    }

    void OnCollisionEnter2D(Collision2D objCollision2D)
    {
        if (objCollision2D.gameObject.layer == LayerMask.NameToLayer("Black")
            || objCollision2D.gameObject.layer == LayerMask.NameToLayer("White"))
        {
        }
    }

    void Jump()
    {
        rigidbody2D.AddForce(Vector2.up * jumpSpeed);
    }

}
