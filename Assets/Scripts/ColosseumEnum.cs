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
    public enum GuardState
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

    // Ŀ�ǵ� ����ȭ
    public enum CommandEnum
    {
        SW = 1,
        S = 2,
        SE = 3,
        W = 4,
        Neutral = 5,
        E = 6,
        NW = 7,
        N = 8,
        NE = 9
    }

    public enum ActionPriority
    {
        Move,
        Dash,
        LastCommand,
        TwoCommand,
        ThreeCommand,
        Guard,
        Hit
    }
}