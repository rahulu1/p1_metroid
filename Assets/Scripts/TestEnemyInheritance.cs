using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyInheritance : MonoBehaviour
{
    public EnemyController controller;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            controller.DisableMovement();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            controller.EnableMovement();
        }
    }
}
