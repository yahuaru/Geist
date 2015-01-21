using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{

    private enum State
    {
        Falling, Running, Transition, Death
    };

    private enum ColorState
    {
        White, Black, Both
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

    private SpriteRenderer sprite;

    private Transform floor;
    private Vector3 floorPrevPos;

    private RaycastHit2D groundHit;
    private Vector2 downDirection;

    [Range(0, 100)]
    public float maxTransitionJumpVelocity = 10.0f;

    [Range(0.3f, 10f)]
    public float raycastTransitionDetectionLength = 1.6f;

    private Animator animator;

    public Vector3 checkpointPos;

    private readonly Vector3 deltaColliderTransofrm = new Vector3(0.0f, 0.03f);

    public bool IsWhite
    {
        get { return currentColor == ColorState.White; }
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentColor = (CurrentZone() == ColorState.Black) ? ColorState.White : ColorState.Black;
        downDirection = currentColor == ColorState.Black ? -Vector2.up : Vector2.up;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), currentColor == ColorState.White);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), currentColor == ColorState.Black);

        rigidbody2D.gravityScale = 0;

        checkpointPos = transform.position;
    }

    void Update()
    {

        if (currentState == State.Transition)
        {
            ColorState zoneColor = CurrentZone();
            if ((currentColor == ColorState.White && zoneColor == ColorState.White)
                || (currentColor == ColorState.Black && zoneColor == ColorState.Black))
            {

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

                ChangeState(State.Falling);
            }
        }

        if (currentState == State.Running && floor != null)
        {
            Vector3 posDelta = floor.position - floorPrevPos;
            if (posDelta != Vector3.zero)
            {
                int layerMask = (currentColor == ColorState.Black) ? 1 << LayerMask.NameToLayer("Black")
                                                               : 1 << LayerMask.NameToLayer("White");
                Debug.DrawRay(transform.position - deltaColliderTransofrm,
                    Vector3.Normalize(posDelta) * (posDelta.magnitude + 0.25f), Color.red);
                RaycastHit2D wallHit = Physics2D.Raycast(transform.position - deltaColliderTransofrm,
                    Vector3.Normalize(posDelta), posDelta.magnitude + 0.25f, layerMask);
                if (wallHit.collider == null)
                {
                    transform.position += posDelta;
                }
            }
        }

        if (currentState == State.Running || currentState == State.Falling)
        {
            int layerMask = (currentColor == ColorState.Black) ? 1 << LayerMask.NameToLayer("Black")
                                                               : 1 << LayerMask.NameToLayer("White");
            Debug.DrawRay(transform.position, downDirection * 0.4f, Color.red);
            groundHit = Physics2D.Raycast(transform.position, downDirection, 0.4f, layerMask);
            if (groundHit.collider != null)
            {
                floor = groundHit.collider.transform;
                floorPrevPos = floor.transform.position;

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
        if (currentState == State.Falling || currentState == State.Running || currentState == State.Transition)
        {
            rigidbody2D.AddForce(downDirection * Physics2D.gravity.magnitude * gravityScale);
        }
    }

    public void Jump()
    {
        if (currentState == State.Running)
        {
            rigidbody2D.AddForce(-downDirection * jumpSpeed);
        }
    }

    public void HorizontalMovement(float perc)
    {
        newVelocity = rigidbody2D.velocity;
        switch (currentState)
        {
            case State.Running:
                if (Vector2.Dot(groundHit.normal, -downDirection) >= 0.891)
                {
                    newVelocity.x = Mathf.Lerp(newVelocity.x, perc * runSpeed, Time.deltaTime * runDamping);
                }
                break;

            case State.Falling:
                newVelocity.x = Mathf.Lerp(newVelocity.x, perc * fallingControllSpeed, Time.deltaTime * fallDamping);
                break;

            case State.Transition:
                newVelocity.x = Mathf.Lerp(newVelocity.x, perc * fallingControllSpeed, Time.deltaTime * fallDamping);
                break;
        }
        rigidbody2D.velocity = newVelocity;
    }

    public void SwapColors(bool forceChangeColor = false, float perc = 0.0f)
    {
        if (currentState == State.Falling || currentState == State.Running)
        {

            int layerMask = (currentColor == ColorState.Black) ? 1 << LayerMask.NameToLayer("Black")
                                                               : 1 << LayerMask.NameToLayer("White");
            var movementVelocity = Vector2.zero;
            movementVelocity.x = Mathf.Lerp(newVelocity.x, perc * fallingControllSpeed, Time.deltaTime * fallDamping);

            var nextFrameVelocityDirection =
                downDirection * gravityScale * Physics2D.gravity.magnitude * Time.fixedDeltaTime
                + rigidbody2D.velocity + movementVelocity;

            Debug.DrawRay(collider2D.bounds.center, nextFrameVelocityDirection,
                Color.magenta, 4.0f);
            var hitResult = Physics2D.Raycast(collider2D.bounds.center, nextFrameVelocityDirection.normalized,
                nextFrameVelocityDirection.magnitude, layerMask);

            if (hitResult.collider != null || forceChangeColor)
            {
                ChangeState(State.Transition);
            }
        }
    }

    public void Death()
    {
        if (currentState != State.Death)
        {
            rigidbody2D.velocity = Vector2.zero;
            ChangeState(State.Death);
        }
    }

    public void Respawn()
    {
        ChangeState(State.Falling);
        transform.position = checkpointPos;
        ColorState currentZone = CurrentZone();
        if (currentColor == currentZone || currentZone == ColorState.Both)
        {
            SwapColors(true);
        }
    }

    private void ChangeState(State state)
    {

        if (state == State.Falling)
        {
            if (currentState == State.Running)
            {
                floor = null;
            }
            else if (currentState == State.Transition)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Black"), LayerMask.NameToLayer("Player"), currentColor == ColorState.White);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("White"), LayerMask.NameToLayer("Player"), currentColor == ColorState.Black);

                //Limit height of the transition jump
                newVelocity = rigidbody2D.velocity;
                newVelocity.y = Mathf.Clamp(newVelocity.y, -maxTransitionJumpVelocity, maxTransitionJumpVelocity);
                rigidbody2D.velocity = newVelocity;
            }
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

        if (state == State.Death)
        {
            animator.SetTrigger("IsDead");
        }


        currentState = state;
    }

    ColorState CurrentZone()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Black") | 1 << LayerMask.NameToLayer("White");
        Collider2D[] collides2D = Physics2D.OverlapCircleAll(collider2D.bounds.center, 0.25f, layerMask);
        bool inBlackZone = false;
        bool inWhiteZone = false;
        foreach (var collider in collides2D)
        {
            inBlackZone = inBlackZone || collider.gameObject.layer == LayerMask.NameToLayer("Black");
            inWhiteZone = inWhiteZone || collider.gameObject.layer == LayerMask.NameToLayer("White");
        }

        if (inBlackZone && !inWhiteZone)
        {
            return ColorState.Black;
        }
        if (inWhiteZone && !inBlackZone)
        {
            return ColorState.White;
        }

        return ColorState.Both;
    }
}
