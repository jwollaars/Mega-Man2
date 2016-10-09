using UnityEngine;
using System.Collections;

public interface Enemy
{
    void Start();
    void Update();
}

public class EnemyBehaviour : MonoBehaviour
{
    private GameObject k_Target;
    public GameObject GetTarget
    {
        get { return k_Target; }
    }

    private float k_Distance;
    public float GetDistance
    {
        get { return k_Distance; }
    }

    private float k_XDistance;
    public float GetXDistance
    {
        get { return k_XDistance; }
    }
    private float k_YDistance;
    public float GetYDistance
    {
        get { return k_YDistance; }
    }

    public bool k_HitPlayer = false;

    private bool k_StartBehaviour = false;
    public bool StartBehaviour
    {
        get { return k_StartBehaviour; }
        set { k_StartBehaviour = value; }
    }
    private bool k_UsedStart = false;

    public Vector3 k_Velocity;
    public Vector3 Velocity
    {
        get { return k_Velocity; }
        set { k_Velocity = value; }
    }
    [SerializeField]
    private float k_JumpHeight = 4;
    public float GetJumpHeight
    {
        get { return k_JumpHeight; }
    }
    [SerializeField]
    private float k_TimeToJumpApex = 0.4f;
    public float GetTimeToJumpApex
    {
        get { return k_TimeToJumpApex; }
    }
    private float k_JumpVelocity;
    public float JumpVelocity
    {
        get { return k_JumpVelocity; }
        set { k_JumpVelocity = value; }
    }
    private float k_Gravity;
    public float Gravity
    {
        get { return k_Gravity; }
        set { k_Gravity = value; }
    }

    [SerializeField]
    public int k_Damage;

    private Animator k_Animator;
    public Animator GetAnimator
    {
        get { return k_Animator; }
    }

    [SerializeField]
    private string k_EnemyType;

    public Controller2D k_Controller;

    private Enemy k_CurrentState;
    public Enemy CurrentState
    {
        get { return k_CurrentState; }
        set { k_CurrentState = value; }
    }

    private Enemy k_BattonState;
    private Enemy k_FrienderState;
    private Enemy k_MonkingState;
    private Enemy k_KukkuState;
    private Enemy k_RobbitState;

    void Start()
    {
        k_Target = GameObject.FindGameObjectWithTag("Player");
        k_Animator = gameObject.GetComponent<Animator>();
        k_Controller = GetComponent<Controller2D>();

        k_Gravity = -(2 * k_JumpHeight) / Mathf.Pow(k_TimeToJumpApex, 2);
        k_JumpVelocity = Mathf.Abs(k_Gravity) * k_TimeToJumpApex;

        //Initiate Enemy Type
        switch (k_EnemyType)
        {
            case "Robbit":
                k_RobbitState = new RobbitEnemy(this);
                k_CurrentState = k_RobbitState;
                break;
            case "Friender":
                k_FrienderState = new FrienderEnemy(this);
                k_CurrentState = k_FrienderState;
                break;
            case "Kukku":
                k_KukkuState = new KukkuEnemy(this);
                k_CurrentState = k_KukkuState;
                break;
            case "Monking":
                k_MonkingState = new MonkingEnemy(this);
                k_CurrentState = k_MonkingState;
                break;
            case "Batton":
                k_BattonState = new BattonEnemy(this);
                k_CurrentState = k_BattonState;
                break;
            default:
                Debug.Log(name + ": What the actual f*ck am I? (" + gameObject + ")");
                break;
        }
    }

    void Update()
    {
        if (k_Target != null)
        {
            DistanceBetween(transform.position, k_Target.transform.position);
        }

        if (k_UsedStart == false && k_CurrentState != null)
        {
            k_CurrentState.Start();
            k_UsedStart = true;
        }

        if (k_CurrentState != null)
        {
            if (k_Target != null)
            {
                k_CurrentState.Update();
            }
        }

        //if (k_Controller.HitObj != null)
        //{
        //    if (k_Controller.HitObj.name == "Mega_Man")
        //    {
        //        if (!k_Controller.HitObj.GetComponent<Player>().IsDamaged)
        //        {
        //            if (k_Controller.collisions.left || k_Controller.collisions.right || k_Controller.collisions.below || k_Controller.collisions.above || k_HitPlayer)
        //            {
        //                k_Controller.HitObj.GetComponent<Health>().CurrentHealth -= k_Damage;
        //                k_Controller.HitObj.GetComponent<Player>().Damage();
        //                k_HitPlayer = false;
        //            }
        //        }
        //    }
        //}
    }

    private void DistanceBetween(Vector2 _Self, Vector2 _Other)
    {
        k_Distance = Vector3.Distance(_Self, _Other);
        k_XDistance = Vector3.Distance(new Vector2(_Self.x, 0), new Vector2(_Other.x, 0));
        k_YDistance = Vector3.Distance(new Vector2(0, _Self.y), new Vector2(0, _Other.y));
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}

public class BattonEnemy : Enemy
{
    private EnemyBehaviour _EnemyBehaviour;

    private bool k_Activated = false;
    private float k_StartDistance = 8;
    private float k_MoveSpeed = 1f;

    public BattonEnemy(EnemyBehaviour enemyBehaviour)
    {
        _EnemyBehaviour = enemyBehaviour;
    }

    public void Start()
    {
    }

    public void Update()
    {
        StartBehaviour();
        if (k_Activated)
        {
            MoveBehaviour();
        }
    }

    private void StartBehaviour()
    {
        if (_EnemyBehaviour.GetDistance <= k_StartDistance)
        {
            k_Activated = true;
        }
    }

