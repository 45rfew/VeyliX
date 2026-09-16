using UnityEngine;
public class CrystalSpawner : MonoBehaviour
{
    public static CrystalSpawner Instance;
    public GameObject crystalPrefab;
    void Awake()
    {
        Instance = this;
    }
    public void DropCrystals(float amount, Vector3 pos)
    {
        int rand = Random.Range(0, 100);
        int chunks = rand < 50 ? 1 : rand > 75 ? 3 : 2;
        if (chunks == 1)
        {
            SpawnCrystals(amount, pos);
        }
        if (chunks == 2)
        {
            float firstAmount = Random.Range(0, amount);
            float secondAmount = amount - firstAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
        }
        if (chunks == 3)
        {
            float firstAmount = Random.Range(0, amount);
            float secondAmount = Random.Range(amount - firstAmount, amount);
            float thirdAmount = amount - firstAmount + secondAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
            SpawnCrystals(thirdAmount, pos);
        }   
    }
    void SpawnCrystals(float amount, Vector3 pos)
    {
        GameObject crystal = Instantiate(crystalPrefab, pos, Quaternion.identity);
        crystal.GetComponentInChildren<CrystalPickup>().SetValue(amount);
        crystal.transform.localScale = Vector3.one * amount / 10;
        Rigidbody rb = crystal.GetComponent<Rigidbody>();
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, 0, randomZ).normalized;
        rb.AddForce(randomDir * 4, ForceMode.Impulse);
        Destroy(crystal, 10f);       
    }
}