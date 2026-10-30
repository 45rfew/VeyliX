using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public enum EffectDirection
    {
        BackAlongBullet,   
        Ricochet,          
        SurfaceNormal    
    }
    public GameObject collisionEffectPrefab;
    [SerializeField] private float effectScale = 3f;
    [SerializeField] private EffectDirection effectDirection = EffectDirection.Ricochet;
    private Rigidbody rb;
    private Vector3 lastVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        //save the velocity every physics step so when a hit happens, this still holds the speed and direction from just before the collision.
        if (rb != null) lastVelocity = rb.linearVelocity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collisionEffectPrefab == null) return;
        ContactPoint contact = collision.GetContact(0);
        Vector3 normal = contact.normal;
        //direction the bullet was travelling
        Vector3 incoming = lastVelocity.sqrMagnitude > 0.0001f ? lastVelocity.normalized : transform.forward;
        //angle between the bullet's path and the surface,  0 = hit dead-on (head on), 90 = grazing along the surface
        float impactAngle = Vector3.Angle(-incoming, normal);
        Vector3 lookDirection;
        switch (effectDirection)
        {
            case EffectDirection.BackAlongBullet:
                lookDirection = -incoming;
                break;
            case EffectDirection.Ricochet:
                lookDirection = Vector3.Reflect(incoming, normal);
                break;
            default:
                lookDirection = normal;
            break;
        }
        Vector3 horizontalLookDirection = Vector3.ProjectOnPlane(lookDirection, Vector3.up);
        if (horizontalLookDirection.sqrMagnitude < 0.0001f)
        {
            horizontalLookDirection = Vector3.ProjectOnPlane(incoming, Vector3.up);
        }
        if (horizontalLookDirection.sqrMagnitude < 0.0001f)
        {
            horizontalLookDirection = Vector3.forward;
        }
        float yaw = Vector3.SignedAngle(Vector3.forward, horizontalLookDirection, Vector3.up);
        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        GameObject effect = Instantiate(collisionEffectPrefab, contact.point, rotation);
        effect.transform.localScale = Vector3.one * effectScale;
        Destroy(effect, 1f);
    }
}