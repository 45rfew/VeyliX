using Unity.AppUI.Core;
using Unity.AppUI.UI;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    public static CrystalSpawner Instance;
    public GameObject crystalPrefab;
    void Awake()
    {
        Instance = this;
    }
    public void DropCrystals(int amount, Vector3 pos)
    {
        int rand = Random.Range(0, 100);
        int chunks = rand < 50 ? 1 : rand > 75 ? 3 : 2;
        if (chunks == 1)
        {
            SpawnCrystals(amount, pos);
        }
        if (chunks == 2)
        {
            int firstAmount = Random.Range(2, amount);
            int secondAmount = amount - firstAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
        }
        if (chunks == 3)
        {
            int firstAmount = Random.Range(2, amount);
            int remaining = amount - firstAmount;
            int secondAmount = Random.Range(2, remaining);
            int thirdAmount = remaining - secondAmount;
            SpawnCrystals(firstAmount, pos);
            SpawnCrystals(secondAmount, pos);
            SpawnCrystals(thirdAmount, pos);
        }   
    }
    void SpawnCrystals(int amount, Vector3 pos)
    {
        if (amount < 1) return;
        GameObject crystal = Instantiate(crystalPrefab, pos, Quaternion.identity);
        crystal.GetComponentInChildren<CrystalPickup>().SetValue(amount);
        //float gemScale = Mathf.Lerp(1f, 20f, (amount - 1f) / 249f);
        float gemScale = amount <= 1 ? 1f :
        amount <= 5 ? Mathf.Lerp(1f, 2f, (amount - 1f) / 4f) :
        amount <= 10 ? Mathf.Lerp(2f, 2.5f, (amount - 5f) / 5f) :
        amount <= 100 ? Mathf.Lerp(2.5f, 12f, (amount - 10f) / 90f) :
        Mathf.Lerp(12f, 20f, (amount - 100f) / 150f);
        crystal.transform.localScale = Vector3.one * gemScale;
        Rigidbody rb = crystal.GetComponent<Rigidbody>();
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDir = new Vector3(randomX, 0, randomZ).normalized;
        rb.AddForce(randomDir * 5, ForceMode.Impulse);
        Destroy(crystal, 10f);       
    }
}