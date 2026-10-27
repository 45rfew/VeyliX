
using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public GameObject collisionEffectPrefab;
    [SerializeField] private float effectScaleMultiplier = 0.3f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionEffectPrefab == null)
        {
            return;
        }
        Vector3 size = collision.transform.localScale;
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.LookRotation(contact.normal);
        float effectScale = size.magnitude * effectScaleMultiplier;
        GameObject effect = Instantiate(collisionEffectPrefab,contact.point,rotation);
        effect.transform.localScale = Vector3.one * 3f;
        Debug.Log("Bullet dsdasdsadsahit: " + collision.gameObject.name);
        Destroy(effect, 1f);
    }
}
