using UnityEngine;

public class EnemyCollisionDamage : MonoBehaviour
{
    private float damageMultiplier = 0.05f;
    private float myDamageRemainder;
    private float otherDamageRemainder;
    private bool isColliding = false;

    private void OnCollisionStay(Collision collision)
    {       
        //CollsionDamage(collision);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //CollsionDamage(collision);
    }
    private void CollsionDamage(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody myRb = GetComponent<Rigidbody>();
            Rigidbody otherRb = collision.rigidbody;
            if (myRb == null || otherRb == null) return;
            Health myHealth = GetComponent<Health>();
            Health otherHealth = collision.gameObject.GetComponentInParent<Health>();
            if (myHealth == null || otherHealth == null) return;
            float impactSpeed = Mathf.Abs(Vector3.Dot(collision.relativeVelocity, collision.GetContact(0).normal));
            float damageToOther = impactSpeed / 4f * myRb.mass * damageMultiplier;
            float damageToMe = impactSpeed / 4f * otherRb.mass * damageMultiplier;
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
}