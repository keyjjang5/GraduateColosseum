using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

using Colosseum;

// 커맨드와 관련된 활동을 하는 클래스
// 커맨드를 화면에 보이게 하기 위한 클래스, 입력을 여기서 받아서 플레이어 컨트롤러에 넘기는 것도 생각중
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
    Dictionary<int, Command> intToCommand = new Dictionary<int, Command>
    {
        {1, Command.SW },
        {2, Command.S },
        {3, Command.SE },
        {4, Command.W },
        {5, Command.Neutral },
        {6, Command.E },
        {7, Command.NW },
        {8, Command.N },
        {9, Command.NE }
    };
    Dictionary<Command, int> commandToInt = new Dictionary<Command, int>
    {
        {Command.SW, 1},
        {Command.S, 2},
        {Command.SE, 3},
        {Command.W, 4},
        {Command.Neutral, 5},
        {Command.E, 6},
        {Command.NW, 7},
        {Command.N, 8},
        {Command.NE, 9}
    };

    Text arrow;
    Text button;

    StreamWriter sw;
    // Start is called before the first frame update
    void Start()
    {
        commands = new Queue<int>();
        arrow = GameObject.Find("Command_Arrow").GetComponent<Text>();
        button = GameObject.Find("Command_Button").GetComponent<Text>();

        string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        sw = OpenTextFile(fileName + ".txt");
    }

    // Update is called once per frame
    void Update()
    {
        Command command = GetCurrentCommand();
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
        Command command = GetCurrentCommand();
        if (IsCommandChange(command))
        {
            commandChange(command);
            commandView(command, KeyCode.T);
        }

        writeLog(sw, command, postCommandTime);
    }

    private void OnDestroy()
    {
        CloseTextFile(sw);
    }

    public Command GetCurrentCommand()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int numPadCommand);
        intToCommand.TryGetValue(numPadCommand, out Command command);

        return command;
    }

    bool IsCommandChange(Command command)
    {
        commandToInt.TryGetValue(command, out int numPadCommand);
        if (postCommand != numPadCommand)
            return true;
        return false;
    }

    // 커맨드 queue 관리
    void commandChange(Command command)
    {
        commandToInt.TryGetValue(command, out int numPadCommand);

        commands.Enqueue(numPadCommand);
        if (commands.Count > 4)
            commands.Dequeue();
        postCommand = numPadCommand;
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

    public void commandView(Command command, KeyCode keyCode)
    {
        commandToInt.TryGetValue(command, out int numPadCommand);

        commandView(numPadCommand);
        commandView(keyCode);
    }

    StreamWriter OpenTextFile(string fileName)
    {
        // 파일을 연다.
        string path = "Assets/Log/" + fileName;

        Debug.Log(path);

        StreamWriter sw = null;
        if (!File.Exists(path))
        {
            // Create a file to write to.
            sw = new StreamWriter(path);

            sw.WriteLine("## 리플레이 작성을 위한 로그파일");
            sw.WriteLine("## NumPadCommand, PostCommandTime, CurrentTime");
        }
        return sw;
    }

    void ReadTextFile(string fileName)
    {
        string path = "Assets/Log/" + fileName;

        // Open the file to read from.
        using (StreamReader sr = File.OpenText(path))
        {
            string s;
            while ((s = sr.ReadLine()) != null)
            {
                Debug.Log(s);
            }
        }
    }

    void CloseTextFile(StreamWriter sw)
    {
        sw.Flush();
        sw.Close();
    }
    // 로그를 남긴다.
    // 입력된 커맨드와 커맨드 입력 간격이 필요함
    void writeLog(StreamWriter sw, Command command, float postCommandTime)
    {
        commandToInt.TryGetValue(command, out int numPadCommand);
        sw.WriteLine(numPadCommand + " " + postCommandTime + " " + Time.time);
        // 파일에 현재 커맨드와 직전 커맨드 시간, 현재 커맨드 시간을 기록한다.
        // 리플레이 시스템에서 해당 로그를 읽고 행동으로 전환하여 보여준다.
    }
}
