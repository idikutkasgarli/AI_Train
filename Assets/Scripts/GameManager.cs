using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class GameManager : MonoBehaviour
{
    public GameObject Parent;
    public GameObject objToSpawn; // Spawnlamak istediğiniz GameObject
    public Vector3 spawnAreaCenter; // Spawn bölgesinin merkezi
    public Vector3 spawnAreaSize; // Spawn bölgesinin genişliği ve yüksekliği
    public float EnemySpawnTimer;
    public float SpawnY;
    public GameObject Target;
    private int enemyCount = 0;

    public GameObject pauseMenu;
    private bool isGamePaused = false;
    public GameObject InGameUI;

    public GameObject GrenadePrefab;
    public float GrenadeSpawnTimer;

    private void Start()
    {
        // Belirli aralıklarla SpawnObject metodunu çağırmak için InvokeRepeating kullanılır.
        InvokeRepeating("SpawnObject", 0f, EnemySpawnTimer);
        InvokeRepeating("SpawnGrenade", 0f, GrenadeSpawnTimer);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void SpawnGrenade()
    {
        // Rastgele bir konum belirleme
        Vector3 grenadeSpawnPosition = GetRandomSpawnPosition();

        // Belirtilen konumda grenade objesini spawnla
        Instantiate(GrenadePrefab, grenadeSpawnPosition, Quaternion.identity, Parent.transform);
    }

    private void SpawnObject()
    {
        // Rastgele bir konum belirleme
        Vector3 spawnPosition = GetRandomSpawnPosition();

        // Belirtilen konumda objeyi spawnla
        GameObject spawnedObj = Instantiate(objToSpawn, spawnPosition, Quaternion.identity, Parent.transform);

        WalkerAgent walkerAgent = spawnedObj.GetComponent<WalkerAgent>();
        if (walkerAgent != null)
        {
            // WalkerAgent içindeki target'ı değiştir
            walkerAgent.target = Target.transform; // YourNewTargetTransform, yeni hedefinizi içeren bir Transform olmalı
            walkerAgent.directionInd.targetToLookAt = Target.transform;
        }

        enemyCount++;
        UnityEngine.Debug.Log("Kalan Enemy: !" + enemyCount);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Spawn bölgesinde rastgele bir konum belirleme
        float randomX = UnityEngine.Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float randomZ = UnityEngine.Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);

        // Rastgele konumu Vector3 olarak oluşturup döndürme
        return new Vector3(randomX, SpawnY, randomZ);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InGameUI.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        InGameUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu"); // "Menu" yerine kendi ana menü sahnenizin adını kullanın
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void EnemyDestroyed()
    {
        // Düşman yok olduğunda düşman sayısını azalt
        enemyCount--;
        UnityEngine.Debug.Log("Kalan Enemy: !" + enemyCount);

        // Eğer bütün düşmanlar yok olduysa, oyunu kazanmış olabilirsiniz
        if (enemyCount <= 0)
        {
            GameOver(true);
            // Burada başka bir şeyler yapabilirsiniz, örneğin bir sonraki seviyeye geçiş veya oyunu tamamen bitirme.           
        }
    }

    public void GameOver(bool bWon)
    {
        if(bWon)
        {
            UnityEngine.Debug.Log("Oyunu Kazandınız!");
        }
        else
        {
            UnityEngine.Debug.Log("Oyunu Kaybettin!!");
        }
        Time.timeScale = 0f;
    }

}
