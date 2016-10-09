using UnityEngine;
using System.Collections;

public class BulletMove : MonoBehaviour
{
    private float k_Direction;
    public float Direction
    {
        get { return k_Direction; }
        set { k_Direction = value; }
    }
    
    void Update()
    {
        //transform.Translate(new Vector3(k_Direction * 15 * Time.deltaTime, 0, 0));
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
