using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAttackBehaviorOld : StateMachineBehaviour
{
    float moveSpeedHolder;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var insWeapon = Instantiate(attackScriptOld.instance.currentWeapon, attackScriptOld.instance.attackPos.position,Quaternion.Euler(new Vector3(0,0,0)));
        insWeapon.transform.parent = attackScriptOld.instance.attackPos;
        moveSpeedHolder = playerControllerOld.instance.moveSpeed;
        playerControllerOld.instance.canMove = false;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerControllerOld.instance.moveSpeed = 0;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackScriptOld.instance.canReceiveInput = true;
        playerControllerOld.instance.moveSpeed = moveSpeedHolder;
        animator.SetFloat("speed", 0f);
        playerControllerOld.instance.canMove = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
