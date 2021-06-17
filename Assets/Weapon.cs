using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float coolDown = 1.5f;
    void FixedUpdate()
    {
        coolDown -= Time.fixedDeltaTime;
        if (coolDown > 0) return;
        
        Shoot();
        coolDown = 1.5f;
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}
