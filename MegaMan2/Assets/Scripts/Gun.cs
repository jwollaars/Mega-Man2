using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject k_BulletPrefab;

    private int k_BulletDamage = 1;
    public int GetBulletDamage
    {
        get { return k_BulletDamage; }
    }
    private int k_BulletSpeed = 14;

    public void Shoot()
    {
        GameObject bullet = Instantiate(k_BulletPrefab, transform.position, Quaternion.identity) as GameObject;
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.Damage = k_BulletDamage;

        if (transform.localScale.x > 0)
        {
            bulletScript.Speed = k_BulletSpeed;
        }
        else
        {
            bulletScript.Speed = -k_BulletSpeed;
        }
    }
}