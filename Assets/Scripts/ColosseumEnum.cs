using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태
public enum State
{
    // 선자세
    Standing,
    // 앉은자세
    Sitting,
    // 공격 중
    Attacking,
    // 맞는 중
    Hited
}