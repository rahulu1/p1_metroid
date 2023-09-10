using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 damagePos = new Vector2(transform.position.x, transform.position.y);
            collision.gameObject.GetComponent<HasHealth>().TakeDamage(5, damagePos);
        }
    }
}
