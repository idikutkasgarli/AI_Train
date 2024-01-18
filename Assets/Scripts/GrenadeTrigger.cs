using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeTrigger : MonoBehaviour
{
    public float explosionRadius = 10;
    public float explosionForce = 100;
    public ParticleSystem explosionEffect; // Patlama efekti

    private bool isThrown = false;

    void OnCollisionEnter(Collision collision)
    {
        if (!isThrown) return;

        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("ground") || collision.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    public void SetThrown()
    {
        isThrown = true;
    }

    void Explode()
    {
        // Patlama efekti, ses, ya da başka özellikler ekleyebilirsiniz.

        // Patlama efektini başlat
        if (explosionEffect != null)
        {
            ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        // Yakınındaki her şeyi geriye itmek için etkileşim alanı yaratın
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hitCollider in colliders)
        {
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Bombadan uzaklığa göre bir kuvvet hesapla ve uygula
                Vector3 direction = (rb.transform.position - transform.position).normalized;
                float distance = Vector3.Distance(rb.transform.position, transform.position);
                float force = Mathf.Clamp(explosionForce / distance, 0, explosionForce);

                rb.AddForce(direction * force, ForceMode.Impulse);
            }
        }

        // Patladığında bombayı yok et
        Destroy(gameObject);
    }
}
