using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chargeable : MonoBehaviour
{
    public static System.Action<int> on_charge; // Callback which runs when chargeable is charged
    public static System.Action<int> on_discharge; // Callback which runs when chargeable is discharged

    [SerializeField]
    protected List<int> sets; // Every set the button belongs to

    [SerializeField]
    protected float chargeDuration; // How long the transistor holds a charge
    protected float timeCharged = 0f;
    protected bool currentlyCharged; // Whether or not it's currently charged

    private Animator chargeAnimator;

    protected void Start()
    {
        chargeAnimator = GetComponent<Animator>();
    }


    protected void Update()
    {
        if (currentlyCharged)
        {
            if (chargeDuration > 0f) // Negative charge duration means never discharge
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
    }

    public void ChargeUp()
    {
        timeCharged = chargeDuration;

        if (!currentlyCharged)
        {
            currentlyCharged = true;
            chargeAnimator.SetTrigger("ChargeUp");
            foreach (int set in sets)
            {
                on_charge(set);
            }
        }
    }

    protected void Discharge()
    {
        currentlyCharged = false;
        chargeAnimator.SetTrigger("Discharge");

        foreach (int set in sets)
        {
            on_discharge(set);
        }
    }
}
