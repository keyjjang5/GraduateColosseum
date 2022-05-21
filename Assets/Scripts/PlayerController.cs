using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


using Colosseum;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    Transform center;

    Vector3 velocity = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    // Ŀ�ǵ���� ����
    Queue<int> commands;
    int postCommand = 0;
    // Ŀ�ǵ� �Է�
    // Vector2(horizontal, vertical)
    Dictionary<Vector2, Vector3> horiVer = new Dictionary<Vector2, Vector3>()
        {
            {new Vector2(-1, -1), Vector3.zero },
            {new Vector2(-1, 0), Vector3.back },
            {new Vector2(-1, 1), Vector3.zero },

            {new Vector2(0, -1), Vector3.zero },
            {new Vector2(0, 0), Vector3.zero },
            {new Vector2(0, 1), Vector3.zero },

            {new Vector2(1, -1), Vector3.zero },
            {new Vector2(1, 0), Vector3.forward },
            {new Vector2(1, 1), Vector3.zero }
        };
    Dictionary<Vector2, int> horiVerCommand = new Dictionary<Vector2, int>()
        {
            {new Vector2(-1, -1), 1 },
            {new Vector2(-1, 0), 4 },
            {new Vector2(-1, 1), 7 },

            {new Vector2(0, -1), 2 },
            {new Vector2(0, 0), 5 },
            {new Vector2(0, 1), 8 },

            {new Vector2(1, -1), 3 },
            {new Vector2(1, 0), 6 },
            {new Vector2(1, 1), 9 }
        };

    [SerializeField]
    Dictionary<int, AttackArea.AttackInfo> attackInfoDictLP;
    Dictionary<int, AttackArea.AttackInfo> attackInfoDictRP;
    Dictionary<int, AttackArea.AttackInfo> attackInfoDictLK;
    Dictionary<int, AttackArea.AttackInfo> attackInfoDictRK;

    Dictionary<int, CommandPattern.Action> attackInfoCommandLP;
    Dictionary<int, CommandPattern.Action> attackInfoCommandRP;
    Dictionary<int, CommandPattern.Action> attackInfoCommandLK;
    Dictionary<int, CommandPattern.Action> attackInfoCommandRK;

    Status status;
    public AttackArea.AttackInfo attackInfo;
    FightManager fightManager;

    TestStateBehaviour stateBehaviour;

    CommandSystem commandSystem;

    bool isHit;
    Animator opponentAnimator;
    public UnityEvent HitEvent;
    GameObject manager;
    public EffectManager effectManager;

    bool isGuard;

    Coroutine currentCoroutine;
    int currentPriority;

    AttackAreaManager attackAreaManager;

    ReplaySystem replaySystem;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        center = GameObject.Find("CenterPoint").transform;

        commands = new Queue<int>();

        status = GetComponent<Status>();
        status.CurrentState = State.Standing;

        attackInfo = new AttackArea.AttackInfo();

        fightManager = FindObjectOfType<FightManager>();


        attackInfoDictLP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // ��
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ��
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ����
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ��¥ ��
            {4, new AttackArea.AttackInfo(5, transform, AttackArea.AttackType.upper, new Vector3(0,500f,200f)) },
            // ���ο�
            {5, new AttackArea.AttackInfo(14, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ���� ��
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // �Ȳ�ġ �ظ�
            {7, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �� ��
            {8, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // �ٵ� ��ο�
            {9, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) }

        };
        attackInfoDictRP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // ��
            {1, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ��
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ����
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, new Vector3(0, 2000f, 200f)) },
            // ��� ����
            {4, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // ����mk2
            {5, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // �ڿ���
            {6, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.upper, Vector3.zero) }
        };
        attackInfoDictLK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // �ߴ�ű
            {1, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �ֱⰢ, ����
            {2, new AttackArea.AttackInfo(24, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �ɾ� §��
            {3, new AttackArea.AttackInfo(8, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // ��õ
            {4, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ����� �̵�ű 1-2Ÿ
            {5, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ��ű
            {6, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) }
        };
        attackInfoDictRK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // ����ű, ī���ͽ� ����
            {1, new AttackArea.AttackInfo(11, transform, AttackArea.AttackType.upper, new Vector3(0, 2000f, 500f)) },
            // §��
            {2, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // �̽�, �Ѿ���
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // �ߴ� ������
            {4, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ��� ������
            {5, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ����
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �̽�
            {7, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // �������� ����ű
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

        stateBehaviour = animator.GetBehaviour<TestStateBehaviour>();

        commandSystem = FindObjectOfType<CommandSystem>();


        // enemyController
        isHit = false;
        if (transform.name == "1pPlayer")
            opponentAnimator = GameObject.Find("1pPlayer").GetComponent<Animator>();
        else// if (transform.name == "2pPlayer")
            opponentAnimator = GameObject.Find("2pPlayer").GetComponent<Animator>();


        manager = GameObject.Find("Manager");

        //HitEvent.AddListener(() => status.CurrentState = State.Hitted);
        HitEvent.AddListener(() => status.HitState = HitState.Hit);
        HitEvent.AddListener(() => isHit = true);

        effectManager = FindObjectOfType<EffectManager>();

        isGuard = false;

        currentCoroutine = null;
        currentPriority = 0;

        attackAreaManager = GetComponent<AttackAreaManager>();

        replaySystem = FindObjectOfType<ReplaySystem>();
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

        CommandSystem.Command command = null;
        CommandPattern.Action action = null;
        CommandPattern.Action temp1 = null;
        CommandPattern.Action temp2 = null;


        if (transform.name == "1pPlayer")
        {
           command = commandSystem.GetCommand();
        }
        else// if (transform.name == "2pPlayer")
        {
            command = commandSystem.Get2pCommand();
        }

        // Ŀ�ǵ忡 ���� ���� ���� ��ȭ
        guardStateBasedOnCommand((CommandEnum)command.CurrentCommand);

        // ���¿� Ŀ�ǵ忡 ���� �ൿ
        temp1 = actionBasedOnState(command, (CommandEnum)command.CurrentCommand, command.Commands);

        // Ư�� �ൿ�� �����Ѵ�.
        temp2 = PlayAction(command, command.Commands, command.ActiveCode);

        if (temp1 == null)
            action = temp2;
        else if (temp2 == null)
            action = temp1;
        else
            action = (temp1.priority > temp2.priority) ? temp1 : temp2;

        if (action != null)
        {
            action.excute(transform);
            action.record();
        }
    }

    // Ŀ�ǵ带 ��� �Լ�
    public int GetCurrentCommand()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int command);

        return command;
    }

    // Ŀ�ǵ尡 ���� Ŀ�ǵ�� �ٲ������ Ȯ���ϴ� �Լ�
    public bool IsCommandChange(CommandEnum command)
    {
        if (postCommand != (int)command)
            return true;

        return false;
    }

    // Ŀ�ǵ尡 �ٲ���� queue�� �߰��ϰ� ���� ���̸� ������ �ͺ��� ���� ����
    //void addCommand(int command)
    //{
    //    commands.Enqueue(command);
    //    if (commands.Count > 4)
    //        commands.Dequeue();
    //    postCommand = command;
    //    postCommandTime = currentCommandTime;
    //    currentCommandTime = Time.time;

    //    string s = null;
    //    foreach (int j in commands)
    //        s += j;
    //    //Debug.Log("commands : :" + s);
    //}

    // Ŀ�ǵ� ã��
    bool searchCommands(Queue<int> commands, string command) 
    {
        string s = null;
        foreach (int i in commands)
            s += i;
        bool b = s.Contains(command);
        //Debug.Log(command + "�� : " + b);

        return b;
    }

    // Ŀ�ǵ� �ʱ�ȭ
    void clearCommands()
    {
        commands.Clear();
        postCommand = 0;
    }

    // animator �ʱ�ȭ
    void clearAnimator()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);
        animator.SetBool("Sit", false);
        animator.SetBool("Jump", false);
        animator.SetBool("FrontDash", false);
        animator.SetBool("BackDash", false);
        animator.SetBool("Standing", true);
    }

    

    CommandPattern.Action PlayAction(CommandSystem.Command command, Queue<int>commands, KeyCode code)
    {
        CommandPattern.Action action = null;

        if (code == KeyCode.U)
            action = activeLP(command, commands);
        else if (code == KeyCode.I)
            action = activeRP(command, commands);
        else if (code == KeyCode.J)
            action = activeLK(command, commands);
        else if (code == KeyCode.K)
            action = activeRK(command, commands);

        return action;
    }
    // �޼� ���� Ž��, ����
    CommandPattern.Action activeLP(CommandSystem.Command command, Queue<int> commands)
    {
        int code = 0;
        CommandPattern.Action action = null;
        // ���¿� ���� ����
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "656") && command.PostCommand == 6)
                    code = 9;
                else if (command.PostCommand == 6)
                    code = 2;
                else if (command.PostCommand == 3)
                    code = 3;
                else if (command.PostCommand == 4)
                    code = 5;
                else if (command.PostCommand == 9)
                    code = 6;
                else if (command.PostCommand == 7)
                    code = 7;
                else if (command.PostCommand == 8)
                    code = 8;
                else
                    code = 4;
                    //action = new CommandPattern.LeftZap(GetComponent<PlayerController>());
                break;
            case State.Crouching:
                break;
        }
        // attackInfo����
        if (code != 0)
        {
            // �ִϸ��̼� ����
            //animator.SetInteger("LPunch", code);
            //animator.SetTrigger("Trigger");

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            attackInfoDictLP.TryGetValue(code, out attackInfo); // �� �κ��� attackInfo��� ��� ������ �������� ���� > �Ѵ� ��������, attackInfo�� ��Ʈ���� �����

            attackInfoCommandLP.TryGetValue(code, out action);

            // ���ݻ��·� ��ȯ
            //status.CurrentState = State.Attacking;

            // �ൿ�� ǥ��
            //animator.SetBool("Acting", true);
        }

        command.Clear();

        return action;
    }

    // ������ ���� Ž��, ����
    CommandPattern.Action activeRP(CommandSystem.Command command, Queue<int> commands)
    {
        int code = 0;
        CommandPattern.Action action = null;

        // ���¿� ���� ����
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "523") && command.PostCommand == 3)
                    code = 5;
                else if (command.PostCommand == 6)
                    code = 2;
                else if (command.PostCommand == 3)
                    code = 3;
                else if (command.PostCommand == 4)
                    code = 6;
                else
                    code = 1;
                break;
            case State.Crouching:
                if (command.PostCommand == 5)
                    code = 4;
                break;
        }
        if (code != 0)
        {
            // �ִϸ��̼� ����
            //animator.SetInteger("RPunch", code);
            //animator.SetTrigger("Trigger");

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictRP.TryGetValue(code, out attackInfo);

            attackInfoCommandRP.TryGetValue(code, out action);

            // ���ݻ��·� ��ȯ
            //status.CurrentState = State.Attacking;

            // �ൿ�� ǥ��
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;

    }

    // �޹� ���� Ž��, ����
    CommandPattern.Action activeLK(CommandSystem.Command command, Queue<int> commands)
    {
        // ���ڼ�
        int code = 0;
        CommandPattern.Action action = null;

        // ���¿� ���� ����
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "656") && command.PostCommand == 6)
                    code = 4;
                else if (searchCommands(commands, "523") && command.PostCommand == 3)
                    code = 6;
                else if (command.PostCommand == 9)
                    code = 2;
                else if (command.PostCommand == 3)
                    code = 5;
                else
                    code = 1;
                break;
            case State.Crouching:
                if (command.PostCommand == 2)
                    code = 3;
                break;
        }
        if (code != 0)
        {
            // �ִϸ��̼� ����
            //animator.SetInteger("LKick", code);
            //animator.SetTrigger("Trigger");

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictLK.TryGetValue(code, out attackInfo);

            attackInfoCommandLK.TryGetValue(code, out action);

            // ���ݻ��·� ��ȯ
            //status.CurrentState = State.Attacking;

            // �ൿ�� ǥ��
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;
    }

    // ������ ���� Ž��, ����
    CommandPattern.Action activeRK(CommandSystem.Command command, Queue<int> commands)
    {
        // ���ڼ�
        int code = 0;
        CommandPattern.Action action = null;

        // ���¿� ���� ����
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "656") && command.PostCommand == 6)
                    code = 6;
                else if (searchCommands(commands, "523") && command.PostCommand == 3)
                    code = 7;
                else if (command.PostCommand == 2)
                    code = 2;
                else if (command.PostCommand == 3)
                    code = 4;
                else if (command.PostCommand == 4)
                    code = 8;
                else
                    code = 1;
                break;
            case State.Crouching:
                if (command.PostCommand == 3)
                    code = 3;
                else if (command.PostCommand == 5)
                    code = 5;
                break;
        }
        if (code != 0)
        { 
            // �ִϸ��̼� ����
            //animator.SetInteger("RKick", code);
            //animator.SetTrigger("Trigger");

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictRK.TryGetValue(code, out attackInfo);

            attackInfoCommandRK.TryGetValue(code, out action);

            // ���ݻ��·� ��ȯ
            //status.CurrentState = State.Attacking;

            // �ൿ�� ǥ��
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;
    }

    // ������ ���� �Լ�
   
    // ���
    public void idle(int priority, AttackArea.FrameData frameData)
    {
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(idle(frameData));
    }

    IEnumerator idle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        status.Guard = GuardState.NoGuard;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        AttackEnd();
    }


    // ����
    public void forwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(forwardWalk(frameData));
    }

    IEnumerator forwardWalk(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("Walk", true);
        animator.SetTrigger("Trigger");
        status.Guard = GuardState.NoGuard;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("Walk", false);
        AttackEnd();
    }

    // ���� �뽬
    public void forwardDash(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(forwardDash(frameData));
    }

    IEnumerator forwardDash(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("FrontDash", true);
        animator.SetTrigger("Trigger");
        status.Guard = GuardState.NoGuard;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("FrontDash", false);
        AttackEnd();
    }

    // �ڷ� �ȱ�
    public void backwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(backwardWalk(frameData));
    }

    IEnumerator backwardWalk(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("BWalk", true);
        animator.SetTrigger("Trigger");
        status.Guard = GuardState.Stand;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("BWalk", false);
        status.Guard = GuardState.NoGuard;

        AttackEnd();
    }

    // �ڷ� �뽬
    public void backwardDash(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(backwardDash(frameData));
    }

    IEnumerator backwardDash(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("BackDash", true);
        animator.SetTrigger("Trigger");

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("BackDash", false);
        AttackEnd();
    }

    // �ɱ�
    public void crouch(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(crouch(frameData));
    }

    IEnumerator crouch(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("Sit", true);
        animator.SetBool("Standing", false);

        animator.SetTrigger("Trigger");
        status.Guard = GuardState.NoGuard;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        AttackEnd();
    }

    // �ɾƼ� ������ �ȱ�
    public void crouchForwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(crouchForwardWalk(frameData));
    }

    IEnumerator crouchForwardWalk(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("Sit", true);
        animator.SetBool("Standing", false);
        animator.SetBool("Walk", true);
        animator.SetTrigger("Trigger");
        status.Guard = GuardState.NoGuard;

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        animator.SetBool("Walk", false);
        AttackEnd();
    }


    // �ɾƼ� �ڷ� �ȱ�
    public void crouchBackwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(crouchBackwardWalk(frameData));
    }

    IEnumerator crouchBackwardWalk(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("Sit", true);
        animator.SetBool("Standing", false);
        animator.SetBool("BWalk", true);
        animator.SetTrigger("Trigger");

        status.Guard = GuardState.Crouch;


        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        animator.SetBool("BWalk", false);

        status.Guard = GuardState.NoGuard;

        AttackEnd();
    }

    // ��Ʈ
    public void hitHigh(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitHigh(frameData));
    }

    IEnumerator hitHigh(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 1);
        animator.SetBool("Hited", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void hitMiddle(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitMiddle(frameData));
    }

    IEnumerator hitMiddle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 2);
        animator.SetBool("Hited", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void hitLow(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitLow(frameData));
    }

    IEnumerator hitLow(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 3);
        animator.SetBool("Hited", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void guardHigh(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardHigh(frameData));
    }

    IEnumerator guardHigh(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("StandingGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetBool("StandingGuard", false);
        AttackEnd();
    }

    public void guardMiddle(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardMiddle(frameData));
    }

    IEnumerator guardMiddle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("StandingGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetBool("StandingGuard", false);
        AttackEnd();
    }

    public void guardLow(int priority, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� ������� ����ϰ� �ǰ��Ѵ�.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardLow(frameData));
    }

    IEnumerator guardLow(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("CrouchGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // �ൿ ����
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // �ൿ ��
        currentCoroutine = null;
        animator.SetBool("CrouchGuard", false);
        AttackEnd();
    }


    // �մ��
    void frontDash()
    {
        animator.SetBool("FrontDash", true);
        //animator.SetTrigger("Trigger");

        clearCommands();
    }

    // ����
    void backDash()
    {
        animator.SetBool("BackDash", true);

        clearCommands();
    }

    // �ɱ�
    void crouch()
    {
        animator.SetBool("Sit", true);
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);

        animator.SetBool("Standing", false);
        //status.CurrentState = State.Sitting;
    }

    void Crouching()
    {
        status.CurrentState = State.Crouching;
    }

    // �⺻����
    void Idle()
    {
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        //status.CurrentState = State.Standing;
    }

    void Standing()
    {
        status.CurrentState = State.Standing;
    }

    void jump()
    {
        animator.SetBool("Jump", true);
    }

    void EndJump()
    {
        animator.SetBool("Jump", false);
    }

    void DashEnd()
    {
        animator.SetBool("FrontDash", false);
        animator.SetBool("BackDash", false);
        animator.SetBool("Trigger", false);
        clearCommands();
    }

    void StartAttack()
    {
        status.CurrentState = State.Attacking;
    }

    // �� �Լ��� ���� ���� ���Է��� ����
    void AttackEnd()
    {
        animator.SetInteger("LPunch", 0);
        animator.SetInteger("RPunch", 0);
        animator.SetInteger("LKick", 0);
        animator.SetInteger("RKick", 0);
        animator.SetBool("Blocked", false);

        //attackInfo.Clear();
        status.CurrentState = State.Standing;
        Idle();

        // �ൿ �� ǥ��
        animator.SetBool("Acting", false);

        // �̿ϼ�
        //animator.SetInteger("TriggerNum", (int)AnimatorTrigger.StandingIdle);
    }

    void AttackEndCrouch()
    {
        animator.SetInteger("LPunch", 0);
        animator.SetInteger("RPunch", 0);
        animator.SetInteger("LKick", 0);
        animator.SetInteger("RKick", 0);
        animator.SetBool("Blocked", false);

        //attackInfo.Clear();
        status.CurrentState = State.Crouching;
        animator.SetBool("Sit", true);

        // �ൿ �� ǥ��
        animator.SetBool("Acting", false);

        // �̿ϼ�
        //animator.SetInteger("TriggerNum", (int)AnimatorTrigger.CrouchingIdle);
    } 

    void FootR()
    {

    }
    void FootL()
    {

    }

    void guard(GuardState guard)
    {
        status.Guard = guard;
    }

    CommandPattern.Action actionBasedOnState(CommandSystem.Command command, CommandEnum currentCommand, Queue<int> commands)
    {
        CommandPattern.Action action = null;
        switch (status.CurrentState)
        {
            case State.Standing:
                switch (currentCommand)
                {
                    case CommandEnum.E:
                        action = new CommandPattern.ForwardWalk(GetComponent<PlayerController>());
                        if (searchCommands(commands, "656") && animator.GetBool("FrontDash") == false && Time.time - command.PostCommandTime < 0.15f)
                            action = new CommandPattern.ForwardDash(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.W:
                        action = new CommandPattern.BackwardWalk(GetComponent<PlayerController>());
                        if (searchCommands(commands, "454") && animator.GetBool("BackDash") == false && Time.time - command.PostCommand < 0.15f)
                            action = new CommandPattern.BackwardDash(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.Neutral:
                        //stateBehaviour.WalkStop(animator);
                        //Idle();
                            action = new CommandPattern.Idle(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.SW:
                    case CommandEnum.S:
                    case CommandEnum.SE:
                        action = new CommandPattern.Crouch(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.NW:
                    case CommandEnum.N:
                    case CommandEnum.NE:
                        //jump();
                        break;
                }
                break;
            case State.Crouching:
                switch (currentCommand)
                {
                    case CommandEnum.SW:
                        action = new CommandPattern.CrouchBackwardWalk(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.SE:
                        action = new CommandPattern.CrouchForwardWalk(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.S:
                        action = new CommandPattern.Crouch(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.Neutral:
                        //Idle();
                        action = new CommandPattern.Idle(GetComponent<PlayerController>());
                        break;
                }
                break;
            case State.Attacking:
                switch (currentCommand)
                {
                    case CommandEnum.S:
                        action = new CommandPattern.Crouch(GetComponent<PlayerController>());
                        break;
                    case CommandEnum.Neutral:
                        //Idle();
                        break;
                }
                break;
        }
        return action;
    }

    void guardStateBasedOnCommand(CommandEnum command)
    {
        if (command == CommandEnum.W)
            guard(GuardState.Stand);
        else if (command == CommandEnum.SW)
            guard(GuardState.Crouch);
        else
            guard(GuardState.NoGuard);
    }

    // EnemyController�� �ִ��� �ű�
    // �ǰ� ����
    public void Hit(AttackArea.AttackInfo attackInfo)
    {
        CommandPattern.Action action = null;
        //Debug.Log("1");
        if (isHit)
            return;
        //Debug.Log("2");
        if (isGuard)
            return;
        //Debug.Log("3");
        // ���� Ȯ��
        switch (attackInfo.attackType)
        {
            case AttackArea.AttackType.upper:
                if (status.CurrentState == State.Crouching)
                    return;
                if (status.Guard == GuardState.Stand)
                {
                    //standingGuard(attackInfo);
                    action = new CommandPattern.GuardHigh(GetComponent<PlayerController>());
                    effectManager.PlayGuardEffect(attackInfo.hitPos);
                    isGuard = true;

                    action.excute(transform);
                    return;
                }
                break;
            case AttackArea.AttackType.middle:
                if (status.Guard == GuardState.Stand)
                {
                    //standingGuard(attackInfo);
                    action = new CommandPattern.GuardMiddle(GetComponent<PlayerController>());
                    effectManager.PlayGuardEffect(attackInfo.hitPos);
                    isGuard = true;

                    action.excute(transform);
                    return;
                }
                break;
            case AttackArea.AttackType.lower:
                if (status.Guard == GuardState.Crouch)
                {
                    //crouchGuard(attackInfo);
                    action = new CommandPattern.GuardLow(GetComponent<PlayerController>());
                    effectManager.PlayGuardEffect(attackInfo.hitPos);

                    action.excute(transform);
                    return;
                }
                break;
        }
        //Debug.Log("4");
        // ���ݿ� ���� �ǰ� ���
        switch (attackInfo.attackType)
        {
            case AttackArea.AttackType.upper:
                //animator.SetInteger("HitPos", 1);
                effectManager.PlayHitEffect(attackInfo.hitPos);
                action = new CommandPattern.HitHigh(GetComponent<PlayerController>());
                break;
            case AttackArea.AttackType.middle:
                //animator.SetInteger("HitPos", 2);
                effectManager.PlayHitEffect(attackInfo.hitPos);
                action = new CommandPattern.HitMiddle(GetComponent<PlayerController>());
                break;
            case AttackArea.AttackType.lower:
                //animator.SetInteger("HitPos", 3);
                effectManager.PlayHitEffect(attackInfo.hitPos);
                action = new CommandPattern.HitLow(GetComponent<PlayerController>());
                break;
        }

        //Debug.Log("Hited");
        //animator.SetBool("Hited", true);
        if (action != null)
            action.excute();

        HitEvent.Invoke();
        status.Damaged(attackInfo.attackPower);
        //Debug.Log("power : " + attackInfo.attackPower);
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

    // Ŀ�ǵ� ���� ����ؼ� �ڷ�ƾ���� ������ 
    // ���� ��Ƽ�� �ڷ�ƾ�� �߰��� ���߸� �ȵǴϱ� ���ߴ°� ���� ��������� ���� �𸣰���
    // ���ߴ°� ���� �� ��Ʈ�� ���� ��Ȳ�� ������ �־���, Ŀ�ǵ� ���� ���� �ɶ� �����س��� ���÷��̷� Ȱ��
    // Ŀ�ǵ� ������ active rp lp rk lk���� ���� �ϰ� �װ��� ���� �񱳸� ���ؼ� �ϳ��� ���� �׸��� ������ ���� ����
    public void activeCoroutineLP(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(ActiveActionLP(actionCode, attackInfo, frameData));
    }

    IEnumerator ActiveActionLP(int actionCode, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        animator.SetInteger("LPunch", actionCode);
        animator.SetTrigger("Trigger");
        animator.SetBool("Acting", true);

        this.attackInfo = attackInfo;
        status.Guard = GuardState.NoGuard;

        // ���� ����
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while�� ���ؼ� n�����ӽ��� n������ ���� ����
        // ��Ʈ ���� ����
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("EndAttackHit");
        // ��Ʈ ���� ��
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineRP(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(ActiveActionRP(actionCode, attackInfo, frameData));
    }

    IEnumerator ActiveActionRP(int actionCode, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        animator.SetInteger("RPunch", actionCode);
        animator.SetTrigger("Trigger");
        animator.SetBool("Acting", true);

        this.attackInfo = attackInfo;
        status.Guard = GuardState.NoGuard;


        // ���� ����
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while�� ���ؼ� n�����ӽ��� n������ ���� ����
        // ��Ʈ ���� ����
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ��Ʈ ���� ��
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineLK(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(ActiveActionLK(actionCode, attackInfo, frameData));
    }

    IEnumerator ActiveActionLK(int actionCode, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        animator.SetInteger("LKick", actionCode);
        animator.SetTrigger("Trigger");
        animator.SetBool("Acting", true);

        this.attackInfo = attackInfo;
        status.Guard = GuardState.NoGuard;


        // ���� ����
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // while�� ���ؼ� n�����ӽ��� n������ ���� ����
        // ��Ʈ ���� ����
        attackAreaManager.SendMessage("StartAttackHit");
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ��Ʈ ���� ��
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineRK(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // ���� �������� �ൿ�� �켱������ �̵��� ��� �ߴ��ϰ� ����
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // �Է� ���� �� �ִ� ��Ȳ�� ��� � �ൿ���� Ȯ���Ѵ�(���� ���� �غ�)
        // �ƴ� ��� ���� �ൿ�� �����Ѵ�.
        else if (currentCoroutine != null)
            return;

        // ����
        currentPriority = priority;
        currentCoroutine = StartCoroutine(ActiveActionRK(actionCode, attackInfo, frameData));
    }

    IEnumerator ActiveActionRK(int actionCode, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        animator.SetInteger("RKick", actionCode);
        animator.SetTrigger("Trigger");
        animator.SetBool("Acting", true);

        this.attackInfo = attackInfo;
        status.Guard = GuardState.NoGuard;


        // ���� ����
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while�� ���ؼ� n�����ӽ��� n������ ���� ����
        // ��Ʈ ���� ����
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ��Ʈ ���� ��
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // ���� ��
        currentCoroutine = null;
        AttackEnd();
    }
}