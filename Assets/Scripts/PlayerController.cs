using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        center = GameObject.Find("CenterPoint").transform;

        commands = new Queue<int>();

        status = GetComponent<Status>();
        status.CurrentState = State.Standing;

        attackInfo = new AttackArea.AttackInfo();
        attackInfo.Init();

        fightManager = FindObjectOfType<FightManager>();


        attackInfoDictLP = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 잽
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // 훅
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 어퍼
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, Vector3.zero) }
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
            {4, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) }
        };
        attackInfoDictLK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 중단킥
            {1, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 쌍기각, 띄우기
            {2, new AttackArea.AttackInfo(24, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // 앉아 짠발
            {3, new AttackArea.AttackInfo(8, transform, AttackArea.AttackType.lower, Vector3.zero) }
        };
        attackInfoDictRK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // 하이킥, 카운터시 띄우기
            {1, new AttackArea.AttackInfo(11, transform, AttackArea.AttackType.upper, new Vector3(0, 3000f, 500f)) },
            // 짠발
            {2, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // 이슬, 넘어짐
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.lower, Vector3.zero) }
        };
    }


    // Update is called once per frame
    void Update()
    {
        // 액션 실행
        if (Input.GetKeyDown(KeyCode.U))
            activeLP();
        else if (Input.GetKeyDown(KeyCode.I))
            activeRP();
        else if (Input.GetKeyDown(KeyCode.J))
            activeLK();
        else if (Input.GetKeyDown(KeyCode.K))
            activeRK();

        transform.LookAt(center);    
    }

    
    private void FixedUpdate()
    {
        // 커맨드
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int command);
        if (postCommand != command)
            commandChange(command);

        if (command == 4)
            guard(Guard.Stand);
        else if (command == 1)
            guard(Guard.Crouch);
        else
            guard(Guard.NoGuard);

        if (status.CurrentState == State.Standing)
        {
            // 이동 영역
            Vector3 velo = Vector3.zero;
            horiVer.TryGetValue(new Vector2(horizontal, vertical), out velo );

            //velocity.x = 1;
            //velocity.z = 1;
            //GetComponent<Rigidbody>().AddForce(velocity);
            velo = Vector3.Lerp(currentVelocity, velo, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            //GetComponent<Rigidbody>().MovePosition(transform.position+velo);
            //transform.Translate(velo);

            if (command == 6)
                animator.SetBool("Walk", true);
            else if (command == 4)
                animator.SetBool("BWalk", true);
            else if (velo == Vector3.zero)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("BWalk", false);
            }

            
            // 커맨드 영역
            if (command == 6)
                if (searchCommands("656") && animator.GetBool("FrontDash") == false && Time.time - postCommandTime < 0.15f)
                    frontDash();
            if (command == 4)
                if (searchCommands("454") && animator.GetBool("BackDash") == false && Time.time - postCommandTime < 0.15f)
                    backDash();
            if (command == 1 || command == 2 || command == 3)
                sit();
            if (command == 7 || command == 8 || command == 9)
                jump();
            if (command == 5)
                Idle();
        }
        else if(status.CurrentState == State.Sitting)
        {
            if (command == 1)
                animator.SetBool("BWalk", true);
            if (command == 3)
                animator.SetBool("Walk", true);
            if (command == 2)
                sit();
            if (command == 5)
                Idle();
        }
    }

    // 커맨드 queue 관리
    void commandChange(int i)
    {
        commands.Enqueue(i);
        if (commands.Count > 4)
            commands.Dequeue();
        postCommand = i;
        postCommandTime = currentCommandTime;
        currentCommandTime = Time.time;

        string s = null;
        foreach (int j in commands)
            s += j;
        Debug.Log("commands : :" + s);
    }

    // 커맨드 찾기
    bool searchCommands(string command) 
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
    }

    // 왼손 공격 탐색, 실행
    void activeLP()
    {
        // 선자세
        int code = 0;
        Debug.Log("code : " + code);
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56") && postCommand == 6)
                code = 2;
            else if (searchCommands("53") && postCommand == 3)
                code = 3;
            else
                code = 1;
        }
        // 앉은자세
        if(status.CurrentState == State.Sitting)
        {
            
        }

        // attackInfo저장
        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("LPunch", code);

            // 애니메니터 변수 초기화
            //clearAnimator();

            attackInfoDictLP.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;
        }

        clearCommands();
    }

    // 오른손 공격 탐색, 실행
    void activeRP()
    {
        // 선자세
        int code = 0;
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56") && postCommand == 6)
                code = 2;
            else if (searchCommands("53") && postCommand == 3)
                code = 3;
            else
                code = 1;
        }
        if(status.CurrentState == State.Sitting)
        {
            if (postCommand == 5)
                code = 4;
        }

        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("RPunch", code);

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRP.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;
        }
            
        clearCommands();
    }

    // 왼발 공격 탐색, 실행
    void activeLK()
    {
        // 선자세
        int code = 0;
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("59") && postCommand == 9)
                code = 2;
            else
                code = 1;
        }
        if (status.CurrentState == State.Sitting)
        {
            if (postCommand == 2)
                code = 3;
        }

        if (code != 0)
        {
            // 애니메이션 실행
            animator.SetInteger("LKick", code);
            
            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictLK.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;
        }
        
        clearCommands();
    }

    // 오른발 공격 탐색, 실행
    void activeRK()
    {
        // 선자세
        int code = 0;
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("52") && postCommand == 2)
                code = 2;
            else
                code = 1;
        }
        if (status.CurrentState == State.Sitting)
        {
            if (postCommand == 3)
                code = 3;
        }

        if (code != 0)
        { 
            // 애니메이션 실행
            animator.SetInteger("RKick", code);

            // 애니메니터 변수 초기화
            //clearAnimator();

            // attackInfo저장
            attackInfoDictRK.TryGetValue(code, out attackInfo);

            // 공격상태로 전환
            status.CurrentState = State.Attacking;
        }
        
        clearCommands();
    }

    // 앞대시
    void frontDash()
    {
        //Debug.Log("시간간격 : " + (Time.time - postCommandTime));
        animator.SetBool("FrontDash", true);
    }

    // 백대시
    void backDash()
    {
        animator.SetBool("BackDash", true);
    }

    // 앉기
    void sit()
    {
        animator.SetBool("Sit", true);
        animator.SetBool("Walk", false);
        animator.SetBool("BWalk", false);

        //status.CurrentState = State.Sitting;
    }

    void Crouching()
    {
        status.CurrentState = State.Sitting;
    }

    // 기본상태
    void Idle()
    {
        animator.SetBool("Sit", false);
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
        clearCommands();
    }

    void StartAttack()
    {
        status.CurrentState = State.Attacking;
    }

    void AttackEnd()
    {
        animator.SetInteger("LPunch", 0);
        animator.SetInteger("RPunch", 0);
        animator.SetInteger("LKick", 0);
        animator.SetInteger("RKick", 0);

        attackInfo = null;
        status.CurrentState = State.Standing;
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

    void guard(Guard guard)
    {
        status.Guard = guard;
    }
}