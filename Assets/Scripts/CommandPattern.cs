using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern
{
    public class Action
    {
        protected PlayerController playerController;
        protected int actionCode;
        public int priority;

        protected AttackArea.FrameData frameData;
        public AttackArea.AttackInfo attackInfo;

        public Action(){  }
        public Action(PlayerController playerController)
        {
            this.playerController = playerController;
            this.attackInfo = new AttackArea.AttackInfo();
        }
        ~Action() {  }
        public virtual void excute() { }
        public virtual void excute(Transform transform) { }
        public virtual void record() { }

    }

    // 이동 계열
    public class Idle : Action
    {
        public Idle(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(20, 4, 15, 20, 25);
        }
        ~Idle()
        {
            //Debug.Log("Idle 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.idle(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.idle(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class ForwardWalk : Action
    {
        public ForwardWalk(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~ForwardWalk()
        {
            //Debug.Log("ForwardWalk 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.forwardWalk(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.forwardWalk(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class ForwardDash : Action
    {
        public ForwardDash(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Dash;
            frameData = new AttackArea.FrameData(60, 4, 15, 20, 25);
        }
        ~ForwardDash()
        {
            //Debug.Log("ForwardDash 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.forwardDash(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.forwardDash(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class BackwardWalk : Action
    {
        public BackwardWalk(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(45, 4, 15, 20, 25);
        }
        ~BackwardWalk()
        {
            //Debug.Log("BackwardWalk 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.backwardWalk(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.backwardWalk(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class BackwardDash : Action
    {
        public BackwardDash(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Dash;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~BackwardDash()
        {
            //Debug.Log("BackwardDash 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.backwardDash(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.backwardDash(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class Crouch : Action
    {
        public Crouch(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(30, 4, 15, 20, 25);
        }
        ~Crouch()
        {
            //Debug.Log("Crouch 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.crouch(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.crouch(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class CrouchForwardWalk : Action
    {
        public CrouchForwardWalk(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(30, 4, 15, 20, 25);
        }
        ~CrouchForwardWalk()
        {
            //Debug.Log("CrouchForwardWalk 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.crouchForwardWalk(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.crouchForwardWalk(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class CrouchBackwardWalk : Action
    {
        public CrouchBackwardWalk(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Move;
            frameData = new AttackArea.FrameData(30, 4, 15, 20, 25);
        }
        ~CrouchBackwardWalk()
        {
            //Debug.Log("CrouchBackwardWalk 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.crouchBackwardWalk(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.crouchBackwardWalk(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    // 피격 계열
    public class HitHigh : Action
    {
        public HitHigh(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Hit;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~HitHigh()
        {
            //Debug.Log("HitHigh 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.hitHigh(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.hitHigh(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class HitMiddle : Action
    {
        public HitMiddle(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Hit;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~HitMiddle()
        {
            //Debug.Log("HitMiddle 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.hitMiddle(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.hitMiddle(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class HitLow : Action
    {
        public HitLow(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Hit;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~HitLow()
        {
            //Debug.Log("HitLow 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.hitLow(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.hitLow(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class GuardHigh : Action
    {
        public GuardHigh(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Guard;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~GuardHigh()
        {
            //Debug.Log("GuardHigh 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.guardHigh(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.guardHigh(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class GuardMiddle : Action
    {
        public GuardMiddle(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Guard;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~GuardMiddle()
        {
            //Debug.Log("GuardMiddle 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.guardMiddle(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.guardMiddle(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class GuardLow : Action
    {
        public GuardLow(PlayerController playerController) : base(playerController)
        {
            priority = (int)Colosseum.ActionPriority.Guard;
            frameData = new AttackArea.FrameData(40, 4, 15, 20, 25);
        }
        ~GuardLow()
        {
            //Debug.Log("GuardLow 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.guardLow(priority, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.guardLow(priority, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }


    // LP
    public class LeftZap : Action
    {
        public LeftZap(PlayerController playerController) : base(playerController)
        {
            actionCode = 4;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(30, 4, 15, 20, 25);
        }
        ~LeftZap()
        {
            //Debug.Log("LeftZap 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftHook : Action
    {
        public LeftHook(PlayerController playerController) : base(playerController)
        {
            actionCode = 2;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(15, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(30, 4, 15, 20, 25);
        }
        ~LeftHook()
        {
            //Debug.Log("LeftHook 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftUppercut : Action
    {
        public LeftUppercut(PlayerController playerController) : base(playerController)
        {
            actionCode = 3;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(17, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 15, 20, 30, 35);
        }
        ~LeftUppercut()
        {
            //Debug.Log("LeftUppercut 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftBackblow : Action
    {
        public LeftBackblow(PlayerController playerController) : base(playerController)
        {
            actionCode = 5;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(14, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 15, 20, 30, 35);
        }
        ~LeftBackblow()
        {
            //Debug.Log("LeftBackblow 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftForwardHook : Action
    {
        public LeftForwardHook(PlayerController playerController) : base(playerController)
        {
            actionCode = 6;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(14, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 15, 20, 30, 35);
        }
        ~LeftForwardHook()
        {
            ///Debug.Log("LeftForwardHook 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftElbowHammer : Action
    {
        public LeftElbowHammer(PlayerController playerController) : base(playerController)
        {
            actionCode = 7;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(19, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 15, 20, 30, 35);
        }
        ~LeftElbowHammer()
        {
            //Debug.Log("LeftElbowHammer 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftQuickHook : Action
    {
        public LeftQuickHook(PlayerController playerController) : base(playerController)
        {
            actionCode = 8;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(30, 12, 15, 20, 25);
        }
        ~LeftQuickHook()
        {
            //Debug.Log("LeftQuickHook 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftBodyblow : Action
    {
        public LeftBodyblow(PlayerController playerController) : base(playerController)
        {
            actionCode = 9;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(18, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(30, 12, 15, 20, 25);
        }
        ~LeftBodyblow()
        {
            //Debug.Log("LeftBodyblow 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    // RP
    public class RightTwo : Action
    {
        public RightTwo(PlayerController playerController) : base(playerController)
        {
            actionCode = 1;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(12, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 25, 30);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightHook : Action
    {
        public RightHook(PlayerController playerController) : base(playerController)
        {
            actionCode = 2;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(15, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 25, 30);
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightUpper : Action
    {
        public RightUpper(PlayerController playerController) : base(playerController)
        {
            actionCode = 3;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(17, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 20, 25, 35, 40);
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightStandUpper : Action
    {
        public RightStandUpper(PlayerController playerController) : base(playerController)
        {
            actionCode = 4;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(15, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 15, 20, 30, 35);
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightUpperTwo : Action
    {
        public RightUpperTwo(PlayerController playerController) : base(playerController)
        {
            actionCode = 5;
            priority = (int)Colosseum.ActionPriority.TwoCommand;
            attackInfo = new AttackArea.AttackInfo(15, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 15, 20, 25, 30);
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightBackRP : Action
    {
        public RightBackRP(PlayerController playerController) : base(playerController)
        {
            actionCode = 6;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(19, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 20, 25, 35, 40);
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRP(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    // LK
    public class LeftMiddleKick : Action
    {
        public LeftMiddleKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 1;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(16, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(70, 4, 15, 20, 25);
        }
        ~LeftMiddleKick()
        {
            //Debug.Log("LeftMiddleKick 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftDoubleCutKick : Action
    {
        public LeftDoubleCutKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 2;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(24, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(60, 15, 25, 40, 45);
        }
        ~LeftDoubleCutKick()
        {
            //Debug.Log("LeftDoubleCutKick 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftLowKick : Action
    {
        public LeftLowKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 3;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(8, null, AttackArea.AttackType.lower, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 15, 25, 40, 45);
        }
        ~LeftLowKick()
        {
            //Debug.Log("LeftLowKick 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftKickUppercut : Action
    {
        public LeftKickUppercut(PlayerController playerController) : base(playerController)
        {
            actionCode = 4;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(18, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(50, 20, 25, 40, 45);
        }
        ~LeftKickUppercut()
        {
            //Debug.Log("LeftKickUppercut 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftMiddleKickTwo : Action
    {
        public LeftMiddleKickTwo(PlayerController playerController) : base(playerController)
        {
            actionCode = 5;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(13, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(120, 20, 25, 40, 45);
        }
        ~LeftMiddleKickTwo()
        {
            ///Debug.Log("LeftMiddleKickTwo 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class LeftKneeKick : Action
    {
        public LeftKneeKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 6;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(16, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 15, 20, 30, 35);
        }
        ~LeftKneeKick()
        {
            //Debug.Log("LeftKneeKick 소멸자");
        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineLK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    // RK
    public class RightHighKick : Action
    {
        public RightHighKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 1;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(11, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightLowKick : Action
    {
        public RightLowKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 2;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.lower, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightLowKickCrouchRound : Action
    {
        public RightLowKickCrouchRound(PlayerController playerController) : base(playerController)
        {
            actionCode = 3;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(17, null, AttackArea.AttackType.lower, Vector3.zero);
            frameData = new AttackArea.FrameData(120, 20, 35, 65, 70);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightMiddleDuckKick : Action
    {
        public RightMiddleDuckKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 4;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(13, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightStandKick : Action
    {
        public RightStandKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 5;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(40, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightMiddleStraightKick : Action
    {
        public RightMiddleStraightKick(PlayerController playerController) : base(playerController)
        {
            actionCode = 6;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(12, null, AttackArea.AttackType.middle, Vector3.zero);
            frameData = new AttackArea.FrameData(60, 20, 25, 45, 50);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightLowKickRound : Action
    {
        public RightLowKickRound(PlayerController playerController) : base(playerController)
        {
            actionCode = 7;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.lower, Vector3.zero);
            frameData = new AttackArea.FrameData(120, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }

    public class RightHighKickRound : Action
    {
        public RightHighKickRound(PlayerController playerController) : base(playerController)
        {
            actionCode = 8;
            priority = (int)Colosseum.ActionPriority.LastCommand;
            attackInfo = new AttackArea.AttackInfo(10, null, AttackArea.AttackType.upper, Vector3.zero);
            frameData = new AttackArea.FrameData(120, 10, 15, 35, 40);

        }
        // 커맨드를 실행하는 함수
        public override void excute()
        {
            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        public override void excute(Transform transform)
        {
            attackInfo.attacker = transform;

            playerController.activeCoroutineRK(actionCode, priority, attackInfo, frameData);
        }
        // 커맨드 패턴 누적을 해주는 함수
        public override void record()
        {
            GameObject.FindObjectOfType<ReplaySystem>().Record(this);
        }
    }
}