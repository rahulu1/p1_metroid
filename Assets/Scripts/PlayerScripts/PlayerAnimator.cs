using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerState playerState;
    private PlayerRun playerRun;

    // Every state returns to standing by default
    // Use triggers for all shooting and morph - player weapon and state could do this
    // PlayerAnimator constantly checks for falling - could do this in playerstate
    // but Player Jump can trigger jump anim from
    // Any state except morphed

    /* Triggers, shoot, jump, morph
     * Bools - running, looking up
     * 
     * coverse running and jumping entirely
     * as well as morph and run jump
     * make sure all animations can exit to standing
     */


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
