using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float k_JumpHeight = 4;
    [SerializeField]
    private float k_TimeToJumpApex = 0.4f;
    private float k_JumpAcceleration;
    private float k_AccelerationTimeAirborn = 0.2f;
    private float k_AccelerationTimeGrounded = 0.1f;
    private float k_MoveSpeed = 6;

    private float k_Gravity;
    public float GetGravity
    {
        get { return k_Gravity; }
    }
    private float k_JumpVelocity;
    private Vector3 k_Velocity;
    private float k_VelocityXSmoothing;

    [SerializeField]
    private AudioClip k_Shoot;
    [SerializeField]
    private AudioClip k_Damaged;
    private AudioSource k_AudioSource;

    public GameObject HitObject;
    private bool k_CanLadder = false;
    public bool CanLadder
    {
        get { return k_CanLadder; }
        set { k_CanLadder = value; }
    }

    private float k_TimeDamaged = 1;
    private float k_FlashSpeed = 0.1f;
    private float k_DamageMoveSpeed = 5;
    private bool k_IsDamaged = false;
    public bool IsDamaged
    {
        get { return k_IsDamaged; }
    }

    private int k_BulletsToShoot;
    private float k_CoolDown = 0.3f;
    private bool k_Shooting = false;
    private float k_Timer = 0.5f;

    private Animator k_Animator;
    private SpriteRenderer k_SpriteRenderer;
    private Controller2D k_Controller;
    private Gun k_Gun;

    [SerializeField]
    private GameObject m_ReplayObj;
    private ReplaySystem m_ReplaySystem;
    private InputDirector m_InputDirector;

    void Start()
    {
        k_Animator = GetComponent<Animator>();
        k_SpriteRenderer = GetComponent<SpriteRenderer>();
        k_Controller = GetComponent<Controller2D>();
        k_AudioSource = GetComponent<AudioSource>();
        k_Gun = GetComponent<Gun>();
        m_ReplaySystem = m_ReplayObj.GetComponent<ReplaySystem>();
        m_InputDirector = GetComponent<InputDirector>();

        k_Gravity = -(2 * k_JumpHeight) / Mathf.Pow(k_TimeToJumpApex, 2);
        k_JumpVelocity = Mathf.Abs(k_Gravity) * k_TimeToJumpApex;
    }

    void Update()
    {
        if (k_Controller.collisions.above || k_Controller.collisions.below)
        {
            k_Velocity.y = 0;
        }

        Vector2 input = new Vector2(m_InputDirector.GetAxis(m_InputDirector.m_InputDir[0], m_InputDirector.m_InputDir[1]), 0);

        k_Animator.SetFloat("k_Move", input.x);

        if (k_CanLadder && m_InputDirector.m_InputDir[2])
        {
            k_Animator.SetBool("k_Climb", true);
            transform.Translate(0, 1 * -k_MoveSpeed * Time.deltaTime, 0);
            k_Animator.speed = 1;
        }
        else if (k_CanLadder && m_InputDirector.m_InputDir[3])
        {
            k_Animator.SetBool("k_Climb", true);
            transform.Translate(0, 1 * k_MoveSpeed * Time.deltaTime, 0);
            k_Animator.speed = 1;
        }

        if (k_CanLadder)
        {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                k_Animator.speed = 0;
            }
        }

        if (k_CanLadder == false)
        {
            k_Animator.SetBool("k_Climb", false);
            k_Animator.speed = 1;
        }

        if (input.x >= 0.1f)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1);
            //Debug.Log(k_Controller.collisions.below);
        }

        if (input.x <= -0.1f)
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 1);

        }

        if (m_InputDirector.m_InputDir[4] && k_Controller.collisions.below)
        {
            k_Velocity.y = k_JumpVelocity;// *jumpAcceleration;
            k_Animator.SetBool("k_IsJumping", true);
        }
        else if (k_Controller.collisions.below)
        {
            k_Animator.SetBool("k_IsJumping", false);
            //jumpAcceleration = 0;
        }

        if (m_InputDirector.m_InputDir[5])
        {
            k_BulletsToShoot = 1;
            k_Animator.SetBool("k_Shoot", true);
            k_Shooting = true;
        }

        if (m_InputDirector.m_InputDir[6])
        {
            k_BulletsToShoot = 3;
            k_Animator.SetBool("k_Shoot", true);
            k_Shooting = true;
        }

        if (k_Shooting == true)
        {
            k_Timer -= Time.deltaTime;

            if (k_Timer <= 0)
            {
                k_Animator.SetBool("k_Shoot", false);
                k_Timer = 0.3f;
                k_Shooting = false;
            }
        }

        if (k_BulletsToShoot > 0)
        {
            Shoot();
        }

        float targetVelocityX = input.x * k_MoveSpeed;

        if (k_IsDamaged)
        {
            k_TimeDamaged -= Time.deltaTime;

            if (k_DamageMoveSpeed >= 0)
            {
                k_DamageMoveSpeed -= 0.3f;
            }

            k_FlashSpeed -= Time.deltaTime;

            if (k_FlashSpeed <= 0)
            {
                if (k_SpriteRenderer.color.a == 1)
                {
                    k_SpriteRenderer.color = new Vector4(k_SpriteRenderer.color.r, k_SpriteRenderer.color.g, k_SpriteRenderer.color.b, 0);
                }
                else if (k_SpriteRenderer.color.a == 0)
                {
                    k_SpriteRenderer.color = new Vector4(k_SpriteRenderer.color.r, k_SpriteRenderer.color.g, k_SpriteRenderer.color.b, 1);
                }
                k_FlashSpeed = 0.08f;
            }

            k_Controller.Move(new Vector3(1 * -k_DamageMoveSpeed * Time.deltaTime, 0, 0));

            if (k_TimeDamaged <= 0)
            {
                k_DamageMoveSpeed = 5;
                k_IsDamaged = false;
                k_SpriteRenderer.color = new Vector4(k_SpriteRenderer.color.r, k_SpriteRenderer.color.g, k_SpriteRenderer.color.b, 1);
                k_Animator.SetBool("k_Damage", false);
                k_TimeDamaged = 1;
            }
        }

        k_Velocity.x = Mathf.SmoothDamp(k_Velocity.x, targetVelocityX, ref k_VelocityXSmoothing, 0.1f);
        k_Velocity.y += k_Gravity * Time.deltaTime;
        k_Controller.Move(k_Velocity * Time.deltaTime);
    }

    public void Damage()
    {
        k_IsDamaged = true;
        k_Animator.SetBool("k_Damage", true);
        k_AudioSource.PlayOneShot(k_Damaged);
    }

    private void Shoot()
    {
        k_CoolDown -= Time.deltaTime;

        if (k_CoolDown <= 0)
        {
            k_AudioSource.PlayOneShot(k_Shoot);
            k_CoolDown = 0.1f;
            k_Gun.Shoot();
            k_BulletsToShoot -= 1;
        }
    }

    
}
