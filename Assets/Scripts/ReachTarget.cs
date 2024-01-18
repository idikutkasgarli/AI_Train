using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ReachTarget : MonoBehaviour
{
    public string TriggerTag;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Temas eden objenin tag'ini kontrol et
        if (other.CompareTag(TriggerTag))
        {
            UnityEngine.Debug.Log("Trigger with " + other.gameObject.name);
            if (gameManager != null)
            {
                gameManager.EnemyDestroyed();
            }
            StartCoroutine(DestroyAfterDelay(.1f));
        }
        else if (other.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("Trigger with " + other.gameObject.name);
            if (gameManager != null)
            {
                gameManager.GameOver(false);
            }
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
