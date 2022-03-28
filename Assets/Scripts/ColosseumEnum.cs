using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colosseum
{
    // 상태
    public enum State
    {
        // 선자세
        Standing,
        // 앉은자세
        Crouching,
        // 공격 중
        Attacking
        // 맞는 중
        //Hitted
    }

    // 가드 상태
    public enum Guard
    {
        // 가드안함
        NoGuard,
        // 선자세 가드
        Stand,
        // 앉은자세 가드
        Crouch
    }

    // 히트 상태
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