using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ReachTarget : MonoBehaviour
{
    public string TriggerTag;

    private void OnTriggerEnter(Collider other)
    {
        // Temas eden objenin tag'ini kontrol et
        if (other.CompareTag(TriggerTag))
        {
            UnityEngine.Debug.Log("Trigger with " + other.gameObject.name);
            StartCoroutine(DestroyAfterDelay(.1f));
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
