using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Colosseum;

public class EnemyController : MonoBehaviour
{
    bool isHit;
    Animator animator;
    Animator opponentAnimator;
    Status status;
    public UnityEvent HitEvent;
    GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        animator = GetComponent<Animator>();
        opponentAnimator = GameObject.Find("1pPlayer").GetComponent<Animator>();
        status = GetComponent<Status>();

        manager = GameObject.Find("Manager");

        HitEvent.AddListener(() => status.CurrentState = State.Hited);

      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            sit();
            status.Guard = Guard.Crouch;
        }
        else
        {   // 임시
            //status.Guard = Guard.NoGuard;
        }
    }

    // 피격 당함
    public void Hit(AttackArea.AttackInfo attackInfo)
    {
        if (isHit)
            return;

        //Debug.Log("hit");

        if (attackInfo.attackType == AttackArea.AttackType.upper)
        {
            //Debug.Log("in uppper");
            if (status.CurrentState == State.Sitting)
                return;
            if (status.Guard == Guard.Stand)
            {
                //Debug.Log("upper guard");
                standingGuard(attackInfo);
                return;
            }
            //Debug.Log("upper");
        }
        else if(attackInfo.attackType == AttackArea.AttackType.middle)
        {
            if (status.Guard == Guard.Stand)
            {
                //Debug.Log("middle guard");
                standingGuard(attackInfo);
                return;
            }
            //Debug.Log("middle");
        }
        else if(attackInfo.attackType == AttackArea.AttackType.lower)
        {
            if (status.Guard == Guard.Crouch)
            {
                //Debug.Log("lower guard");
                crouchGuard(attackInfo);
                return;
            }
            //Debug.Log("lower");
        }

        Debug.Log("Hited");
        animator.SetBool("Hited", true);
        HitEvent.Invoke();
        status.Damaged(attackInfo.attackPower);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(attackInfo.force);

        manager.SendMessage("UIUpdate");
    }

    void EndHit()
    {
        isHit = false;
        animator.SetBool("Hited", false);

        status.CurrentState = State.Standing;
    }

    void EndGuard()
    {
        animator.SetBool("StandingGuard", false);
        animator.SetBool("CrouchGuard", false);

        status.CurrentState = State.Standing;
    }

    // 앉기
    void sit()
    {
        animator.SetBool("Sit", true);
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);

        status.CurrentState = State.Sitting;
    }

    void standingGuard(AttackArea.AttackInfo attackInfo)
    {
        animator.SetBool("StandingGuard", true);

        GetComponent<Rigidbody>().AddForce(attackInfo.force);

        opponentAnimator.SetBool("Blocked", true);

        manager.SendMessage("UIUpdate");
    }

    void crouchGuard(AttackArea.AttackInfo attackInfo)
    {
        animator.SetBool("CrouchGuard", true);

        GetComponent<Rigidbody>().AddForce(attackInfo.force);

        opponentAnimator.SetBool("Blocked", true);

        manager.SendMessage("UIUpdate");
    }

    void Crouching()
    {
        status.CurrentState = State.Sitting;
    }
}
