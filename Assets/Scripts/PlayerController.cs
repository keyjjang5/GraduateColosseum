using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    Transform center;

    Vector3 velocity = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    // Ŀ�ǵ���� ����
    Queue<int> commands;
    int postCommand = 0;
    float postCommandTime = 0;
    float currentCommandTime = 0;
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
            // ��
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, Vector3.zero) },
            // ��
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // ����
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, Vector3.zero) }
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
            {4, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, new Vector3(0,500f,200f)) }
        };
        attackInfoDictLK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // �ߴ�ű
            {1, new AttackArea.AttackInfo(16, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �ֱⰢ, ����
            {2, new AttackArea.AttackInfo(24, transform, AttackArea.AttackType.middle, Vector3.zero) },
            // �ɾ� §��
            {3, new AttackArea.AttackInfo(8, transform, AttackArea.AttackType.lower, Vector3.zero) }
        };
        attackInfoDictRK = new Dictionary<int, AttackArea.AttackInfo>()
        {
            // ����ű, ī���ͽ� ����
            {1, new AttackArea.AttackInfo(11, transform, AttackArea.AttackType.upper, new Vector3(0, 3000f, 500f)) },
            // §��
            {2, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.lower, Vector3.zero) },
            // �̽�, �Ѿ���
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.lower, Vector3.zero) }
        };
    }


    // Update is called once per frame
    void Update()
    {
        // �׼� ����
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
        // Ŀ�ǵ�
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
            // �̵� ����
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

            
            // Ŀ�ǵ� ����
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

    // Ŀ�ǵ� queue ����
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

    // Ŀ�ǵ� ã��
    bool searchCommands(string command) 
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
    }

    // �޼� ���� Ž��, ����
    void activeLP()
    {
        // ���ڼ�
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
        // �����ڼ�
        if(status.CurrentState == State.Sitting)
        {
            
        }

        // attackInfo����
        if (code != 0)
        {
            // �ִϸ��̼� ����
            animator.SetInteger("LPunch", code);

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            attackInfoDictLP.TryGetValue(code, out attackInfo);

            // ���ݻ��·� ��ȯ
            status.CurrentState = State.Attacking;
        }

        clearCommands();
    }

    // ������ ���� Ž��, ����
    void activeRP()
    {
        // ���ڼ�
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
            // �ִϸ��̼� ����
            animator.SetInteger("RPunch", code);

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictRP.TryGetValue(code, out attackInfo);

            // ���ݻ��·� ��ȯ
            status.CurrentState = State.Attacking;
        }
            
        clearCommands();
    }

    // �޹� ���� Ž��, ����
    void activeLK()
    {
        // ���ڼ�
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
            // �ִϸ��̼� ����
            animator.SetInteger("LKick", code);
            
            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictLK.TryGetValue(code, out attackInfo);

            // ���ݻ��·� ��ȯ
            status.CurrentState = State.Attacking;
        }
        
        clearCommands();
    }

    // ������ ���� Ž��, ����
    void activeRK()
    {
        // ���ڼ�
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
            // �ִϸ��̼� ����
            animator.SetInteger("RKick", code);

            // �ִϸ޴��� ���� �ʱ�ȭ
            //clearAnimator();

            // attackInfo����
            attackInfoDictRK.TryGetValue(code, out attackInfo);

            // ���ݻ��·� ��ȯ
            status.CurrentState = State.Attacking;
        }
        
        clearCommands();
    }

    // �մ��
    void frontDash()
    {
        //Debug.Log("�ð����� : " + (Time.time - postCommandTime));
        animator.SetBool("FrontDash", true);
    }

    // ����
    void backDash()
    {
        animator.SetBool("BackDash", true);
    }

    // �ɱ�
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

    // �⺻����
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