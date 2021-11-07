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
    // Ŀ�ǵ� �Է�
    // Vector2(horizontal, vertical)
    Dictionary<Vector2, Vector3> horiVer = new Dictionary<Vector2, Vector3>()
        {
            {new Vector2(-1, -1), Vector3.back + Vector3.right },
            {new Vector2(-1, 0), Vector3.back },
            {new Vector2(-1, 1), Vector3.back + Vector3.left },

            {new Vector2(0, -1), Vector3.right },
            {new Vector2(0, 0), Vector3.zero },
            {new Vector2(0, 1), Vector3.left },

            {new Vector2(1, -1), Vector3.forward + Vector3.right },
            {new Vector2(1, 0), Vector3.forward },
            {new Vector2(1, 1), Vector3.forward + Vector3.left }
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

    State state;
    public AttackArea.AttackInfo attackInfo = new AttackArea.AttackInfo();
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        center = GameObject.Find("CenterPoint").transform;

        commands = new Queue<int>();

        state = State.Standing;
        attackInfo.Init();
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

        if (state == State.Standing)
        {
            // �̵� ����
            Vector3 velo = Vector3.zero;
            horiVer.TryGetValue(new Vector2(horizontal, vertical), out velo);

            velo = Vector3.Lerp(currentVelocity, velo, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velo);

            animator.SetBool("Walk", true);
            if (velo == Vector3.zero)
                animator.SetBool("Walk", false);


            // Ŀ�ǵ� ����
            if (command == 6)
                if (searchCommands("656") && animator.GetBool("FrontDash") == false)
                    frontDash();
            if (command == 4)
                if (searchCommands("454") && animator.GetBool("BackDash") == false)
                    backDash();
        }
    }

    // Ŀ�ǵ� queue ����
    void commandChange(int i)
    {
        commands.Enqueue(i);
        if (commands.Count > 10)
            commands.Dequeue();
        postCommand = i;
        postCommandTime = Time.time;

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
        Debug.Log(command + "�� : " + b);

        return b;
    }

    // Ŀ�ǵ� �ʱ�ȭ
    void clearCommands()
    {
        commands.Clear();
        postCommand = 0;
    }

    // �޼� ���� Ž��, ����
    void activeLP()
    {
        Dictionary<int, AttackArea.AttackInfo> attackInfoDict = new Dictionary<int, AttackArea.AttackInfo>()
        {
            {1, new AttackArea.AttackInfo(10, transform, AttackArea.AttackType.upper, false) },
            {2, new AttackArea.AttackInfo(15, transform, AttackArea.AttackType.middle, false) },
            {3, new AttackArea.AttackInfo(17, transform, AttackArea.AttackType.middle, false) }
        };
        // ���ڼ�
        if (state == State.Standing)
        {
            int code = 0;

            if (searchCommands("56"))
                code = 2;
            else if (searchCommands("53"))
                code = 3;
            else
                code = 1;

            animator.SetInteger("LPunch", code);
            attackInfoDict.TryGetValue(code, out attackInfo);
            state = State.Attacking;
        }

        clearCommands();
    }

    // ������ ���� Ž��, ����
    void activeRP()
    {
        // ���ڼ�
        if (state == State.Standing)
        {
            if (searchCommands("56"))
                animator.SetInteger("RPunch", 2);
            else if (searchCommands("53"))
                animator.SetInteger("RPunch", 3);
            else
                animator.SetInteger("RPunch", 1);
            state = State.Attacking;
        }

        clearCommands();
    }

    // �޹� ���� Ž��, ����
    void activeLK()
    {
        // ���ڼ�
        if (state == State.Standing)
        {
            if (searchCommands("59"))
                animator.SetInteger("LKick", 2);
            else
                animator.SetInteger("LKick", 1);
            state = State.Attacking;
            animator.SetBool("Walk", false);
        }

        clearCommands();
    }

    // ������ ���� Ž��, ����
    void activeRK()
    {
        // ���ڼ�
        if (state == State.Standing)
        {
            if (searchCommands("52"))
                animator.SetInteger("RKick", 2);
            else
                animator.SetInteger("RKick", 1);
            state = State.Attacking;
        }

        clearCommands();
    }

    // �մ��
    void frontDash()
    {
        animator.SetBool("FrontDash", true);
    }

    // ����
    void backDash()
    {
        animator.SetBool("BackDash", true);
    }

    void DashEnd()
    {
        animator.SetBool("FrontDash", false);
        animator.SetBool("BackDash", false);
        clearCommands();
    }

    void AttackEnd()
    {
        animator.SetInteger("LPunch", 0);
        animator.SetInteger("RPunch", 0);
        animator.SetInteger("LKick", 0);
        animator.SetInteger("RKick", 0);

        attackInfo.Init();
        state = State.Standing;
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

// ����
enum State
{
    // ���ڼ�
    Standing,
    // �����ڼ�
    Sitting,
    // ���� ��
    Attacking,
    // �´� ��
    Hited
}