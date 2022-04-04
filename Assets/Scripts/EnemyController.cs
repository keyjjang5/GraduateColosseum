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

        //HitEvent.AddListener(() => status.CurrentState = State.Hitted);
        HitEvent.AddListener(() => status.HitState = HitState.Hit);
        HitEvent.AddListener(() => isHit = true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            sit();
            status.Guard = GuardState.Crouch;
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

        switch(attackInfo.attackType)
        {
            case AttackArea.AttackType.upper:
                if (status.CurrentState == State.Crouching)
                    return;
                if (status.Guard == GuardState.Stand)
                {
                    standingGuard(attackInfo);
                    return;
                }
                break;
            case AttackArea.AttackType.middle:
                if (status.Guard == GuardState.Stand)
                {
                    standingGuard(attackInfo);
                    return;
                }
                break;
            case AttackArea.AttackType.lower:
                if (status.Guard == GuardState.Crouch)
                {
                    crouchGuard(attackInfo);
                    return;
                }
                break;
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

        status.CurrentState = State.Crouching;
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
        status.CurrentState = State.Crouching;
    }
}
