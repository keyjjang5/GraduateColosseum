using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateBehaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Animator : " + animator);
        Debug.Log("StateInfo : " + stateInfo.shortNameHash);
        
        Debug.Log("LayerIndex : " + layerIndex);
        GameObject player = GameObject.Find("1pPlayer");
        Debug.Log("PlayerPosition : " + player.transform.position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Now Update");
        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Press U Button");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that processes and affects root motion
        //Debug.Log("OnStateMove");
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        Debug.Log("OnStateMachineEnter");
    }

    public void ForwardWalk(Animator animator)
    {
        animator.SetBool("Walk", true);
        animator.SetTrigger("Trigger");
    }

    public void BackwardWalk(Animator animator)
    {
        animator.SetBool("BWalk", true);
        animator.SetTrigger("Trigger");
    }

    public void WalkStop(Animator animator)
    {
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);
    }
}
