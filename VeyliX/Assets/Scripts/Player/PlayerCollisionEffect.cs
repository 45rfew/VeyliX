
using UnityEngine;

public class PlayerCollisionEffect : MonoBehaviour
{
    public GameObject collisionEffectPrefab;
    [SerializeField] private float collisionThreshold = 1f;
    [SerializeField] private float effectScaleMultiplier = 0.2f;
    private void OnCollisionStay(Collision collision)
    {
        if (collisionEffectPrefab == null)
        {
            return;
        }
        float velocity = collision.relativeVelocity.magnitude;
        if (velocity < collisionThreshold)
        {
            return;
        }
        ContactPoint contact = collision.GetContact(0);
        Quaternion rotation = Quaternion.LookRotation(contact.normal);
        float effectScale = velocity * effectScaleMultiplier;
        GameObject effect = Instantiate(collisionEffectPrefab,contact.point,rotation);
        effect.transform.localScale = Vector3.one * effectScale;
        Destroy(effect, 1f);
    }
}
