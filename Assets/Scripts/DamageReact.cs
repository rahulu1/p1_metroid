using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReact : MonoBehaviour
{
    public enum DamageSource { Projectile, Enemy, Lava}

    protected HasHealth entityHealth;

    protected Rigidbody rigid;

    protected virtual void Start()
    {
        entityHealth = GetComponent<HasHealth>();
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void ReactToDamage(DamageSource source, Vector3 DamagePos) { }
    protected virtual void Die() { }
}
