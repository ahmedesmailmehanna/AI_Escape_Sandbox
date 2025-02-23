using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 20f; // Rocket speed
    public float explosionRadius = 5f; // Explosion size
    public float explosionForce = 700f; // Force applied to nearby objects
    public GameObject explosionEffect; // Explosion effect prefab

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("🚨 Rigidbody is missing on the Rocket Prefab!");
            return;
        }

        rb.isKinematic = false; // ✅ Enable physics
        rb.linearVelocity = Vector3.zero; // ✅ Reset velocity
        rb.AddForce(transform.forward * speed, ForceMode.Impulse); // ✅ Fire forward

        Debug.Log("🚀 Rocket Launched!");
    }

    void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    void Explode()
    {
        Debug.Log("💥 Rocket Exploded!");

        // ✅ Spawn explosion effect
        if (explosionEffect != null) 
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // Destroy explosion after 2 seconds
        }
        // ✅ Apply explosion force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearby in colliders)
        {
            Rigidbody rb = nearby.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }

        Destroy(gameObject); // ✅ Destroy rocket after explosion
    }
}
