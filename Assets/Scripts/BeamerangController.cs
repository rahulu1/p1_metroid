using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// This script primarily controls the movement and
// despawning of the Beamerang

// TODO: Change sound for beamerang

public class BeamerangController : MonoBehaviour
{
    [SerializeField]
    private float beamerangSpeed; // Speed when shot/recalled

    [SerializeField]
    private float beamerangControlSpeed; // Speed if controlled by player

    [SerializeField]
    private float maxControlSpeed;

    [SerializeField]
    private float maxSplatTime; // Max time it stays on tile before autorecall

    [SerializeField]
    private float maxControlTime; // Max time before exploding in control mode

    private Vector3 lerpPosition;
    private Vector3 targetDirection; // Direction to launch towards
    private Vector3 pointerMouseOffset;

    [SerializeField]
    private LayerMask dontSplat; // Layers not to splat on. Can potentially damage still

    [SerializeField]
    private GameObject controlPointer;
    private Rigidbody rigid;
    private Animator beamerangAnimator;

    // Determines what happens OnTriggerEnter and current animation state
    public enum BeamerangState { firstLaunch, splatted, recalled, ctrlRecalled, detonating }

    private BeamerangState currState = BeamerangState.firstLaunch;



    // Launches Beamerang forward with beamerangSpeed after FormBlast finishes
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        targetDirection = transform.right;
        beamerangAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        beamerangAnimator.SetInteger("State", (int) currState);
        // If controlling Beamerang, update pointer position
        if(currState == BeamerangState.ctrlRecalled)
        {
            controlPointer.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + pointerMouseOffset;
            lerpPosition = Vector3.Lerp(transform.position, controlPointer.transform.position, beamerangControlSpeed);

            if((lerpPosition - transform.position).magnitude > maxControlSpeed)
                lerpPosition = ((lerpPosition - transform.position).normalized * maxControlSpeed) + transform.position;
        }
    }


    void FixedUpdate()
    {
        // If controlling Beamerang, push it towards controlPointer
        if (currState == BeamerangState.ctrlRecalled)
        {
            rigid.position = lerpPosition;
        }
    }


    // When Beamerang collides with a tile
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<DoorCollider>(out DoorCollider dc))
        {
            if(dc.IsUnlocked())
            {
                dc.OnProjectileCollision(this.gameObject);
                Detonate();
                return;
            }
        }
        if (!(Utilities.LayerInMask(dontSplat, other.gameObject.layer)))
        {
            if (currState == BeamerangState.firstLaunch)
            {
                // The commented out section aligns the splat
                // with the middle of tile it hit. Currently
                // disabled until the collider is fixed.

                /*
                Vector3 adjustedPos = transform.position;
                if (Vector3.Dot(transform.forward, Vector3.up) == 0)
                    adjustedPos.y = other.transform.position.y;
                else
                    adjustedPos.x = other.transform.position.x;

                transform.position = adjustedPos;
                */

                SplatOnTile();
            }  
            else if ((currState == BeamerangState.recalled) || (currState == BeamerangState.ctrlRecalled))
                Detonate();
        }
        else
        {
            other.gameObject.TryGetComponent<TransistorController>(out TransistorController tc);

            if(tc != null)
                tc.ChargeUp();
        }
    }


    void SplatOnTile()
    {
        currState = BeamerangState.splatted;
        rigid.velocity = Vector3.zero;
        StartCoroutine(DetonateIfMaxTimeReached(maxSplatTime));
    }


    // Launches the beamerang towards the player
    public void Recall(Vector3 playerPosition)
    {
        currState = BeamerangState.recalled;
        GetComponent<Collider>().includeLayers = LayerMask.GetMask("Player");

        targetDirection = playerPosition - transform.position;
    }


    // Lets player temporarily control beamerang instead of Samus
    // Beamerang moves towards controlPointer with beamerangControlForce
    // Should feel like tugging it with string or something
    public void ControlRecall()
    {
        currState = BeamerangState.ctrlRecalled;
        GetComponent<Collider>().includeLayers = LayerMask.GetMask("Player");

        rigid.velocity = Vector3.zero;
        pointerMouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        controlPointer.SetActive(true);
        StartCoroutine(DetonateIfMaxTimeReached(maxControlTime));
    }


    // Called by animator at the end of FormBlast and Unsplat
    void Launch()
    {
        if (currState == BeamerangState.firstLaunch || currState == BeamerangState.recalled)
        {
            transform.right = targetDirection;
            Vector3 launchVelocity = targetDirection.normalized * beamerangSpeed;
            transform.rotation = Quaternion.Euler(Vector3.forward * Mathf.Atan2(launchVelocity.y, launchVelocity.x) * Mathf.Rad2Deg);
            rigid.velocity = launchVelocity;
        }
    }


    // Destroys Beamerang if in Control Mode or Splatted too long
    IEnumerator DetonateIfMaxTimeReached(float timeToDetonate)
    {
        BeamerangState startState = currState;
        yield return new WaitForSeconds(timeToDetonate);

        // If Beamerang in same state it
        // was when called, detonate it
        if (currState == startState)
            Detonate();
    }


    void Detonate()
    {
        rigid.velocity = Vector3.zero;
        currState = BeamerangState.detonating;
    }

    public void ToggleBeamerangState(Vector3 playerPosition)
    {
        if((currState == BeamerangState.firstLaunch) || (currState == BeamerangState.splatted))
        {
            CapsuleCollider beamerangCollider = GetComponent<CapsuleCollider>();
            


            if (Input.GetKey(KeyCode.LeftShift))
            {
                beamerangCollider.height = 0f;
                beamerangCollider.radius = 0.5f;
                ControlRecall();
            }
            else
            {
                Recall(playerPosition);
            }
        }
        else if((currState == BeamerangState.recalled) || (currState == BeamerangState.ctrlRecalled))
        {
            Detonate();
        }
    }


    void DestroyBeamerang()
    {
        Destroy(this.gameObject);
    }
}
