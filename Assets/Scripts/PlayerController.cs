using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ����Ŭ�� �׽�Ʈ ��
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;

    private Animator animator;

    Transform center;

    Vector3 velocity = Vector3.zero;
    Vector3 currentVelocity = Vector3.zero;

    // Ŀ�ǵ���� ����
    Queue<int> commands;
    int postCommand = 0;
    float postCommandTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        center = GameObject.Find("CenterPoint").transform;

        commands = new Queue<int>();
    }

    
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
            //ActiveCommand();

        /*
        // �⺻���� �̵� 211010
        if (Input.GetKey(KeyCode.D))
        {
            //Debug.Log("�������� �ȱ�");
            velocity = Vector3.forward;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //Debug.Log("�ĸ����� �ȱ�");
            velocity = Vector3.back;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            //Debug.Log("���������� �ȱ�");
            velocity = Vector3.left;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //Debug.Log("���������� �ȱ�");
            velocity = Vector3.right;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        */

        transform.LookAt(center);

        // �׽�Ʈ
        if (Input.GetMouseButtonDown(0))
        {
            if (!m_IsOneClick)
            {
                m_Timer = Time.time;
                m_IsOneClick = true;
            }
            else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
            {
                m_IsOneClick = false;
                //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ� 
            }
        }      
    }

    
    private void FixedUpdate()
    {
        // Ŀ�ǵ� �׽�Ʈ 211017
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Ŀ�ǵ� �Է�
        if (horizontal > 0)
        {
            if (vertical > 0)
            {
                // 9����
                //Debug.Log("9");
                if (postCommand != 9)
                    commandChange(9);
            }
            else if (vertical < 0)
            {
                // 3����
                //Debug.Log("3");
                if (postCommand != 3)
                    commandChange(3);
            }
            else
            {
                // 6����
                //Debug.Log("6");
                if (postCommand != 6)
                    commandChange(6);
                if (searchCommands("656") && animator.GetBool("FrontDash") == false)
                    frontDash();
            }
        }
        else if (horizontal < 0)
        {
            if (vertical > 0)
            {
                // 7����
                //Debug.Log("7");
                if (postCommand != 7)
                    commandChange(7);
            }
            else if (vertical < 0)
            {
                // 1����
                //Debug.Log("1");
                if (postCommand != 1)
                    commandChange(1);
            }
            else
            {
                // 4����
                //Debug.Log("4");
                if (postCommand != 4)
                    commandChange(4);
            }
        }
        else if (vertical > 0)
        {
            // 8����
            //Debug.Log("8");
            if (postCommand != 8)
                commandChange(8);
        }
        else if (vertical < 0)
        {
            // 2����
            //Debug.Log("2");
            if (postCommand != 2)
                commandChange(2);
        }
        else
        {
            // �߸�
            //Debug.Log("5");
            if (postCommand != 5)
                commandChange(5);
        }


        // horizontal, vertical�� ���� �̵�
        if (horizontal > 0)
        {
            //Debug.Log("�������� �ȱ�");
            velocity = Vector3.forward;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        if (horizontal < 0)
        {
            //Debug.Log("�ĸ����� �ȱ�");
            velocity = Vector3.back;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        if (vertical > 0)
        {
            //Debug.Log("���������� �ȱ�");
            velocity = Vector3.left;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        if (vertical < 0)
        {
            //Debug.Log("���������� �ȱ�");
            velocity = Vector3.right;
            velocity = Vector3.Lerp(currentVelocity, velocity, Mathf.Min(Time.deltaTime * 5.0f, 1.0f));
            transform.Translate(velocity);

            animator.SetBool("Walk", true);
        }
        if (vertical == 0 && horizontal == 0)
        {
            animator.SetBool("Walk", false);
        }
        // ���� Ŀ�ǵ� ǥ��
        //string s = null;
        //foreach (int i in commands)
        //    s += i;
        //Debug.Log("commands : :" + s);

        //Debug.Log("verticla : " + vertical);
        //Debug.Log("Horizontal : " +horizontal);

        //int j = 0;
        //foreach (char c in Input.inputString)
        //{
        //    commands.Enqueue(c);
        //    if (commands.Count > 10)
        //        commands.Dequeue();
        //    j++;
        //}

        //if (!(j > 0))
        //{
        //    commands.Enqueue('*');
        //    if (commands.Count > 10)
        //        commands.Dequeue();
        //}
        //else
        //    j = 0;

        //int i = 0;
        //foreach (char c in commands)
        //{
        //    Debug.Log("Commands[" + i + "] : " + c);
        //    i++;
        //}
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

    // �մ��
    void frontDash()
    {
        animator.SetBool("FrontDash", true);
        Debug.Log("���� ���");
    }

    // ����
    void backDash()
    {

    }

    void DashEnd()
    {
        animator.SetBool("FrontDash", false);
        clearCommands();
    }

    void FootR()
    {

    }
    void FootL()
    {

    }
}
