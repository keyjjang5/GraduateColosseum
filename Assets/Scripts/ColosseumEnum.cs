using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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