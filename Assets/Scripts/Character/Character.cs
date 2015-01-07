using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{

    private enum State
    {
        Falling, Running, Transition
    };

    private enum ColorState
    {
        White, Black
    }

    private State currentState;
    private ColorState currentColor;

    public float gravityScale = 1.0f;

    public float runDamping = 20.0f;
    public float fallDamping = 5.0f;

    private Vector2 newVelocity;
    public float runSpeed = 10.0f;
    public float jumpSpeed = 10.0f;
    public float fallingControllSpeed = 10.0f;

    private ZoneDetector zoneDetector;
    private EdgeDectector edgeDectector;
    private SpriteRenderer sprite;

    private Transform floor;
    private Vector3 floorPrevPos;

    private RaycastHit2D groundHit;
    private Vector2 downDirection;

    public bool forceChangeColor = false;

    public bool IsWhite
    {
        get { return currentColor == ColorState.White; }
    }

    void Start()
    {
        zoneDetector = GetComponentInChildren<ZoneDetector>();
        edgeDectector = GetComponentInChildren<EdgeDectector>();
        sprite = GetComponent<SpriteRenderer>();

        currentColor = (zoneDetector.isInBlackZone()) ? ColorState.White : ColorState.Black;
        downDirection = currentColor == ColorState.Black ? -Vector2.up : Vector2.up;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), currentColor == ColorState.White);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), currentColor == ColorState.Black);

        rigidbody2D.gravityScale = 0;
    }

    void Update()
    {
        if (currentState == State.Transition &&
            (zoneDetector.isInBlackZone() && currentColor == ColorState.Black ||
            zoneDetector.isInWhiteZone() && currentColor == ColorState.White))
        {
            currentState = State.Falling;
            jumpSpeed = -jumpSpeed;
            if (currentColor == ColorState.White)
            {
                currentColor = ColorState.Black;
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                downDirection = -Vector2.up;
            }
            else
            {
                currentColor = ColorState.White;
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                downDirection = Vector2.up;
            }
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), currentColor == ColorState.White);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), currentColor == ColorState.Black);
        }

        if (currentState == State.Running && floor != null)
        {
            transform.position += floor.position - floorPrevPos;
        }

        if (currentState == State.Running || currentState == State.Falling)
        {
            int layerMask = currentColor == ColorState.Black ? 1 << LayerMask.NameToLayer("Black") : 1 << LayerMask.NameToLayer("White");
            groundHit = Physics2D.Raycast(transform.position, downDirection, 0.5f, layerMask);
            if (groundHit.collider != null)
            {
                ChangeState(State.Running);
            }
            else
            {
                ChangeState(State.Falling);
            }
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.AddForce(downDirection * Physics2D.gravity.magnitude * gravityScale);
    }

    public void Jump()
    {
        if (currentState == State.Running)
        {
            rigidbody2D.AddForce(-downDirection * jumpSpeed);
        }
    }

    public void Falling(float perc)
    {
        newVelocity = rigidbody2D.velocity;
        if (currentState == State.Running && Vector2.Dot(groundHit.normal, -downDirection) >= 0.891)
        {
            newVelocity.x = Mathf.Lerp(newVelocity.x, perc * fallingControllSpeed, Time.deltaTime * fallDamping);
        }
        rigidbody2D.velocity = newVelocity;
    }

    public void Running(float perc)
    {
        newVelocity = rigidbody2D.velocity;
        if (currentState == State.Running && Vector2.Dot(groundHit.normal, -downDirection) >= 0.891)
        {
            newVelocity.x = Mathf.Lerp(newVelocity.x, perc * runSpeed, Time.deltaTime * runDamping);
        }
        rigidbody2D.velocity = newVelocity;
    }

    public void SwapColors()
    {
        if ((edgeDectector.NearZoneEdge && currentState != State.Transition) || forceChangeColor)
        {
            forceChangeColor = false;
            ChangeState(State.Transition);
        }
    }


    private void ChangeState(State state)
    {
        if (currentState == State.Falling && state == State.Running)
        {
            floor = groundHit.collider.transform;
            floorPrevPos = floor.transform.position;
        }

        if (currentState == State.Running && state == State.Falling)
        {
            floor = null;
        }

        if (state == State.Transition)
        {
            if (currentColor == ColorState.White)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), true);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), true);
            }
            transform.position = transform.position;
        }

        currentState = state;
    }
}
