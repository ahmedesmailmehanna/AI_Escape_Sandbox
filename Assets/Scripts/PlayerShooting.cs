using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject rocketPrefab; // Assign Rocket Prefab in Inspector
    public Transform firePoint; // Where the rocket spawns
    public float fireRate = 0.5f; // Time between shots
    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (rocketPrefab == null || firePoint == null)
        {
            Debug.LogError("🚨 Rocket Prefab or FirePoint is missing! Check Inspector.");
            return;
        }

        Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
        Debug.Log("🚀 Rocket Fired!");
    }
}
