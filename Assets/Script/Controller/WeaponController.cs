using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("===== ×Óµ¯²ÎÊý =====")]
    public GameObject bulletObject;
    public float bulletSpeed = 30.0f;
    public float fireInterval = 0.3f;
    private float fireCD = 0;

    public bool isAI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAI)
        {
            Fire();
        }
    }

    public void Fire()
    {
        if (fireCD > Time.time)
        {
            return;
        }
        var bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
        var bulletRigid = bullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = transform.forward * bulletSpeed;
        fireCD = Time.time + fireInterval;
    }
}
