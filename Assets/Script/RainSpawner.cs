using System.Collections;
using UnityEngine;

public class RainSpawner : MonoBehaviour
{
    [Header("Rain Settings")]
    public GameObject rainPrefab; // The prefab to spawn
    public Vector3 spawnArea = new Vector3(10, 1, 10); // Size of the spawn area
    public float initialSpawnRate = 1f; // Initial time interval between spawns
    public float finalSpawnRate = 0.1f; // Final time interval between spawns
    public float rainDuration = 10f; // Total duration of the rain (in seconds)
    public float rateIncreaseDuration = 5f; // Duration over which the drop rate increases

    [Header("Spawn Position Offset")]
    public float spawnHeight = 10f; // Height from which the raindrops fall

    private void Start()
    {
        StartCoroutine(RainController());
    }

    private IEnumerator RainController()
    {
        float elapsedTime = 0f;
        float currentSpawnRate = initialSpawnRate;

        while (elapsedTime < rainDuration)
        {
            // Adjust the spawn rate only during the rateIncreaseDuration
            if (elapsedTime < rateIncreaseDuration)
            {
                currentSpawnRate = Mathf.Lerp(initialSpawnRate, finalSpawnRate, elapsedTime / rateIncreaseDuration);
            }

            // Spawn a raindrop and wait for the current spawn rate
            SpawnRaindrop();
            yield return new WaitForSeconds(currentSpawnRate);

            elapsedTime += currentSpawnRate;
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