    private void MoveBehaviour()
    {
        _EnemyBehaviour.GetAnimator.SetInteger("State", 1);

        if (_EnemyBehaviour.GetXDistance >= _EnemyBehaviour.GetYDistance)
        {
            if (_EnemyBehaviour.transform.position.x > _EnemyBehaviour.GetTarget.transform.position.x)
            {
                _EnemyBehaviour.k_Controller.Move(new Vector3(1 * -k_MoveSpeed * Time.deltaTime, 0, 0));
            }
            else
            {
                _EnemyBehaviour.k_Controller.Move(new Vector3(1 * k_MoveSpeed * Time.deltaTime, 0, 0));
            }
        }
        else
        {
            if (_EnemyBehaviour.transform.position.y > _EnemyBehaviour.GetTarget.transform.position.y)
            {
                _EnemyBehaviour.k_Controller.Move(new Vector3(0, 1 * -k_MoveSpeed * Time.deltaTime, 0));
            }
            else
            {
                _EnemyBehaviour.k_Controller.Move(new Vector3(0, 1 * k_MoveSpeed * Time.deltaTime, 0));
            }
        }
    }
}

public class FrienderEnemy : Enemy
{
    private EnemyBehaviour _EnemyBehaviour;

    private float k_StartDistance = 5;

    public FrienderEnemy(EnemyBehaviour enemyBehaviour)
    {
        _EnemyBehaviour = enemyBehaviour;
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    void MoveBehaviour()
    {
    }
}

public class MonkingEnemy : Enemy
{
    private EnemyBehaviour _EnemyBehaviour;

    private float k_StartDistance = 5;

    public MonkingEnemy(EnemyBehaviour enemyBehaviour)
    {
        _EnemyBehaviour = enemyBehaviour;
    }

    public void Start()
    {
    }

    public void Update()
    {
    }

    void MoveBehaviour()
    {
    }
}

public class KukkuEnemy : Enemy
{
    private EnemyBehaviour _EnemyBehaviour;

    private bool k_Activated = false;
    private float k_StartDistance = 8;
    private float k_MoveSpeed = 5f;

    private float k_Timer = 2f;

    public KukkuEnemy(EnemyBehaviour enemyBehaviour)
    {
        _EnemyBehaviour = enemyBehaviour;
    }

    public void Start()
    {
    }

    public void Update()
    {
        if (k_Timer != 0)
        {
            k_Timer -= Time.deltaTime;
        }

        if (k_Timer <= 0)
        {
            k_Timer = 0;
        }

        //if (_EnemyBehaviour.k_Controller.collisions.above || _EnemyBehaviour.k_Controller.collisions.below)
        //{
        //    _EnemyBehaviour.k_Velocity.y = 0;
        //}

        StartBehaviour();
        if (k_Activated)
        {
            MoveBehaviour();
        }

        _EnemyBehaviour.k_Velocity.y += _EnemyBehaviour.Gravity * Time.deltaTime;
        _EnemyBehaviour.k_Controller.Move(_EnemyBehaviour.k_Velocity * Time.deltaTime);
    }

    void MoveBehaviour()
    {
        if (k_Timer <= 0)
        {
            _EnemyBehaviour.k_Velocity.y = _EnemyBehaviour.JumpVelocity;
            k_Timer = 2;
        }
        _EnemyBehaviour.k_Controller.Move(new Vector3(1 * -k_MoveSpeed * Time.deltaTime, 0, 0));
    }

    private void StartBehaviour()
    {
        if (_EnemyBehaviour.GetDistance <= k_StartDistance)
        {
            k_Activated = true;
        }
    }
}

public class RobbitEnemy : Enemy
{
    private EnemyBehaviour _EnemyBehaviour;

    private bool k_Activated = false;
    private float k_StartDistance = 8;
    private float k_MoveSpeed = 5f;

    private float k_Timer = 2f;

    public RobbitEnemy(EnemyBehaviour enemyBehaviour)
    {
        _EnemyBehaviour = enemyBehaviour;
    }

    public void Start()
    {
    }

    public void Update()
    {
        if (k_Timer != 0)
        {
            k_Timer -= Time.deltaTime;
        }

        if (k_Timer <= 0)
        {
            k_Timer = 0;
        }

        //if (_EnemyBehaviour.k_Controller.collisions.above || _EnemyBehaviour.k_Controller.collisions.below)
        //{
        //    _EnemyBehaviour.k_Velocity.y = 0;
        //}

        StartBehaviour();
        if (k_Activated)
        {
            MoveBehaviour();
        }

        _EnemyBehaviour.k_Velocity.y += _EnemyBehaviour.Gravity * Time.deltaTime;
        _EnemyBehaviour.k_Controller.Move(_EnemyBehaviour.k_Velocity * Time.deltaTime);
    }

    void MoveBehaviour()
    {
        if (k_Timer <= 0)
        {
            _EnemyBehaviour.k_Velocity.y = _EnemyBehaviour.JumpVelocity;
            k_Timer = 2;
        }

        if (!_EnemyBehaviour.k_Controller.collisions.below)
        {
            _EnemyBehaviour.k_Controller.Move(new Vector3(1 * -k_MoveSpeed * Time.deltaTime, 0, 0));
            //Debug.Log(_EnemyBehaviour.k_Controller.collisions.below);
        }
    }

    private void StartBehaviour()
    {
        if (_EnemyBehaviour.GetDistance <= k_StartDistance)
        {
            k_Activated = true;
        }
    }
}