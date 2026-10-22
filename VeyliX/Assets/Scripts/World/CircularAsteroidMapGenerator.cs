using System;
using UnityEngine;

[DisallowMultipleComponent]
public class CircularAsteroidMapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private Transform player;
    [SerializeField, Min(1)] private int ringCount = 5;
    [SerializeField, Min(0f)] private float firstRingInnerRadius = 25f;
    [SerializeField, Min(1f)] private float ringWidth = 35f;
    [SerializeField, Min(0)] private int minAsteroidsPerRing = 8;
    [SerializeField, Min(0)] private int maxAsteroidsPerRing = 14;
    [SerializeField, Range(1, 9)] private int minAsteroidSize = 1;
    [SerializeField, Range(1, 9)] private int maxAsteroidSize = 9;
    [SerializeField] private int seed;
    private Transform generatedAsteroids;
    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        if (asteroidPrefab == null)
        {
            return;
        }

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        ClearGeneratedAsteroids();
        generatedAsteroids = new GameObject("Generated Asteroids").transform;
        generatedAsteroids.SetParent(transform, false);
        Vector3 mapCenter = player != null ? player.position : transform.position;
        var random = seed == 0 ? new System.Random() : new System.Random(seed);
        for (int ring = 0; ring < ringCount; ring++)
        {
            float innerRadius = firstRingInnerRadius + ringWidth * ring;
            float outerRadius = innerRadius + ringWidth;
            int count = NextInt(random, minAsteroidsPerRing, maxAsteroidsPerRing);
            GetRingSizeRange(ring, out int minSize, out int maxSize);
            for (int i = 0; i < count; i++)
            {
                double angle = random.NextDouble() * Math.PI * 2.0;
                double radius = Math.Sqrt(random.NextDouble() * (outerRadius * outerRadius - innerRadius * innerRadius) + innerRadius * innerRadius);
                Vector3 position = mapCenter + new Vector3((float)(Math.Cos(angle) * radius),0f,(float)(Math.Sin(angle) * radius));
                Quaternion rotation = Quaternion.Euler(0f, (float)(random.NextDouble() * 360.0), 0f);
                int size = NextInt(random, minSize, maxSize);
                GameObject asteroid = Instantiate(asteroidPrefab, position, rotation, generatedAsteroids);
                asteroid.transform.localScale = Vector3.one * size;
            }
        }
    }
    private void GetRingSizeRange(int ring, out int minSize, out int maxSize)
    {
        int sizeRange = maxAsteroidSize - minAsteroidSize + 1;
        minSize = minAsteroidSize + ring * sizeRange / ringCount;
        maxSize = minAsteroidSize + (ring + 1) * sizeRange / ringCount - 1;
        maxSize = Mathf.Max(minSize, maxSize);
    }
    private static int NextInt(System.Random random, int minimum, int maximum)
    {
        return random.Next(minimum, maximum + 1);
    }
    private void ClearGeneratedAsteroids()
    {
        if (generatedAsteroids != null)
        {
            Destroy(generatedAsteroids.gameObject);
        }
    }
    private void OnValidate()
    {
        minAsteroidSize = Mathf.Clamp(minAsteroidSize, 1, 9);
        maxAsteroidSize = Mathf.Clamp(maxAsteroidSize, minAsteroidSize, 9);
        ringCount = Mathf.Clamp(ringCount, 1, maxAsteroidSize - minAsteroidSize + 1);
        ringWidth = Mathf.Max(1f, ringWidth);
        minAsteroidsPerRing = Mathf.Max(0, minAsteroidsPerRing);
        maxAsteroidsPerRing = Mathf.Max(minAsteroidsPerRing, maxAsteroidsPerRing);
    }
}
