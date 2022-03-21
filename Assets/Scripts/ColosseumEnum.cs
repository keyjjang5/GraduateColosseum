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
        Sitting,
        // ���� ��
        Attacking,
        // �´� ��
        Hited
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

    // Trigger Number
    public enum AnimatorTrigger
    {
        StandingIdle = 1,
        CrouchingIdle = 11
    }
}