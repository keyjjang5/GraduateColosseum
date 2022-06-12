using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


using Colosseum;

public class PlayerController : MonoBehaviour
{
    protected Animator animator;

    protected Transform center;

    Vector3 velocity = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    // 커맨드관련 변수
    protected Queue<int> commands;
    int postCommand = 0;
    // 커맨드 입력
    // Vector2(horizontal, vertical)
    protected Dictionary<Vector2, Vector3> horiVer = new Dictionary<Vector2, Vector3>()
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
    protected Dictionary<Vector2, int> horiVerCommand = new Dictionary<Vector2, int>()
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
    protected Dictionary<int, AttackArea.AttackInfo> attackInfoDictLP;
    protected Dictionary<int, AttackArea.AttackInfo> attackInfoDictRP;
    protected Dictionary<int, AttackArea.AttackInfo> attackInfoDictLK;
    protected Dictionary<int, AttackArea.AttackInfo> attackInfoDictRK;

    protected Dictionary<int, CommandPattern.Action> attackInfoCommandLP;
    protected Dictionary<int, CommandPattern.Action> attackInfoCommandRP;
    protected Dictionary<int, CommandPattern.Action> attackInfoCommandLK;
    protected Dictionary<int, CommandPattern.Action> attackInfoCommandRK;

    protected Status status;
    public AttackArea.AttackInfo attackInfo;
    FightManager fightManager;

    TestStateBehaviour stateBehaviour;

    CommandSystem commandSystem;

    bool isHit;
    Animator opponentAnimator;
    public UnityEvent HitEvent;
    protected GameObject manager;
    public EffectManager effectManager;

    bool isGuard;

    Coroutine currentCoroutine;
    int currentPriority;

    protected AttackAreaManager attackAreaManager;

    protected ReplaySystem replaySystem;

