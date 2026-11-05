using UnityEngine;

public class CollisionDamage : MonoBehaviour
{
    private float damageMultiplier = 20f;
    private float myDamageRemainder;
    private float otherDamageRemainder;
    private bool isColliding = false;
    private void OnCollisionStay(Collision collision)
    {
        Rigidbody myRb = GetComponent<Rigidbody>();
        Rigidbody otherRb = collision.rigidbody;
        if (myRb == null || otherRb == null) return;
        Health myHealth = GetComponent<Health>();
        Health otherHealth = collision.gameObject.GetComponentInParent<Health>();
        if (myHealth == null || otherHealth == null) return;
        float impactSpeed = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, collision.GetContact(0).normal));
        float damageToOther = impactSpeed / 4f * myRb.mass / otherRb.mass * damageMultiplier;
        float damageToMe = impactSpeed / 4f * otherRb.mass / myRb.mass * damageMultiplier;
        otherDamageRemainder += damageToOther;
        myDamageRemainder += damageToMe;
        int otherDamage = Mathf.FloorToInt(otherDamageRemainder);
        int myDamage = Mathf.FloorToInt(myDamageRemainder);
        otherDamageRemainder -= otherDamage;
        myDamageRemainder -= myDamage;
        if (otherDamage > 0)
        {
            otherHealth.TakeDamage(otherDamage);
        }
        if (myDamage > 0)
        {
            myHealth.TakeDamage(myDamage);
        }
    }
}

