using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject rocketPrefab; // Assign Rocket Prefab in Inspector
    public Transform firePoint; // Where the rocket spawns
    public float reloadTime = 1.5f; // Time between shots
    private bool isReloading = false;
    void Start()
    {
        // currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            Shoot();
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

        // Start reloading
        StartCoroutine(Reload());
    }
    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("🔄 Reloading...");
        // StartCoroutine(recoil.ReloadAnimation()); // Trigger weapon animation

        ReloadUI reloadUI = FindFirstObjectByType<ReloadUI>();
        if (reloadUI != null)
        {
            reloadUI.StartReloadUI(reloadTime);
        }


        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        Debug.Log("✅ Reload Complete!");
    }
}
