using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
[DisallowMultipleComponent]
public class CircularAlienRingSpawner : MonoBehaviour
{
    public event Action<int> OnRingUnlocked;

    private enum RingClearCondition
    {
        AllEnemiesDefeated,
        BossDefeated
    }

    [Serializable]
    private class AlienSpawnEntry
    {
        [Tooltip("Alien prefab eligible to spawn in the selected ring.")]
        public GameObject alienPrefab;
        [Tooltip("1-based ring number this alien can spawn in.")]
        [Min(1)] public int ring = 1;
        [Tooltip("Relative spawn chance within this ring. Positive weights for each ring should total 1.")]
        [Range(0f, 1f)] public float weight = 1f;
    }

    [Serializable]
    private class RingProgressionSettings
    {
        [Tooltip("Entry 0 configures ring 1, entry 1 configures ring 2, and so on. Missing entries default to clearing all enemies.")]
        public RingClearCondition clearCondition = RingClearCondition.AllEnemiesDefeated;
        [Tooltip("Required when the clear condition is Boss Defeated. The boss spawns once in this ring.")]
        public GameObject bossPrefab;
    }

    private const int GateSegmentCount = 64;

    [SerializeField] private List<AlienSpawnEntry> alienEntries = new List<AlienSpawnEntry>();
    [SerializeField] private List<RingProgressionSettings> ringProgression = new List<RingProgressionSettings>();
    [SerializeField] private Transform player;
    [SerializeField] private Transform mapCenter;
    [SerializeField, Min(1)] private int ringCount = 5;
    [SerializeField, Min(0f)] private float firstRingInnerRadius = 25f;
    [SerializeField, Min(1f)] private float ringWidth = 35f;
    [SerializeField, Min(1)] private int minAliensInFirstRing = 2;
    [SerializeField, Min(1)] private int maxAliensInFirstRing = 4;
    [SerializeField, Min(0)] private int additionalAliensPerRing = 1;
    [SerializeField, Min(0f)] private float activationDistance = 15f;
    [SerializeField, Min(0.1f)] private float gateThickness = 1f;
    [SerializeField, Min(1f)] private float gateHeight = 100f;
    [SerializeField] private bool respawnAfterDefeat;
    [SerializeField, Min(0f)] private float respawnDelay = 10f;
    [SerializeField] private int seed;
    private readonly List<AlienRing> rings = new List<AlienRing>();
    private readonly List<List<AlienSpawnEntry>> eligibleEntriesByRing = new List<List<AlienSpawnEntry>>();
    private Transform generatedAliens;
    private System.Random random;
    private Vector3 centerPosition;
    private sealed class AlienRing
    {
        public float innerRadius;
        public float outerRadius;
        public int ringIndex;
        public bool activated;
        public bool waitingToRespawn;
        public float respawnAt;
        public bool cleared;
        public GameObject boss;
        public bool reportedMissingBoss;
        public Transform waveRoot;
        public Transform gateRoot;
        public readonly List<GameObject> aliens = new List<GameObject>();
    }
    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
        if (player == null)
        {
            return;
        }
        centerPosition = mapCenter != null ? mapCenter.position : player.position;
        random = seed == 0 ? new System.Random() : new System.Random(seed);
        generatedAliens = new GameObject("Generated Aliens").transform;
        generatedAliens.SetParent(transform, false);
        for (int i = 0; i < ringCount; i++)
        {
            float innerRadius = firstRingInnerRadius + ringWidth * i;
            rings.Add(new AlienRing
            {
                ringIndex = i + 1,
                innerRadius = innerRadius,
                outerRadius = innerRadius + ringWidth
            });
            eligibleEntriesByRing.Add(new List<AlienSpawnEntry>());
        }
        for (int i = 1; i < ringCount; i++)
        {
            rings[i].gateRoot = CreateGate(rings[i].innerRadius, i + 1);
        }
        for (int i = 0; i < alienEntries.Count; i++)
        {
            AlienSpawnEntry entry = alienEntries[i];
            if (entry == null || entry.alienPrefab == null)
            {
                continue;
            }
            if (entry.ring < 1 || entry.ring > ringCount)
            {
                continue;
            }
            if (entry.weight <= 0f)
            {
                continue;
            }
            eligibleEntriesByRing[entry.ring - 1].Add(entry);
        }
        for (int i = 0; i < ringCount; i++)
        {
            List<AlienSpawnEntry> entries = eligibleEntriesByRing[i];
            float weightTotal = GetWeightTotal(entries);
            if (entries.Count == 0 || weightTotal <= 0f)
            {
            }
            else if (Mathf.Abs(weightTotal - 1f) > 0.001f)
            {
            }
        }
    }
    private void Update()
    {
        if (generatedAliens == null || player == null)
        {
            return;
        }
        float playerRadius = HorizontalDistance(player.position, centerPosition);
        foreach (AlienRing ring in rings)
        {
            bool isUnlocked = ring.ringIndex == 1 || rings[ring.ringIndex - 2].cleared;
            if (ring.gateRoot != null && ring.gateRoot.gameObject.activeSelf == isUnlocked)
            {
                ring.gateRoot.gameObject.SetActive(!isUnlocked);
            }

            if (!ring.activated)
            {
                if (!isUnlocked)
                {
                    continue;
                }

                if (playerRadius >= ring.innerRadius - activationDistance
                    && playerRadius <= ring.outerRadius + activationDistance)
                {
                    ring.activated = SpawnRing(ring);
                }

                continue;
            }

            if (!ring.cleared && IsRingCleared(ring))
            {
                ring.cleared = true;
                if (ring.ringIndex < ringCount)
                {
                    OnRingUnlocked?.Invoke(ring.ringIndex + 1);
                }
            }

            if (respawnAfterDefeat && !ring.waitingToRespawn && AllAliensDefeated(ring))
            {
                ring.waitingToRespawn = true;
                ring.respawnAt = Time.time + respawnDelay;
            }
            else if (respawnAfterDefeat && ring.waitingToRespawn && Time.time >= ring.respawnAt)
            {
                ring.waitingToRespawn = !SpawnRing(ring);
                if (ring.waitingToRespawn)
                {
                    ring.respawnAt = Time.time + respawnDelay;
                }
            }
        }
    }

    private bool SpawnRing(AlienRing ring)
    {
        List<AlienSpawnEntry> entries = eligibleEntriesByRing[ring.ringIndex - 1];
        float weightTotal = GetWeightTotal(entries);
        RingProgressionSettings progression = GetProgressionSettings(ring.ringIndex);
        bool requiresBoss = progression != null && progression.clearCondition == RingClearCondition.BossDefeated;
        if (requiresBoss && progression.bossPrefab == null)
        {
            if (!ring.reportedMissingBoss)
            {
                Debug.LogError($"Ring {ring.ringIndex} dont have boss", this);
                ring.reportedMissingBoss = true;
            }
            return false;
        }
        if (weightTotal <= 0f && !requiresBoss)
        {
            return false;
        }
        if (ring.waveRoot != null)
        {
            Destroy(ring.waveRoot.gameObject);
        }
        ring.aliens.Clear();
        GameObject wave = new GameObject("Alien Wave");
        wave.SetActive(false);
        wave.transform.SetParent(generatedAliens, false);
        ring.waveRoot = wave.transform;

        int ringOffset = (ring.ringIndex - 1) * additionalAliensPerRing;
        int minimumAmount = minAliensInFirstRing + ringOffset;
        int maximumAmount = maxAliensInFirstRing + ringOffset;
        int amount = weightTotal > 0f ? random.Next(minimumAmount, maximumAmount + 1) : 0;
        for (int i = 0; i < amount; i++)
        {
            Vector3 position = GetRandomPosition(ring);
            Quaternion rotation = Quaternion.Euler(0f, (float)(random.NextDouble() * 360.0), 0f);

            GameObject alien = Instantiate(ChooseAlienPrefab(entries, weightTotal), position, rotation, ring.waveRoot);
            ring.aliens.Add(alien);
        }
        if (requiresBoss)
        {
            ring.boss = Instantiate(
                progression.bossPrefab,
                GetRandomPosition(ring),
                Quaternion.Euler(0f, (float)(random.NextDouble() * 360.0), 0f),
                ring.waveRoot);
            ring.aliens.Add(ring.boss);
        }
        wave.SetActive(true);
        return true;
    }

    private RingProgressionSettings GetProgressionSettings(int ringIndex)
    {
        int settingsIndex = ringIndex - 1;
        return settingsIndex < ringProgression.Count ? ringProgression[settingsIndex] : null;
    }

    private bool IsRingCleared(AlienRing ring)
    {
        RingProgressionSettings progression = GetProgressionSettings(ring.ringIndex);
        if (progression != null && progression.clearCondition == RingClearCondition.BossDefeated)
        {
            return ring.boss == null;
        }
        return AllAliensDefeated(ring);
    }

    private Vector3 GetRandomPosition(AlienRing ring)
    {
        double angle = random.NextDouble() * Math.PI * 2.0;
        double radius = Math.Sqrt(
            random.NextDouble() * (ring.outerRadius * ring.outerRadius - ring.innerRadius * ring.innerRadius)
            + ring.innerRadius * ring.innerRadius);
        return centerPosition + new Vector3(
            (float)(Math.Cos(angle) * radius),
            0f,
            (float)(Math.Sin(angle) * radius));
    }

    private Transform CreateGate(float radius, int ringIndex)
    {
        Transform gateRoot = new GameObject($"Ring {ringIndex} Gate").transform;
        gateRoot.SetParent(generatedAliens, false);
        float segmentLength = 2f * Mathf.PI * radius / GateSegmentCount;
        for (int i = 0; i < GateSegmentCount; i++)
        {
            float angle = 2f * Mathf.PI * i / GateSegmentCount;
            Vector3 radialDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            GameObject segment = new GameObject("Gate Segment");
            segment.transform.SetParent(gateRoot, false);
            segment.transform.position = centerPosition + radialDirection * radius;
            segment.transform.rotation = Quaternion.LookRotation(radialDirection);
            BoxCollider collider = segment.AddComponent<BoxCollider>();
            collider.size = new Vector3(segmentLength * 1.1f, gateHeight, gateThickness);
        }
        return gateRoot;
    }

    private GameObject ChooseAlienPrefab(List<AlienSpawnEntry> entries, float weightTotal)
    {
        double choice = random.NextDouble() * weightTotal;
        foreach (AlienSpawnEntry entry in entries)
        {
            choice -= entry.weight;
            if (choice < 0.0)
            {
                return entry.alienPrefab;
            }
        }
        return entries[entries.Count - 1].alienPrefab;
    }

    private static float GetWeightTotal(List<AlienSpawnEntry> entries)
    {
        float total = 0f;
        foreach (AlienSpawnEntry entry in entries)
        {
            total += entry.weight;
        }
        return total;
    }

    private static bool AllAliensDefeated(AlienRing ring)
    {
        foreach (GameObject alien in ring.aliens)
        {
            if (alien != null)
            {
                return false;
            }
        }
        return true;
    }
    private static float HorizontalDistance(Vector3 first, Vector3 second)
    {
        first.y = 0f;
        second.y = 0f;
        return Vector3.Distance(first, second);
    }
    private void OnValidate()
    {
        ringCount = Mathf.Max(1, ringCount);
        ringWidth = Mathf.Max(1f, ringWidth);
        minAliensInFirstRing = Mathf.Max(1, minAliensInFirstRing);
        maxAliensInFirstRing = Mathf.Max(minAliensInFirstRing, maxAliensInFirstRing);
        additionalAliensPerRing = Mathf.Max(0, additionalAliensPerRing);
        activationDistance = Mathf.Max(0f, activationDistance);
        gateThickness = Mathf.Max(0.1f, gateThickness);
        gateHeight = Mathf.Max(1f, gateHeight);
        respawnDelay = Mathf.Max(0f, respawnDelay);
        foreach (AlienSpawnEntry entry in alienEntries)
        {
            if (entry == null)
            {
                continue;
            }
            entry.ring = Mathf.Clamp(entry.ring, 1, ringCount);
            entry.weight = Mathf.Clamp01(entry.weight);
        }
    }
}
