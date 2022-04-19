using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

using Colosseum;

// Ŀ�ǵ�� ���õ� Ȱ���� �ϴ� Ŭ����
// Ŀ�ǵ带 ȭ�鿡 ���̰� �ϱ� ���� Ŭ����, �Է��� ���⼭ �޾Ƽ� �÷��̾� ��Ʈ�ѷ��� �ѱ�� �͵� ������
public class CommandSystem : MonoBehaviour
{
    // Ŀ�ǵ���� ����
    Queue<int> commands;
    int postCommand = 0;
    float postCommandTime = 0;
    float currentCommandTime = 0;
    List<KeyCode> inputs = new List<KeyCode>();

    // �����ϱ� ���� Ŭ����
    public class Command
    {
        Queue<int> commands;
        public Queue<int> Commands { get { return commands; } }
        List<KeyCode> inputs;
        public List<KeyCode> Inputs { get { return inputs; } }
        int postCommand;
        public int PostCommand { get { return postCommand; } }
        int currentCommand;
        public int CurrentCommand { get { return currentCommand; } }
        float postCommandTime;
        public float PostCommandTime { get { return postCommandTime; } }
        float currentCommandTime;
        public float CurrentCommandTime { get { return currentCommandTime; } }
        KeyCode activeCode;
        public KeyCode ActiveCode { get { return activeCode; } }

        public Command()
        {
            commands = new Queue<int>();
            inputs = new List<KeyCode>();
            postCommand = 0;
            currentCommand = 0;
            postCommandTime = 0;
            currentCommandTime = 0;
            activeCode = 0;
        }

        public void Update(Queue<int> commands, List<KeyCode> inputs, int postCommand, int currentCommand, float postCommandTime, float currentCommandTime, KeyCode input)
        {
            this.commands = commands;
            this.inputs = inputs;
            this.postCommand = postCommand;
            this.postCommandTime = postCommandTime;
            this.currentCommand = currentCommand;
            this.currentCommandTime = currentCommandTime;
            this.activeCode = input;
        }

