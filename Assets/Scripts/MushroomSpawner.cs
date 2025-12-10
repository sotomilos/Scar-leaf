using UnityEngine;
using System.Collections; // Required for using Coroutines

public class MushroomSpawner : MonoBehaviour
{

    public GameObject mushroomPrefab;
    public LayerMask terrainLayer;
    
    public int maxMushroomsOnScreen = 50;
    public float spawnInterval = 5f;    
    
    public float minX = 0f;
    public float maxX = 40f;
    public float minZ = 0f;
    public float maxZ = 10f;
    
    private int currentMushroomCount = 0; 

    void Start()
    {
        if (mushroomPrefab == null)
        {
            Debug.LogError("Mushroom Prefab is not assigned! Stopping spawner.");
            return;
        }
        
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            currentMushroomCount = FindObjectsOfType<CollectableItem>().Length;

            if (currentMushroomCount < maxMushroomsOnScreen)
            {
                AttemptSpawnMushroom();
            }
        }
    }

    void AttemptSpawnMushroom()
    {
        float startY = 50f;
        
        Vector3 randomPos = new Vector3(
            Random.Range(minX, maxX), 
            startY,                     
            Random.Range(minZ, maxZ)  
        );


        RaycastHit hit;
        if (Physics.Raycast(randomPos, Vector3.down, out hit, startY + 5f, terrainLayer)) 
        {
            Vector3 spawnPosition = hit.point;
            spawnPosition.y += 0.05f; 

            Instantiate(mushroomPrefab, spawnPosition, Quaternion.identity);
            
            currentMushroomCount++;
        }
        else
        {
            Debug.LogWarning($"Could not find terrain at {randomPos}. Check LayerMask or map bounds.");
        }
    }
}