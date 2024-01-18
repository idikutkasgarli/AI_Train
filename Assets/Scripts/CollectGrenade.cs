using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGrenade : MonoBehaviour
{
    public GrenadeLauncher grenadeLauncher;

    void OnCollisionEnter(Collision collision)
    {
        // Eğer çarpışan obje "grenade" tag'ine sahipse
        if (collision.gameObject.CompareTag("Grenade"))
        {
            if(grenadeLauncher.availableGrenades < grenadeLauncher.maxGrenades)
            {
                grenadeLauncher.availableGrenades++;
                grenadeLauncher.UpdateGrenadeSlider();
                Destroy(collision.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Eğer çarpışan obje "grenade" tag'ine sahipse
        if (other.CompareTag("Grenade"))
        {
            // Çarpışan objeyi yok et
            Destroy(other.gameObject);
            grenadeLauncher.availableGrenades++;
            grenadeLauncher.UpdateGrenadeSlider();
        }
    }
}