        public void Clear()
        {
            commands.Clear();
            inputs.Clear();
        }
    }
    Command command;

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
        {KeyCode.T, 'T' },
        {KeyCode.A, 'a' },
        {KeyCode.S, 's' },
        {KeyCode.D, 'd' },
        {KeyCode.Z, 'A' },
        {KeyCode.X, 'S' },
        {KeyCode.C, 'D' },
        {KeyCode.Alpha1, 'Q' },
        {KeyCode.Alpha2, 'W' },
        {KeyCode.Alpha3, 'E' },
        {KeyCode.Alpha4, 'R' },
        {KeyCode.Alpha5, 't' }
    };

    List<KeyCode> inputTypes = new List<KeyCode>()
    {
        KeyCode.U,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K
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

        command = new Command();
    }

    // Update is called once per frame
    void Update()
    {
        //int command = GetCurrentCommand();
        //// Ű���� �Է�
        //if (Input.GetKeyDown(KeyCode.U))
        //    commandView(command, KeyCode.U);
        //else if (Input.GetKeyDown(KeyCode.I))
        //    commandView(command, KeyCode.I);
        //else if (Input.GetKeyDown(KeyCode.J))
        //    commandView(command, KeyCode.J);
        //else if (Input.GetKeyDown(KeyCode.K))
        //    commandView(command, KeyCode.K);
    }

    private void FixedUpdate()
    {
        // ���� �Է��� �ʱ�ȭ�Ѵ�.
        inputs.Clear();

        int currentCommand = GetCurrentCommand();

        if (Input.anyKeyDown)
        {
            // ���� ��� �ϴ� Ű�� �Է��� ���Դٸ� �߰��Ѵ�.
            foreach (var code in inputTypes)
            {
                if (Input.GetKeyDown(code))
                    inputs.Add(code);
            }
            // �ӽ÷� ���⿡ ����� ����, ��� ������ ��� �ʿ�
            commandView(currentCommand, inputs);
        }

        AddArrowCommand(commands, currentCommand);

        command.Update(commands, inputs, postCommand, currentCommand, postCommandTime, currentCommandTime, inputsToActiveCode(inputs));

        writeLog(sw, currentCommand, postCommandTime);
    }

    void AddArrowCommand(Queue<int> commands, int currentCommand)
    {
        // queue�� Ŀ�ǵ� �߰�
        // �߰��ϴ� �������� ���� ť�� �߰������� Ȯ��
        if (postCommand != currentCommand)
        {
            commandChange(currentCommand);
            //commandView(currentCommand, KeyCode.T);
        }
    }

    private void OnDestroy()
    {
        CloseTextFile(sw);
    }

    public int GetCurrentCommand()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        horiVerCommand.TryGetValue(new Vector2(horizontal, vertical), out int command);

        return command;
    }

    bool IsCommandChange(CommandEnum command)
    {
        if (postCommand != (int)command)
            return true;
        return false;
    }

    // Ŀ�ǵ� queue ����
    void commandChange(int command)
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
        //Debug.Log("commands : :" + s);
    }

    // commandView(int command, KeyCode keyCode)���
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

    public void commandView(int command, List<KeyCode> inputs)
    {
        
        commandView(command);

        KeyCode code = inputsToActiveCode(inputs);

        commandView(code);
    }

    public KeyCode inputsToActiveCode(List<KeyCode> inputs)
    {
        KeyCode code = KeyCode.T;
        int num = 0;
        foreach (KeyCode input in inputs)
        {
            if (input == KeyCode.U || input == KeyCode.I || input == KeyCode.J || input == KeyCode.K)
            {
                int i = 0;
                switch (input)
                {
                    case KeyCode.U:
                        i = 1;
                        break;
                    case KeyCode.I:
                        i = 2;
                        break;
                    case KeyCode.J:
                        i = 3;
                        break;
                    case KeyCode.K:
                        i = 4;
                        break;
                }
                num = num * 10 + i;
            }
        }
        switch (num)
        {
            case 1:
                code = KeyCode.U;
                break;
            case 2:
                code = KeyCode.I;
                break;
            case 3:
                code = KeyCode.J;
                break;
            case 4:
                code = KeyCode.K;
                break;
            case 12:
            case 21:
                code = KeyCode.A;
                break;
            case 13:
            case 31:
                code = KeyCode.S;
                break;
            case 14:
            case 41:
                code = KeyCode.D;
                break;
            case 23:
            case 32:
                code = KeyCode.C;
                break;
            case 24:
            case 42:
                code = KeyCode.X;
                break;
            case 34:
            case 43:
                code = KeyCode.Z;
                break;
            case 123:
            case 132:
            case 213:
            case 231:
            case 312:
            case 321:
                code = KeyCode.Alpha4;
                break;
            case 124:
            case 142:
            case 214:
            case 241:
            case 412:
            case 421:
                code = KeyCode.Alpha3;
                break;
            case 134:
            case 143:
            case 314:
            case 341:
            case 413:
            case 431:
                code = KeyCode.Alpha2;
                break;
            case 234:
            case 243:
            case 324:
            case 342:
            case 423:
            case 432:
                code = KeyCode.Alpha1;
                break;
            case 1234:
            case 1243:
            case 1324:
            case 1342:
            case 1423:
            case 1432:

            case 2134:
            case 2143:
            case 2314:
            case 2341:
            case 2413:
            case 2431:

            case 3124:
            case 3142:
            case 3214:
            case 3241:
            case 3412:
            case 3421:

            case 4123:
            case 4132:
            case 4213:
            case 4231:
            case 4312:
            case 4321:
                code = KeyCode.Alpha5;
                break;
        }

        return code;
    }

    public Queue<int> GetCommands()
    {
        return commands;
    }

    public List<KeyCode> GetInputs()
    {
        return inputs;
    }

    public Command GetCommand()
    {
        return command;
    }

    // �α� �ۼ� �����Լ�
    StreamWriter OpenTextFile(string fileName)
    {
        // ������ ����.
        string path = "Assets/Log/" + fileName;

        StreamWriter sw = null;
        if (!File.Exists(path))
        {
            // Create a file to write to.
            sw = new StreamWriter(path);

            sw.WriteLine("## ���÷��� �ۼ��� ���� �α�����");
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

    // �α׸� �����.
    // �Էµ� Ŀ�ǵ�� Ŀ�ǵ� �Է� ������ �ʿ���
    void writeLog(StreamWriter sw, int command, float postCommandTime)
    {
        sw.WriteLine(command + " " + postCommandTime + " " + Time.time);
        // ���Ͽ� ���� Ŀ�ǵ�� ���� Ŀ�ǵ� �ð�, ���� Ŀ�ǵ� �ð��� ����Ѵ�.
        // ���÷��� �ý��ۿ��� �ش� �α׸� �а� �ൿ���� ��ȯ�Ͽ� �����ش�.
    }
    // ������� �α� �ۼ� ����
}
