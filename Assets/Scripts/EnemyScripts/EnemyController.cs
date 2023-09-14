using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
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
