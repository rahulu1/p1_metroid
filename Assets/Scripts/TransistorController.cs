using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistorController : MonoBehaviour
{
    public static System.Action<int> on_transistor_charge; // Callback which runs when transistor is charged
    public static System.Action<int> on_transistor_discharge; // Callback which runs when transistor is discharged

    private Animator transistorAnimator;

    [SerializeField]
    private List<int> sets; // Every set the button belongs to

    [SerializeField]
    private float chargeDuration; // How long the transistor holds a charge
    private float timeCharged = 0f;
    private bool currentlyCharged; // Whether or not it's currently charged

    private void Start()
    {
        transistorAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        if (currentlyCharged)
        {
            if (timeCharged >= 0f)
            {
                timeCharged -= Time.deltaTime;
            }
            else
            {
                Discharge();
            }
        }
    }

    public void ChargeUp()
    {
        timeCharged = chargeDuration;

        if (!currentlyCharged)
        {
            currentlyCharged = true;
            transistorAnimator.SetTrigger("ChargeUp");
            foreach (int set in sets)
            {
                on_transistor_charge(set);
            }
        }
    }

    void Discharge()
    {
        currentlyCharged = false;
        transistorAnimator.SetTrigger("Discharge");

        foreach (int set in sets)
        {
            on_transistor_discharge(set);
        }
    }
}
