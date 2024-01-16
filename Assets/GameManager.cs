using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Parent;  
    public GameObject objToSpawn; // Spawnlamak istediğiniz GameObject
    public Vector3 spawnAreaCenter; // Spawn bölgesinin merkezi
    public Vector3 spawnAreaSize; // Spawn bölgesinin genişliği ve yüksekliği
    public float SpawnTimer;
    public float SpawnY;
    public Transform Target;

    private void Start()
    {
        // Belirli aralıklarla SpawnObject metodunu çağırmak için InvokeRepeating kullanılır.
        InvokeRepeating("SpawnObject", 0f, SpawnTimer);
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
            walkerAgent.target = Target; // YourNewTargetTransform, yeni hedefinizi içeren bir Transform olmalı
            walkerAgent.directionInd.targetToLookAt = Target;
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Spawn bölgesinde rastgele bir konum belirleme
        float randomX = UnityEngine.Random.Range(spawnAreaCenter.x - spawnAreaSize.x / 2, spawnAreaCenter.x + spawnAreaSize.x / 2);
        float randomZ = UnityEngine.Random.Range(spawnAreaCenter.z - spawnAreaSize.z / 2, spawnAreaCenter.z + spawnAreaSize.z / 2);

        // Rastgele konumu Vector3 olarak oluşturup döndürme
        return new Vector3(randomX, SpawnY, randomZ);
    }
}
