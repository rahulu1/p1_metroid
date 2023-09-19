using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected Vector3 lastNonzeroVelocity;

    protected void RecordVelocity(Rigidbody rb)
    {
        bool nonzeroVelocity = Utilities.IsZeroVector(rb.velocity);
        if (nonzeroVelocity)
            lastNonzeroVelocity = rb.velocity;
    }
    public void DisableMovement()
    {
        EnemyController controller = GetController();
        controller.enabled = false;

        Rigidbody rigidbody = controller.GetComponent<Rigidbody>();
        if (rigidbody)
            rigidbody.velocity = Vector3.zero;
    }

    public void EnableMovement()
    {
        GetController().enabled = true;
    }

    // Implemented by derived enemy controller classes; returns controller component
    public abstract EnemyController GetController();
}
