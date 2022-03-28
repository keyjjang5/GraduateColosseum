using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandSystem : MonoBehaviour
{
    // 커맨드관련 변수
    Queue<int> commands;
    int postCommand = 0;
    float postCommandTime = 0;
    float currentCommandTime = 0;
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
    Dictionary<int, char> commandArrow = new Dictionary<int, char>()
    {
        {1, '!' },
        {2, '@' },
        {3, '#' },
        {4, '$' },
        {5, 'N' },
        {6, '^' },
        {7, '&' },
        {8, '*' },
        {9, '(' }
    };
    Dictionary<KeyCode, char> commandButton = new Dictionary<KeyCode, char>()
    {
        {KeyCode.U, 'q' },
        {KeyCode.I, 'w' },
        {KeyCode.J, 'e' },
        {KeyCode.K, 'r' },
        {KeyCode.T, 'T' }
    };

    Text arrow;
    Text button;
    // Start is called before the first frame update
    void Start()
    {
        commands = new Queue<int>();
        arrow = GameObject.Find("Command_Arrow").GetComponent<Text>();
        button = GameObject.Find("Command_Button").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int command = GetCommand();
        // 키보드 입력
        if (Input.GetKeyDown(KeyCode.U))
            commandView(command, KeyCode.U);
        else if (Input.GetKeyDown(KeyCode.I))
            commandView(command, KeyCode.I);
        else if (Input.GetKeyDown(KeyCode.J))
            commandView(command, KeyCode.J);
        else if (Input.GetKeyDown(KeyCode.K))
            commandView(command, KeyCode.K);
    }

    private void FixedUpdate()
    {
        int command = GetCommand();
        if (IsChange(command))
        {
            commandChange(command);
            commandView(command, KeyCode.T);
        }
    }

    public int GetCommand()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int command);

        return command;
    }

    bool IsChange(int command)
    {
        if (postCommand != command)
            return true;
        return false;
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

    // commandView(int command, KeyCode keyCode)사용
    private void commandView(int command)
    {
        commandArrow.TryGetValue(command, out char c);
        if (arrow.text.Length > 23)
            arrow.text = arrow.text.Remove(0,1);
        arrow.text = arrow.text + c;
    }

    private void commandView(KeyCode keyCode)
    {
        commandButton.TryGetValue(keyCode, out char c);
        if (button.text.Length > 23)
            button.text = button.text.Remove(0, 1);
        button.text = button.text + c;
    }

    public void commandView(int command, KeyCode keyCode)
    {
        commandView(command);
        commandView(keyCode);
    }
}
