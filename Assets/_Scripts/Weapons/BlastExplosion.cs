using UnityEngine;

public class BlastExplosion : Explosion
{
    [Header("Physics Settings")]
    [Tooltip("The base force applied by the explosion.")]
    [SerializeField] private float explosionForce = 700f;

    [Tooltip("How much additional upward force is applied. 0 means no extra upward force, 1 means upward force can be as strong as the radial force.")]
    [SerializeField] private float upwardsForceRatio = 0.5f; // Changed from upwardsModifier

    [Tooltip("Whether to apply an upward lift to objects.")]
    [SerializeField] private bool applyUpwardLift = true;

    [Tooltip("The base torque applied for rotational effect.")]
    [SerializeField] private float torqueForce = 2000f;

    [Tooltip("Whether to apply torque.")]
    [SerializeField] private bool applyTorque = true;

    // Assuming 'radius' is defined in the base 'Explosion' class
    // protected float radius;

    protected override void ProcessTarget(Collider2D hit)
    {
        base.ProcessTarget(hit); // Call base class logic if any

        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType == RigidbodyType2D.Static) // Also ignore static bodies
        {
            return;
        }

        // --- Force Calculation ---
        Vector2 explosionCenter = transform.position;
        Vector2 hitPoint = hit.ClosestPoint(explosionCenter);

        // Direction from explosion center to the closest point on the collider
        Vector2 forceDirection = (hitPoint - explosionCenter);

        float distance = forceDirection.magnitude; // More efficient than Vector2.Distance if we already have the vector

        // Prevent division by zero or NaN if distance is extremely small
        if (distance < 0.001f)
        {
            // If too close, apply a default upward push or just a small random direction
            forceDirection = Random.insideUnitCircle.normalized;
            if (forceDirection == Vector2.zero) forceDirection = Vector2.up; // Failsafe
            distance = 0.001f; // Avoid division by zero in forceFactor
        }
        else
        {
            forceDirection /= distance; // Normalize the direction vector
        }

        // Linear falloff: 1 at center, 0 at radius edge
        // Ensure radius is not zero to prevent division by zero
        float forceFactor = (radius > 0) ? (1f - Mathf.Clamp01(distance / radius)) : 1f;

        if (forceFactor <= 0) return; // Object is outside effective range or at the very edge

        // Base radial force
        Vector2 radialForce = forceDirection * (explosionForce * forceFactor);
        rb.AddForce(radialForce, ForceMode2D.Impulse);

        // Optional Upward Lift
        if (applyUpwardLift && upwardsForceRatio > 0)
        {
            // Apply upward force independently. Its strength is relative to explosionForce and also affected by falloff.
            Vector2 upwardForce = Vector2.up * (explosionForce * upwardsForceRatio * forceFactor);
            rb.AddForce(upwardForce, ForceMode2D.Impulse);
        }

        // --- Torque Calculation ---
        if (applyTorque && torqueForce > 0)
        {
            float randomSign = (Random.value < 0.5f) ? -1f : 1f; // Slightly more common way to get -1 or 1
            float rotationalForce = randomSign * torqueForce * forceFactor;
            rb.AddTorque(rotationalForce, ForceMode2D.Impulse);
        }

        // --- Alternative: Force at Position (more realistic, can induce natural torque) ---
        // If you prefer this, you might not need the separate AddTorque or the separate upward AddForce.
        // The upward component would be part of the main 'forceToApply'.
        /*
        Vector2 pointOfImpact = hitPoint; // Or rb.worldCenterOfMass for a different effect
        Vector2 forceToApply = forceDirection; // Start with normalized direction

        if (applyUpwardLift)
        {
            // Blend upward direction. This is one way to do it.
            // The 'upwardsForceRatio' here would act more like a blend factor for the direction.
            forceToApply = (forceToApply + Vector2.up * upwardsForceRatio).normalized;
        }
        forceToApply *= (explosionForce * forceFactor);
        rb.AddForceAtPosition(forceToApply, pointOfImpact, ForceMode2D.Impulse);
        
        // If using AddForceAtPosition, you might want to reduce or remove the manual AddTorque
        // or use it for a more stylistic, less physically-based additional spin.
        if (applyTorque && torqueForce > 0)
        {
            float randomSign = (Random.value < 0.5f) ? -1f : 1f;
            float stylisticTorque = randomSign * torqueForce * forceFactor * 0.1f; // example: make it smaller
            rb.AddTorque(stylisticTorque, ForceMode2D.Impulse);
        }
        */
    }
}

// using UnityEngine;
//
// public class BlastExplosion : Explosion
// {
//    [SerializeField] private float explosionForce = 700f;
//    [SerializeField] private float upwardsModifier = 3f;
//    [SerializeField] private float torqueForce = 2000f;
//
//    protected override void ProcessTarget(Collider2D hit)
//    {
//       base.ProcessTarget(hit);
//     
//       Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
//       if (rb != null)
//       {
//          Vector2 closestPoint = hit.ClosestPoint(transform.position);
//          Vector2 direction = hit.transform.position - transform.position;
//         
//          float distance = Vector2.Distance(transform.position, closestPoint);
//          float forceFactor = 1f - Mathf.Clamp01(distance / radius);
//         
//          Vector2 forceDirection = direction.normalized;
//          forceDirection.y += upwardsModifier * forceFactor;
//          forceDirection.Normalize();
//         
//          Vector2 force = forceDirection * (explosionForce * forceFactor);
//          rb.AddForce(force, ForceMode2D.Impulse);
//         
//          float randomSign = Random.Range(0, 2) * 2 - 1;
//          float rotationalForce = randomSign * torqueForce * forceFactor;
//          rb.AddTorque(rotationalForce, ForceMode2D.Impulse);
//       }
//    }
// }