using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ReachTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Temas eden objenin tag'ini kontrol et
        if (other.CompareTag("target"))
        {
            StartCoroutine(DestroyAfterDelay(.5f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        // Belirli bir süre bekleyin
        yield return new WaitForSeconds(delay);

        // Belirtilen süre sonrasında objeyi yok et
        Destroy(gameObject.transform.parent.parent.gameObject);
    }
}
