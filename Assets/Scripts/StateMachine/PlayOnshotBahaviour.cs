using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnshotBahaviour : StateMachineBehaviour
{
    public AudioClip soundToPlay;
    public float volumn = 1f;
    public bool playOnEnter = true, playOnExit = false, playAfterDelay = false;

    //Delay sound timer
    public float playDelay = 0.25f;
    private float timeSinceEntered = 0;
    private bool hasDelayedSoundPlayed = false;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(playOnEnter)
       {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volumn);
       }

       timeSinceEntered = 0f;
       hasDelayedSoundPlayed = false;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playAfterDelay && !hasDelayedSoundPlayed)
        {
            timeSinceEntered += Time.deltaTime;

            if(timeSinceEntered > playDelay)
            {
                AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volumn);
                hasDelayedSoundPlayed = true;
            }
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(playOnExit)
       {
            AudioSource.PlayClipAtPoint(soundToPlay, animator.gameObject.transform.position, volumn);
       }
    }
}
