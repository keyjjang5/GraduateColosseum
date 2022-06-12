using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Colosseum;


using Panda;


public class EnemyController_child : PlayerController
{
    CommandPattern.Action action;
    EnemyController_child enemyControl;
    private void Start()
    {
        center = GameObject.Find("CenterPoint").transform;

        action = null;
        enemyControl = gameObject.GetComponent<EnemyController_child>();
        replaySystem = FindObjectOfType<ReplaySystem>();

        animator = gameObject.GetComponent<Animator>();

        status = GetComponent<Status>();
        status.CurrentState = State.Standing;

        commands = new Queue<int>();

        attackInfo = new AttackArea.AttackInfo();

        effectManager = FindObjectOfType<EffectManager>();

        attackAreaManager = GetComponent<AttackAreaManager>();

        manager = GameObject.Find("Manager");

        if (tag == "Player")
            playerController = GetComponent<PlayerController>();
        else if (tag == "Enemy")
            playerController = GetComponent<EnemyController_child>();

        attackInfoDictLP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // Àì
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ÈÅ
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ¾îÆÛ
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ÁøÂ¥ Àì
            {4, new AttackArea.AttackInfo(5, transform, AttackArea.AttackType.upper, new Vector3(0,500f,200f)) },
            // ¹éºí·Î¿ì
            {5, new AttackArea.AttackInfo(14, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ÀüÁø ÈÅ
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ÆÈ²ÞÄ¡ ÇØ¸Ó
            {7, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // Äü ÈÅ
            {8, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ¹Ùµð ºí·Î¿ì
            {9, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) }

        };
        attackInfoDictRP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // Åõ
            {1, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ÈÅ
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ¾îÆÛ
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, new Vector3(0, 2000f, 200f)) },
            // ±â»ó ¾îÆÛ
            {4, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // ¾îÆÛmk2
            {5, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // µÚ¿À¼Õ
            {6, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.upper, Vector3.zero) }
        };
        attackInfoDictLK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // Áß´ÜÅ±
            {1, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ½Ö±â°¢, ¶ç¿ì±â
            {2, new AttackArea.AttackInfo(24, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ¾É¾Æ Â§¹ß
            {3, new AttackArea.AttackInfo(8, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // °æÃµ
            {4, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ÆÄÄñ¶÷ ¹ÌµéÅ± 1-2Å¸
            {5, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ´ÏÅ±
            {6, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) }
        };
        attackInfoDictRK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // ÇÏÀÌÅ±, Ä«¿îÅÍ½Ã ¶ç¿ì±â
            {1, new AttackArea.AttackInfo(11, transform, AttackArea.AttackType.upper, new Vector3(0, 2000f, 500f)) },
            // Â§¹ß
            {2, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // ÀÌ½½, ³Ñ¾îÁü
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // Áß´Ü ¿À¸®¹ß
            {4, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ±â»ó ¿À¸¥¹ß
            {5, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // »½¹ß
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ÀÌ½½
            {7, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // µ¹·ÁÂ÷±â ÇÏÀÌÅ±
            {8, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) }

        };

        PlayerController pc = GetComponent<PlayerController>();
        attackInfoCommandLP = new Dictionary<int, CommandPattern.Action>()
        {
            {2, new CommandPattern.LeftHook(pc)},
            {3, new CommandPattern.LeftUppercut(pc) },
            {4, new CommandPattern.LeftZap(pc)},
            {5, new CommandPattern.LeftBackblow(pc) },
            {6, new CommandPattern.LeftForwardHook(pc) },
            {7, new CommandPattern.LeftElbowHammer(pc) },
            {8, new CommandPattern.LeftQuickHook(pc) },
            {9, new CommandPattern.LeftBodyblow(pc) }
        };
        attackInfoCommandRP = new Dictionary<int, CommandPattern.Action>()
        {
            {1, new CommandPattern.RightTwo(pc) },
            {2, new CommandPattern.RightHook(pc) },
            {3, new CommandPattern.RightUpper(pc) },
            {4, new CommandPattern.RightStandUpper(pc) },
            {5, new CommandPattern.RightUpperTwo(pc) },
            {6, new CommandPattern.RightBackRP(pc) }
        };
        attackInfoCommandLK = new Dictionary<int, CommandPattern.Action>()
        {
            {1, new CommandPattern.LeftMiddleKick(pc) },
            {2, new CommandPattern.LeftDoubleCutKick(pc) },
            {3, new CommandPattern.LeftLowKick(pc) },
            {4, new CommandPattern.LeftKickUppercut(pc) },
            {5, new CommandPattern.LeftMiddleKickTwo(pc) },
            {6, new CommandPattern.LeftKneeKick(pc) }
        };

        attackInfoCommandRK = new Dictionary<int, CommandPattern.Action>()
        {
            {1, new CommandPattern.RightHighKick(pc) },
            {2, new CommandPattern.RightLowKick(pc) },
            {3, new CommandPattern.RightLowKickCrouchRound(pc) },
            {4, new CommandPattern.RightMiddleDuckKick(pc) },
            {5, new CommandPattern.RightStandKick(pc) },
            {6, new CommandPattern.RightMiddleStraightKick(pc) },
            {7, new CommandPattern.RightLowKickRound(pc) },
            {8, new CommandPattern.RightHighKickRound(pc) }

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.LookAt(center);

        if (replaySystem.isReplay)
            return;

        if (action != null)
        {
            action.excute(transform);
            action.record();
            action = null;
        }
    }

    float bt_t = 0;
    // PandaBT °ü·Ã ÇÔ¼ö
    [Task]
    void BT_IsClose()
    {
        float halfDistance = Vector3.Distance(center.position, transform.position);

        if (halfDistance < 2)
            ThisTask.Succeed();
        else
            ThisTask.Fail();
    }
    [Task]
    void BT_MoveForward()
    {
        bt_t += Time.fixedDeltaTime;
        if (bt_t >=  0.5f)
        {
            action = new CommandPattern.ForwardWalk(enemyControl);

            ThisTask.Succeed();
            bt_t = 0;
        }
    }
    [Task]
    void BT_IsFar()
    {
        float halfDistance = Vector3.Distance(center.position, transform.position);

        if (halfDistance > 2)
            ThisTask.Succeed();
        else
            ThisTask.Fail();
    }
    [Task]
    void BT_DashForward()
    {
        bt_t += Time.fixedDeltaTime;
        if (bt_t >= 1.0f)
        {
            action = new CommandPattern.ForwardDash(enemyControl);

            ThisTask.Succeed();
            bt_t = 0;
        }
    }
    [Task]
    void BT_Action1()
    {
        bt_t += Time.fixedDeltaTime;
        if (bt_t >= 1.0f)
        {
            
            int target = Random.Range(0, 5);
            switch (target)
            {
                case 0:
                    action = new CommandPattern.RightLowKick(enemyControl);
                    break;
                case 1:
                    action = new CommandPattern.RightHighKick(enemyControl);
                    break;
                case 2:
                    action = new CommandPattern.RightHighKickRound(enemyControl);
                    break;
                case 3:
                    action = new CommandPattern.LeftZap(enemyControl);
                    break;
                case 4:
                    action = new CommandPattern.LeftElbowHammer(enemyControl);
                    break;
                case 5:
                    action = new CommandPattern.RightTwo(enemyControl);
                    break;
            }
            ThisTask.Succeed();
            bt_t = 0;
        }
    }
    [Task]
    void BT_Action2()
    {
        bt_t += Time.fixedDeltaTime;
        if (bt_t >= 1.0f)
        {
            int target = Random.Range(0, 5);
            switch (target)
            {
                case 0:
                    action = new CommandPattern.LeftQuickHook(enemyControl);
                    break;
                case 1:
                    action = new CommandPattern.RightUpperTwo(enemyControl);
                    break;
                case 2:
                    action = new CommandPattern.LeftKneeKick(enemyControl);
                    break;
                case 3:
                    action = new CommandPattern.RightMiddleStraightKick(enemyControl);
                    break;
                case 4:
                    action = new CommandPattern.RightLowKickRound(enemyControl);
                    break;
                case 5:
                    action = new CommandPattern.RightBackRP(enemyControl);
                    break;
            }

            ThisTask.Succeed();
            bt_t = 0;
        }
    }
    [Task]
    void BT_StandGuard()
    {
        guard(GuardState.Stand);

        bt_t += Time.fixedDeltaTime;
        if (bt_t >= 1.0f)
        {
            ThisTask.Succeed();
            bt_t = 0;
            guard(GuardState.NoGuard);
        }
    }
    [Task]
    void BT_CrouchGuard()
    {
        guard(GuardState.Crouch);

        bt_t += Time.fixedDeltaTime;
        if (bt_t >= 1.0f)
        {
            ThisTask.Succeed();
            bt_t = 0;
            guard(GuardState.NoGuard);
        }
    }
}
