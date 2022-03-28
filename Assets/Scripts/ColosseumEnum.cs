using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colosseum
{
    // ����
    public enum State
    {
        // ���ڼ�
        Standing,
        // �����ڼ�
        Crouching,
        // ���� ��
        Attacking
        // �´� ��
        //Hitted
    }

    // ���� ����
    public enum Guard
    {
        // �������
        NoGuard,
        // ���ڼ� ����
        Stand,
        // �����ڼ� ����
        Crouch
    }

    // ��Ʈ ����
    public enum HitState 
    {
        NoHit,
        Hit, 
        CounterHit
    }

    // Trigger Number
    public enum AnimatorTrigger
    {
        StandingIdle = 1,
        CrouchingIdle = 11
    }
}