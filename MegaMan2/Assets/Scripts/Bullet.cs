using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    private int k_Damage;
    public int Damage
    {
        get { return k_Damage; }
        set { k_Damage = value; }
    }
    private int k_Speed;
    public int Speed
    {
        get { return k_Speed; }
        set { k_Speed = value; }
    }

    private Controller2D k_Controller;
    private Renderer k_Renderer;

    void Start()
    {
        k_Controller = GetComponent<Controller2D>();
        k_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        k_Controller.Move(new Vector3(1 * k_Speed * Time.deltaTime, 0, 0));

        if (k_Renderer.isVisible == false)
        {
            Destroy(gameObject);
        }

        if (k_Controller.collisions.left || k_Controller.collisions.right)
        {
            k_Controller.HitObj.GetComponent<Health>().CurrentHealth -= k_Damage;
            Destroy(gameObject);
        }
    }
}