    protected PlayerController playerController;

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
            // 잽
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 훅
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 어퍼
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 진짜 잽
            {4, new AttackArea.AttackInfo(5, transform, AttackArea.AttackType.upper, new Vector3(0,500f,200f)) },
            // 백블로우
            {5, new AttackArea.AttackInfo(14, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 전진 훅
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 팔꿈치 해머
            {7, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 퀵 훅
            {8, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 바디 블로우
            {9, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) }

        };
        attackInfoDictRP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 투
            {1, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 훅
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 어퍼
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, new Vector3(0, 2000f, 200f)) },
            // 기상 어퍼
            {4, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // 어퍼mk2
            {5, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) },
            // 뒤오손
            {6, new AttackArea.AttackInfo(19, transform, AttackArea.AttackType.upper, Vector3.zero) }
        };
        attackInfoDictLK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 중단킥
            {1, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 쌍기각, 띄우기
            {2, new AttackArea.AttackInfo(24, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 앉아 짠발
            {3, new AttackArea.AttackInfo(8, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // 경천
            {4, new AttackArea.AttackInfo(18, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 파쿰람 미들킥 1-2타
            {5, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 니킥
            {6, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) }
        };
        attackInfoDictRK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 하이킥, 카운터시 띄우기
            {1, new AttackArea.AttackInfo(11, transform, AttackArea.AttackType.upper, new Vector3(0, 2000f, 500f)) },
            // 짠발
            {2, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // 이슬, 넘어짐
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // 중단 오리발
            {4, new AttackArea.AttackInfo(13, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 기상 오른발
            {5, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 뻥발
            {6, new AttackArea.AttackInfo(12, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 이슬
            {7, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // 돌려차기 하이킥
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

        if (tag == "Player")
            playerController = GetComponent<PlayerController>();
        else if (tag == "Enemy")
            playerController = GetComponent<EnemyController_child>();
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

        // 커맨드에 따른 가드 상태 변화
        guardStateBasedOnCommand((CommandEnum)command.CurrentCommand);

        // 상태와 커맨드에 따른 행동
        temp1 = actionBasedOnState(command, (CommandEnum)command.CurrentCommand, command.Commands);

        // 특정 행동을 실행한다.
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

    // 커맨드를 얻는 함수
    public int GetCurrentCommand()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int command);

        return command;
    }

    // 커맨드가 직전 커맨드와 바뀌었는지 확인하는 함수
    public bool IsCommandChange(CommandEnum command)
    {
        if (postCommand != (int)command)
            return true;

        return false;
    }

    // 커맨드가 바뀐것을 queue에 추가하고 많이 쌓이면 오래된 것부터 제거 관리
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

    // 커맨드 찾기
    bool searchCommands(Queue<int> commands, string command) 
    {
        string s = null;
        foreach (int i in commands)
            s += i;
        bool b = s.Contains(command);
        //Debug.Log(command + "는 : " + b);

        return b;
    }

    // 커맨드 초기화
    void clearCommands()
    {
        commands.Clear();
        postCommand = 0;
    }

    // animator 초기화
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
    // 왼손 공격 탐색, 실행
    CommandPattern.Action activeLP(CommandSystem.Command command, Queue<int> commands)
    {
        int code = 0;
        CommandPattern.Action action = null;
        // 상태에 따른 동작
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
        // attackInfo저장
        if (code != 0)
        {
            // 애니메이션 실행
            //animator.SetInteger("LPunch", code);
            //animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            attackInfoDictLP.TryGetValue(code, out attackInfo); // 이 부분을 attackInfo대신 명령 패턴을 꺼내도록 개선 > 둘다 꺼내야함, attackInfo를 히트에서 사용함

            attackInfoCommandLP.TryGetValue(code, out action);

            // 공격상태로 전환
            //status.CurrentState = State.Attacking;

            // 행동중 표시
            //animator.SetBool("Acting", true);
        }

        command.Clear();

        return action;
    }

    // 오른손 공격 탐색, 실행
    CommandPattern.Action activeRP(CommandSystem.Command command, Queue<int> commands)
    {
        int code = 0;
        CommandPattern.Action action = null;

        // 상태에 따른 동작
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
            // 애니메이션 실행
            //animator.SetInteger("RPunch", code);
            //animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRP.TryGetValue(code, out attackInfo);

            attackInfoCommandRP.TryGetValue(code, out action);

            // 공격상태로 전환
            //status.CurrentState = State.Attacking;

            // 행동중 표시
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;

    }

    // 왼발 공격 탐색, 실행
    CommandPattern.Action activeLK(CommandSystem.Command command, Queue<int> commands)
    {
        // 선자세
        int code = 0;
        CommandPattern.Action action = null;

        // 상태에 따른 동작
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
            // 애니메이션 실행
            //animator.SetInteger("LKick", code);
            //animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictLK.TryGetValue(code, out attackInfo);

            attackInfoCommandLK.TryGetValue(code, out action);

            // 공격상태로 전환
            //status.CurrentState = State.Attacking;

            // 행동중 표시
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;
    }

    // 오른발 공격 탐색, 실행
    CommandPattern.Action activeRK(CommandSystem.Command command, Queue<int> commands)
    {
        // 선자세
        int code = 0;
        CommandPattern.Action action = null;

        // 상태에 따른 동작
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
            // 애니메이션 실행
            //animator.SetInteger("RKick", code);
            //animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRK.TryGetValue(code, out attackInfo);

            attackInfoCommandRK.TryGetValue(code, out action);

            // 공격상태로 전환
            //status.CurrentState = State.Attacking;

            // 행동중 표시
            //animator.SetBool("Acting", true);
        }

        //clearCommands();
        command.Clear();
        return action;
    }

    // 움직임 관련 함수
   
    // 대기
    public void idle(int priority, AttackArea.FrameData frameData)
    {
        // 아닐 경우 기존 행동을 지속한다.
        if (currentCoroutine != null)
            return;

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(idle(frameData));
    }

    IEnumerator idle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        status.Guard = GuardState.NoGuard;

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        AttackEnd();
    }


    // 전진
    public void forwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(forwardWalk(frameData));
    }

    protected IEnumerator forwardWalk(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("Walk", true);
        animator.SetTrigger("Trigger");
        status.Guard = GuardState.NoGuard;

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("Walk", false);
        AttackEnd();
    }

    // 전진 대쉬
    public void forwardDash(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("FrontDash", false);
        AttackEnd();
    }

    // 뒤로 걷기
    public void backwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("BWalk", false);
        status.Guard = GuardState.NoGuard;

        AttackEnd();
    }

    // 뒤로 대쉬
    public void backwardDash(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(backwardDash(frameData));
    }

    IEnumerator backwardDash(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("BackDash", true);
        animator.SetTrigger("Trigger");

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("BackDash", false);
        AttackEnd();
    }

    // 앉기
    public void crouch(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        AttackEnd();
    }

    // 앉아서 앞으로 걷기
    public void crouchForwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        animator.SetBool("Walk", false);
        AttackEnd();
    }


    // 앉아서 뒤로 걷기
    public void crouchBackwardWalk(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if (currentPriority == (int)Colosseum.ActionPriority.Move && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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


        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        animator.SetBool("Sit", false);
        animator.SetBool("Standing", true);
        animator.SetBool("BWalk", false);

        status.Guard = GuardState.NoGuard;

        AttackEnd();
    }

    // 히트
    public void hitHigh(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitHigh(frameData));
    }

    IEnumerator hitHigh(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 1);
        animator.SetBool("Hited", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void hitMiddle(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitMiddle(frameData));
    }

    IEnumerator hitMiddle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 2);
        animator.SetBool("Hited", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void hitLow(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(hitLow(frameData));
    }

    IEnumerator hitLow(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetInteger("HitPos", 3);
        animator.SetBool("Hited", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetInteger("HitPos", 0);
        animator.SetBool("Hited", false);
        AttackEnd();
    }

    public void guardHigh(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardHigh(frameData));
    }

    IEnumerator guardHigh(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("StandingGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetBool("StandingGuard", false);
        AttackEnd();
    }

    public void guardMiddle(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardMiddle(frameData));
    }

    IEnumerator guardMiddle(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("StandingGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetBool("StandingGuard", false);
        AttackEnd();
    }

    public void guardLow(int priority, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동이 있을경우 취소하고 피격한다.
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        // 실행
        currentPriority = priority;
        currentCoroutine = StartCoroutine(guardLow(frameData));
    }

    IEnumerator guardLow(AttackArea.FrameData frameData)
    {
        int currentFrame = 0;
        clearAnimator();

        animator.SetBool("CrouchGuard", true);
        opponentAnimator.SetBool("Blocked", true);

        // 행동 시작
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 행동 끝
        currentCoroutine = null;
        animator.SetBool("CrouchGuard", false);
        AttackEnd();
    }


    // 앞대시
    void frontDash()
    {
        animator.SetBool("FrontDash", true);
        //animator.SetTrigger("Trigger");

        clearCommands();
    }

    // 백대시
    void backDash()
    {
        animator.SetBool("BackDash", true);

        clearCommands();
    }

    // 앉기
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

    // 기본상태
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

    // 이 함수가 사용된 이후 선입력을 받음
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

        // 행동 끝 표시
        animator.SetBool("Acting", false);

        // 미완성
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

        // 행동 끝 표시
        animator.SetBool("Acting", false);

        // 미완성
        //animator.SetInteger("TriggerNum", (int)AnimatorTrigger.CrouchingIdle);
    } 

    void FootR()
    {

    }
    void FootL()
    {

    }

    protected void guard(GuardState guard)
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

    // EnemyController에 있던것 옮김
    // 피격 당함
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
        // 가드 확인
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
        // 공격에 따른 피격 모션
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

    // 커맨드 패턴 사용해서 코루틴으로 돌리고 
    // 위의 액티브 코루틴은 중간에 멈추면 안되니까 멈추는거 막고 연계공격은 아직 모르겠음
    // 멈추는거 막을 때 히트로 인한 상황을 염두해 둬야함, 커맨드 패턴 실행 될때 저장해놓고 리플레이로 활용
    // 커맨드 패턴은 active rp lp rk lk에서 뱉어내게 하고 그것을 최종 비교를 통해서 하나씩 실행 그리고 실행한 것을 저장
    public void activeCoroutineLP(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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

        // 공격 시작
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while을 통해서 n프레임시작 n프레임 끝을 지정
        // 히트 판정 시작
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("EndAttackHit");
        // 히트 판정 끝
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineRP(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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


        // 공격 시작
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while을 통해서 n프레임시작 n프레임 끝을 지정
        // 히트 판정 시작
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 히트 판정 끝
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineLK(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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


        // 공격 시작
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // while을 통해서 n프레임시작 n프레임 끝을 지정
        // 히트 판정 시작
        attackAreaManager.SendMessage("StartAttackHit");
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 히트 판정 끝
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        AttackEnd();
    }

    public void activeCoroutineRK(int actionCode, int priority, AttackArea.AttackInfo attackInfo, AttackArea.FrameData frameData)
    {
        // 현재 실행중인 행동의 우선순위가 이동인 경우 중단하고 실행
        if ((currentPriority == (int)Colosseum.ActionPriority.Move || currentPriority == (int)Colosseum.ActionPriority.Dash) && currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        // 입력 받을 수 있는 상황일 경우 어떤 행동인지 확인한다(연계 공격 준비)
        // 아닐 경우 기존 행동을 지속한다.
        else if (currentCoroutine != null)
            return;

        // 실행
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


        // 공격 시작
        while (frameData.startHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        attackAreaManager.SendMessage("StartAttackHit");
        // while을 통해서 n프레임시작 n프레임 끝을 지정
        // 히트 판정 시작
        while (frameData.endHitFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 히트 판정 끝
        attackAreaManager.SendMessage("EndAttackHit");
        while (frameData.actionFrame > currentFrame)
        {
            currentFrame++;
            yield return new WaitForFixedUpdate();
        }
        // 공격 끝
        currentCoroutine = null;
        AttackEnd();
    }
}