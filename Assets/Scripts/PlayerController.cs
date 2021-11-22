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

    Status status;
    public AttackArea.AttackInfo attackInfo = new AttackArea.AttackInfo();
    FightManager fightManager;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        center = GameObject.Find("CenterPoint").transform;

        commands = new Queue<int>();

        status = GetComponent<Status>();
        status.CurrentState = State.Standing;

        attackInfo.Init();

        fightManager = FindObjectOfType<FightManager>();

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

        if (status.CurrentState == State.Standing)
        {
            // 이동 영역
            Vector3 velo = Vector3.zero;
            horiVer.TryGetValue(new Vector2(horizontal, vertical), out velo );

            //velocity.x = 1;
            //velocity.z = 1;
            //GetComponent<Rigidbody>().AddForce(velocity);
            velo = Vector3.Lerp(currentVelocity, velo, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            GetComponent<Rigidbody>().MovePosition(transform.position+velo);
            //transform.Translate(velo);

            animator.SetBool("Walk", true);
            if (velo == Vector3.zero)
                animator.SetBool("Walk", false);

            
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

    // 왼손 공격 탐색, 실행
    void activeLP()
    {
        Dictionary<int, AttackArea.AttackInfo> attackInfoDict = new Dictionary<int, AttackArea.AttackInfo>()
        {
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, false) },
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, false) },
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, false) }
        };
        // 선자세
        int code = 0;
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56"))
                code = 2;
            else if (searchCommands("53"))
                code = 3;
            else
                code = 1;

            animator.SetInteger("LPunch", code);
        }
        if(status.CurrentState == State.Sitting)
        {
            
        }

        if (code != 0)
            attackInfoDict.TryGetValue(code, out attackInfo);

        clearCommands();
    }

    // 오른손 공격 탐색, 실행
    void activeRP()
    {
        // 선자세
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("56"))
                animator.SetInteger("RPunch", 2);
            else if (searchCommands("53"))
                animator.SetInteger("RPunch", 3);
            else
                animator.SetInteger("RPunch", 1);
            status.CurrentState = State.Attacking;
        }
        if(status.CurrentState == State.Sitting)
        {
            if (postCommand == 5)
                animator.SetInteger("RPunch", 4);
        }

        clearCommands();
    }

    // 왼발 공격 탐색, 실행
    void activeLK()
    {
        // 선자세
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("59"))
                animator.SetInteger("LKick", 2);
            else
                animator.SetInteger("LKick", 1);
            status.CurrentState = State.Attacking;
            animator.SetBool("Walk", false);
        }
        if (status.CurrentState == State.Sitting)
        {
            if (searchCommands("2"))
                animator.SetInteger("LKick", 3);
        }


        //if (code != 0)
        //    attackInfoDict.TryGetValue(code, out attackInfo);

        clearCommands();
    }

    // 오른발 공격 탐색, 실행
    void activeRK()
    {
        // 선자세
        if (status.CurrentState == State.Standing)
        {
            if (searchCommands("52"))
                animator.SetInteger("RKick", 2);
            else
                animator.SetInteger("RKick", 1);
            status.CurrentState = State.Attacking;
        }
        if (status.CurrentState == State.Sitting)
        {
            if (searchCommands("3"))
                animator.SetInteger("RKick", 3);
        }

        clearCommands();
    }

    // 앞대시
    void frontDash()
    {
        Debug.Log("시간간격 : " + (Time.time - postCommandTime));
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

        attackInfo.Init();
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
}