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
    public EffectManager effectManager;

    bool isGuard;
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

        effectManager = FindObjectOfType<EffectManager>();

        isGuard = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            sit();
            status.Guard = GuardState.Crouch;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad8))
        {
            standing();
            status.Guard = GuardState.NoGuard;
        }
    }

    // �ǰ� ����
    public void Hit(AttackArea.AttackInfo attackInfo)
    {
        Debug.Log("6");
        if (isHit)
            return;
        if (isGuard)
            return;

        // ���� Ȯ��
        switch(attackInfo.attackType)
        {
            case AttackArea.AttackType.upper:
                if (status.CurrentState == State.Crouching)
                    return;
                if (status.Guard == GuardState.Stand)
                {
                    standingGuard(attackInfo);
                    effectManager.PlayGuardEffect(attackInfo.hitPos);
                    isGuard = true;
                    return;
                }
                break;
            case AttackArea.AttackType.middle:
                if (status.Guard == GuardState.Stand)
                {
                    standingGuard(attackInfo);
                    effectManager.PlayGuardEffect(attackInfo.hitPos);
                    isGuard = true;
                    return;
                }
                break;
            case AttackArea.AttackType.lower:
                if (status.Guard == GuardState.Crouch)
                {
                    crouchGuard(attackInfo);
                    effectManager.PlayGuardEffect(attackInfo.hitPos);

                    return;
                }
                break;
        }

        // ���ݿ� ���� �ǰ� ���
        switch(attackInfo.attackType)
        {
            case AttackArea.AttackType.upper:
                animator.SetInteger("HitPos", 1);
                effectManager.PlayHitEffect(attackInfo.hitPos);
                break;
            case AttackArea.AttackType.middle:
                animator.SetInteger("HitPos", 2);
                effectManager.PlayHitEffect(attackInfo.hitPos);
                break;
            case AttackArea.AttackType.lower:
                animator.SetInteger("HitPos", 3);
                effectManager.PlayHitEffect(attackInfo.hitPos);
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

        isGuard = false;

        status.CurrentState = State.Standing;
    }

    // �ɱ�
    void sit()
    {
        animator.SetBool("Sit", true);
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);

        animator.SetBool("Standing", false);
        status.CurrentState = State.Crouching;
    }

    // ����
    void standing()
    {
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);
    }

    void Standing()
    {
        status.CurrentState = State.Standing;
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
