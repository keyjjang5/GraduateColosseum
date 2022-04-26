using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Colosseum;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    Transform center;

    Vector3 velocity = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    // 커맨드관련 변수
    Queue<int> commands;
    int postCommand = 0;
    float postCommandTime = 0;
    float currentCommandTime = 0;
    // 커맨드 입력
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

    Status status;
    public AttackArea.AttackInfo attackInfo;
    FightManager fightManager;

    TestStateBehaviour stateBehaviour;

    CommandSystem commandSystem;
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
            {6, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 이슬
            {7, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 돌려차기 하이킥
            {8, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.middle, Vector3.zero) }

        };

        stateBehaviour = animator.GetBehaviour<TestStateBehaviour>();

        commandSystem = FindObjectOfType<CommandSystem>();
    }


    // Update is called once per frame
    void Update()
    {
    }

    
    private void FixedUpdate()
    {
        transform.LookAt(center);

        var command = commandSystem.GetCommand();

        // 커맨드에 따른 가드 상태 변화
        guardStateBasedOnCommand((CommandEnum)command.CurrentCommand);

        // 상태와 커맨드에 따른 행동
        actionBasedOnState((CommandEnum)command.CurrentCommand, command.Commands);

        // 특정 행동을 실행한다.
        PlayAction(command.Commands, command.ActiveCode);

    }

    void DummyFixedUpdate()
    {
        /*
        if (status.CurrentState == State.Standing)
        {
            // 이동 영역
            Vector3 velo = Vector3.zero;
            horiVer.TryGetValue(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), out velo );
            velo = Vector3.Lerp(currentVelocity, velo, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));

            //velocity.x = 1;
            //velocity.z = 1;
            //GetComponent<Rigidbody>().AddForce(velocity);
            //GetComponent<Rigidbody>().MovePosition(transform.position+velo);
            //transform.Translate(velo);

            // Command에 따른 행동
            switch(command)
            {
                case 6:
                    if (searchCommands("656") && animator.GetBool("FrontDash") == false && Time.time - postCommandTime < 0.15f)
                        frontDash();
                    stateBehaviour.ForwardWalk(animator);

                    break;
                case 4:
                    if (searchCommands("454") && animator.GetBool("BackDash") == false && Time.time - postCommandTime < 0.15f)
                        backDash();
                    stateBehaviour.BackwardWalk(animator);
                    break;
                case 5:
                    stateBehaviour.WalkStop(animator);
                    Idle();
                    break;
                case 1:
                case 2:
                case 3:
                    crouch();
                    break;
                case 7:
                case 8:
                case 9:
                    jump();
                    break;
            }
            /* 
            // 전진
            if (command == 6)
            {
                animator.SetBool("Walk", true);
                animator.SetTrigger("Trigger");
            }
            // 후진
            else if (command == 4)
            {
                animator.SetBool("BWalk", true);
                animator.SetTrigger("Trigger");
            }
            // 정지
            else if (command == 5)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("BWalk", false);
            }
            

            // 커맨드 영역
            if (command == 6)
                if (searchCommands("656") && animator.GetBool("FrontDash") == false && Time.time - postCommandTime < 0.15f)
                    frontDash();
                //else if (animator.GetBool("FrontDash"))
                  //  animator.SetBool("Run", true);
            if (command == 4)
                if (searchCommands("454") && animator.GetBool("BackDash") == false && Time.time - postCommandTime < 0.15f)
                    backDash();
            if (command == 1 || command == 2 || command == 3)
                crouch();
            if (command == 7 || command == 8 || command == 9)
                jump();
            if (command == 5)
                Idle();
            
        }
        else if(status.CurrentState == State.Crouching)
        {
            if (command == 1)
            {
                crouch();
                animator.SetBool("BWalk", true);
            }
            if (command == 3)
            {
                crouch();
                animator.SetBool("Walk", true);
            }
            if (command == 2)
                crouch();
            if (command == 5)
                Idle();
        }
        else if(status.CurrentState == State.Attacking)
        {
            if (command == 2)
                crouch();
            if (command == 5)
                Idle();
        }
    */
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
    void addCommand(int command)
    {
        commands.Enqueue(command);
        if (commands.Count > 4)
            commands.Dequeue();
        postCommand = command;
        postCommandTime = currentCommandTime;
        currentCommandTime = Time.time;

        string s = null;
        foreach (int j in commands)
            s += j;
        Debug.Log("commands : :" + s);
    }

    // 커맨드 찾기
    bool searchCommands(Queue<int> commands, string command) 
    {
        string s = null;
        foreach (int i in commands)
        {
            s += i;
            
        }
        Debug.Log("s : " + s);
        bool b = s.Contains(command);
        Debug.Log(command + "는 : " + b);

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

    //public class Action
    //{
    //    public Action() { Debug.Log("부모클래스 생성자"); }
    //    ~Action() { Debug.Log("부모클래스 소멸자"); }
    //    public virtual void excute() { }
    //}
    //public class LPAction : Action
    //{
    //    public LPAction() { Debug.Log("자식클래스 생성자"); }
    //    ~LPAction() { Debug.Log("자식클래스 소멸자"); }
    //    public override void excute() { }
    //}
    //public class RPAction : Action
    //{
    //    public RPAction() { Debug.Log("자식클래스 생성자"); }
    //    ~RPAction() { Debug.Log("자식클래스 소멸자"); }
    //}
    //public class LKAction : Action
    //{
    //    public LKAction() { Debug.Log("자식클래스 생성자"); }
    //    ~LKAction() { Debug.Log("자식클래스 소멸자"); }
    //}
    //public class RKAction : Action
    //{
    //    public RKAction() { Debug.Log("자식클래스 생성자"); }
    //    ~RKAction() { Debug.Log("자식클래스 소멸자"); }
    //}

    void PlayAction(Queue<int>commands, KeyCode code)
    {
        //Action lpAction = new LPAction();
        //Action rpAction = new RPAction();
        //Action lkAction = new LKAction();
        //Action rkAction = new RKAction();

        if (code == KeyCode.U)
            activeLP(commands);
        else if (code == KeyCode.I)
            activeRP(commands);
        else if (code == KeyCode.J)
            activeLK(commands);
        else if (code == KeyCode.K)
            activeRK(commands);
    }
    // 왼손 공격 탐색, 실행
    void activeLP(Queue<int> commands)
    {
        int code = 0;
        // 상태에 따른 동작
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "65") && commandSystem.GetCommand().PostCommand == 6)
                    code = 9;
                else if (commandSystem.GetCommand().PostCommand == 6)
                    code = 2;
                else if (commandSystem.GetCommand().PostCommand == 3)
                    code = 3;
                else if (commandSystem.GetCommand().PostCommand == 4)
                    code = 5;
                else if (commandSystem.GetCommand().PostCommand == 9)
                    code = 6;
                else if (commandSystem.GetCommand().PostCommand == 7)
                    code = 7;
                else if (commandSystem.GetCommand().PostCommand == 8)
                    code = 8;
                
                else
                    code = 4; // code 1;
                break;
            case State.Crouching:
                break;
        }
        /*
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56") && postCommand == 6)
                code = 2;
            else if (searchCommands("53") && postCommand == 3)
                code = 3;
            else
                code = 4; // code 1;
        }
        // 앉은자세
        if(status.CurrentState == State.Crouching)
        {
            
        }
        */
        // attackInfo저장
        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("LPunch", code);
            animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            attackInfoDictLP.TryGetValue(code, out attackInfo); // 이 부분을 attackInfo대신 명령 패턴을 꺼내도록 개선

            // 공격상태로 전환
            status.CurrentState = State.Attacking;

            // 행동중 표시
            animator.SetBool("Acting", true);
        }
        //commands.Clear();
        //clearCommands();
        commandSystem.GetCommand().Clear();
    }

    // 오른손 공격 탐색, 실행
    void activeRP(Queue<int> commands)
    {
        int code = 0;
        // 상태에 따른 동작
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "56") && commandSystem.GetCommand().PostCommand == 6)
                    code = 2;
                else if (searchCommands(commands, "53") && commandSystem.GetCommand().PostCommand == 3)
                    code = 3;
                else if (searchCommands(commands, "523") && commandSystem.GetCommand().PostCommand == 3)
                    code = 5;
                else if (searchCommands(commands, "54") && commandSystem.GetCommand().PostCommand == 4)
                    code = 6;
                else
                    code = 1;
                break;
            case State.Crouching:
                if (postCommand == 5)
                    code = 4;
                break;
        }
        /*
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56") && postCommand == 6)
                code = 2;
            else if (searchCommands("53") && postCommand == 3)
                code = 3;
            else if (searchCommands("523") && postCommand == 3)
                code = 5;
            else if (searchCommands("54") && postCommand == 4)
                code = 6;
            else
                code = 1;
        }
        if(status.CurrentState == State.Crouching)
        {
            if (postCommand == 5)
                code = 4;
        }
        */
        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("RPunch", code);
            animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRP.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;

            // 행동중 표시
            animator.SetBool("Acting", true);
        }
            
        clearCommands();
    }

    // 왼발 공격 탐색, 실행
    void activeLK(Queue<int> commands)
    {
        // 선자세
        int code = 0;
        // 상태에 따른 동작
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "656") && commandSystem.GetCommand().PostCommand == 6)
                    code = 4;
                else if (searchCommands(commands, "523") && commandSystem.GetCommand().PostCommand == 3)
                    code = 6;
                else if (commandSystem.GetCommand().PostCommand == 9)
                    code = 2;
                else if (commandSystem.GetCommand().PostCommand == 3)
                    code = 5;
                
                else
                    code = 1;
                break;
            case State.Crouching:
                if (postCommand == 2)
                    code = 3;
                break;
        }
        /*
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("59") && postCommand == 9)
                code = 2;
            else if (searchCommands("656") && postCommand == 6)
                code = 4;
            else
                code = 1;
        }
        if (status.CurrentState == State.Crouching)
        {
            if (postCommand == 2)
                code = 3;
        }
        */
        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("LKick", code);
            animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictLK.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;

            // 행동중 표시
            animator.SetBool("Acting", true);
        }
        
        clearCommands();
    }

    // 오른발 공격 탐색, 실행
    void activeRK(Queue<int> commands)
    {
        // 선자세
        int code = 0;
        // 상태에 따른 동작
        switch (status.CurrentState)
        {
            case State.Standing:
                if (searchCommands(commands, "656") && commandSystem.GetCommand().PostCommand == 6)
                    code = 6;
                else if (searchCommands(commands, "523") && commandSystem.GetCommand().PostCommand == 3)
                    code = 7;
                else if (commandSystem.GetCommand().PostCommand == 2)
                    code = 2;
                else if (commandSystem.GetCommand().PostCommand == 3)
                    code = 4;
                else if (commandSystem.GetCommand().PostCommand == 4)
                    code = 8;
                else
                    code = 1;
                break;
            case State.Crouching:
                if (postCommand == 3)
                    code = 3;
                else if (postCommand == 5)
                    code = 5;
                break;
        }
        /*
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("52") && postCommand == 2)
                code = 2;
            else if (searchCommands("53") && postCommand == 3)
                code = 4;
            else
                code = 1;
        }
        if (status.CurrentState == State.Crouching)
        {
            if (postCommand == 3)
                code = 3;
            else if (postCommand == 5)
                code = 5;
        }
        */
        if (code != 0)
        { 
            // 애니메이션 실행
            animator.SetInteger("RKick", code);
            animator.SetTrigger("Trigger");

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRK.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;

            // 행동중 표시
            animator.SetBool("Acting", true);
        }
        
        clearCommands();
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

        attackInfo = null;
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

        attackInfo = null;
        status.CurrentState = State.Crouching;
        animator.SetBool("Sit", true);

        // 행동 끝 표시
        animator.SetBool("Acting", false);

        // 미완성
        //animator.SetInteger("TriggerNum", (int)AnimatorTrigger.CrouchingIdle);
    }

    void Hit()
    {
        
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

    void actionBasedOnState(CommandEnum command, Queue<int> commands)
    {
        switch (status.CurrentState)
        {
            case State.Standing:
                switch (command)
                {
                    case CommandEnum.E:
                        stateBehaviour.ForwardWalk(animator);
                        if (searchCommands(commands, "656") && animator.GetBool("FrontDash") == false && Time.time - commandSystem.GetCommand().PostCommandTime < 0.15f)
                            frontDash();
                        break;
                    case CommandEnum.W:
                        if (searchCommands(commands, "454") && animator.GetBool("BackDash") == false && Time.time - commandSystem.GetCommand().PostCommand < 0.15f)
                            backDash();
                        stateBehaviour.BackwardWalk(animator);
                        break;
                    case CommandEnum.Neutral:
                        stateBehaviour.WalkStop(animator);
                        Idle();
                        break;
                    case CommandEnum.SW:
                    case CommandEnum.S:
                    case CommandEnum.SE:
                        crouch();
                        break;
                    case CommandEnum.NW:
                    case CommandEnum.N:
                    case CommandEnum.NE:
                        //jump();
                        break;
                }
                break;
            case State.Crouching:
                switch (command)
                {
                    case CommandEnum.SW:
                        crouch();
                        animator.SetBool("BWalk", true);
                        break;
                    case CommandEnum.SE:
                        crouch();
                        animator.SetBool("Walk", true);
                        break;
                    case CommandEnum.S:
                        crouch();
                        break;
                    case CommandEnum.Neutral:
                        Idle();
                        break;
                }
                break;
            case State.Attacking:
                switch (command)
                {
                    case CommandEnum.S:
                        crouch();
                        break;
                    case CommandEnum.Neutral:
                        Idle();
                        break;
                }
                break;
        }
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
}