using System.Collections;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    [Header("Rain Settings")]
    public GameObject rainPrefab; // The prefab to spawn
    public Vector3 spawnArea = new Vector3(10, 1, 10); // Size of the spawn area
    public float spawnRate = 0.1f; // Time interval between spawns

    [Header("Spawn Position Offset")]
    public float spawnHeight = 10f; // Height from which the raindrops fall

    private void Start()
    {
        StartCoroutine(SpawnRain());
    }

    private IEnumerator SpawnRain()
    {
        while (true)
        {
            SpawnRaindrop();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnRaindrop()
    {
        if (rainPrefab == null)
        {
            Debug.LogWarning("Rain Prefab is not assigned!");
            return;
        }

        // Randomize the spawn position within the area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            spawnHeight,
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );

        // Adjust spawn position relative to the spawner's position
        randomPosition += transform.position;

        // Spawn the prefab
        Instantiate(rainPrefab, randomPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw the spawn area in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0, spawnHeight / 2, 0),
            new Vector3(spawnArea.x, spawnHeight, spawnArea.z)
        );
    }
}
