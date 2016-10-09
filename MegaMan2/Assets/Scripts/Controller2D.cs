using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;

    const float skinWidth = .015f;
    public int horizontalRayCount = 4;
    public int verticleRayCount = 4;

    private float horizontalRaySpacing;
    private float verticleRaySpacing;

    private GameObject k_HitObj;
    public GameObject HitObj
    {
        get { return k_HitObj; }
    }

    private EnemyBehaviour enemyBehaviour;
    private BoxCollider2D collider;
    private Player player;
    private RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        player = GetComponent<Player>();
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }

        if (velocity.y != 0)
        {
            VerticleCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLenght, Color.red);

            if (hit)
            {
                if (hit.collider.gameObject != gameObject && hit.collider.gameObject.tag != "Ladder")
                {
                    k_HitObj = hit.collider.gameObject;
                }

                if (hit.collider.gameObject.tag == "Door")
                {
                    Application.LoadLevel(1);
                }

                if (hit.collider.gameObject.tag == "Death")
                {
                    Application.LoadLevel(2);
                }

                if (hit.collider.gameObject.tag == "Player" && gameObject.name != "Mega_Man")
                {
                    enemyBehaviour.k_HitPlayer = true;
                }

                if (hit.collider.gameObject.tag != "Ladder")
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLenght = hit.distance;
                                      
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticleCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < verticleRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticleRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if (hit)
            {
                if (gameObject.name == "Mega_Man")
                {
                    if (hit.collider.gameObject.tag == "Ladder")
                    {
                        player.CanLadder = true;
                        player.HitObject = hit.collider.gameObject;
                    }
                    else
                    {
                        player.CanLadder = false;
                    }
                }

                if (hit.collider.gameObject.tag == "Door")
                {
                    Application.LoadLevel(1);
                }

                if (hit.collider.gameObject.tag == "Death")
                {
                    Application.LoadLevel(2);
                }

                if (hit.collider.gameObject != gameObject)
                {
                    k_HitObj = hit.collider.gameObject;
                }

                velocity.y = (hit.distance - skinWidth) * directionY;

                if (hit.collider.gameObject.tag == "Ladder")
                {
                    velocity.y = 0;
                }

                rayLenght = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticleRayCount = Mathf.Clamp(verticleRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticleRaySpacing = bounds.size.x / (verticleRayCount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public void Reset()
        {
            above = below = false;
            left = right = false;
        }
    }
}