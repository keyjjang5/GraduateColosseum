using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

// ���÷��̸� ���� �ϱ� ���� Ŭ����
public class ReplaySystem : MonoBehaviour
{
    Queue<CommandPattern.Action> actions;
    Queue<float> playTimes;

    float gameTime;
    public bool isReplay;

    // �α� �ۼ���
    StreamWriter sw;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTime = 0;
        isReplay = false;

        actions = new Queue<CommandPattern.Action>();
        playTimes = new Queue<float>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            //Debug.Log("alpha5");
            checkReplay();
        }
        if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            //Debug.Log("alpha6");
            // ���÷��� ���� ����
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            sw = OpenTextFile(fileName + ".txt");
            writeLog(sw);
        }
        if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            //Debug.Log("alpha7");
            checkReplay();
            LoadReplay();
        }
    }

    private void FixedUpdate()
    {
        gameTime += Time.fixedDeltaTime;

        if (isReplay)
            replay();
    }

    private void OnDestroy()
    {
        CloseTextFile(sw);
    }

    public void Record(CommandPattern.Action action)
    {
        actions.Enqueue(action);
        playTimes.Enqueue(gameTime);
    }

    public void PlayReplay()
    {
        checkReplay();
        LoadReplay();
    }

    // ���÷��� ����
    void checkReplay()
    {
        isReplay = true;
        gameTime = 0;
        FindObjectOfType<FightManager>().GameReset();
    }

    // ���� ���÷��� �Լ�
    void replay()
    {
        if (actions.Count > 0)
        {
            if (playTimes.Peek() <= gameTime)
            {
                playTimes.Dequeue();
                CommandPattern.Action action = actions.Dequeue();
                action.excute();
                //Debug.Log("action : " + action.attackInfo.attacker);
            }
        }
    }

    // ���÷��� �ϴ� ���� ������ �׼ǵ� ������ �ڵ忡 ���缭 �ٲ������ �۵� �Ұ���
    // �̵��̶� ���� ���� Ŀ�ǵ� ����ȭ ��Ű�� �׼ǵ� ���� Ŀ�ǵ� �������� ���� ��
    // ���÷��� ������ Ȯ���Ѵ�. ����� �����Ѵٸ� ��


    // �α� �ۼ� �����Լ�
    StreamWriter OpenTextFile(string fileName)
    {
        // ������ ����.
        string path = "Assets/Resources/Log/" + "Log.csv";

        StreamWriter sw = null;
        //if (!File.Exists(path))
        //{
            // Create a file to write to.
            sw = new StreamWriter(path);

            //sw.WriteLine("## ���÷��� �ۼ��� ���� �α�����");
            sw.WriteLine("ClassName,AttackerName,ActiveTime");
        //}
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
                //Debug.Log(s);
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
    void writeLog(StreamWriter sw)
    {
        //sw.WriteLine(command + " " + postCommandTime + " " + Time.time);
        // ���Ͽ� ���� Ŀ�ǵ�� ���� Ŀ�ǵ� �ð�, ���� Ŀ�ǵ� �ð��� ����Ѵ�.
        // ���÷��� �ý��ۿ��� �ش� �α׸� �а� �ൿ���� ��ȯ�Ͽ� �����ش�.
        while (actions.Count > 0)
        {
            var action = actions.Dequeue();
            var time = playTimes.Dequeue();
            sw.WriteLine(action + "," + action.attackInfo.attacker.name + "," + time);
        }
    }

    public void SaveReplay()
    {
        StreamWriter sw = OpenTextFile("");
        writeLog(sw);
    }

    // ������� �α� �ۼ� ����

    // string�� �̿��ؼ� Ŭ������ �����Ϸ� ������ ����
    void test()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        Type type = assembly.GetType("CommandPattern.LeftZap");
        //Type.GetType("CommandPattern.LeftZap");
        //Debug.Log("type : " + type);
        object action = Activator.CreateInstance(type);

        if (action == null)
        {
            //Debug.Log("error");
        }

        //Debug.Log("CreateInstance action : " + action);
    }

    // ������ �о actions, playTimes�� ä���ִ´�.
    void LoadReplay()
    {
        actions.Clear();
        playTimes.Clear();

        foreach (var data in DataBase.instance.LogData)
        {
            object obj;
            string cn = null;
            string an = null;
            float t = 0;
            data.TryGetValue("ClassName", out obj);
            if (obj == null)
                return;
            cn = obj as string;
            data.TryGetValue("AttackerName", out obj);
            if (obj == null)
                return;
            an = obj as string;
            data.TryGetValue("ActiveTime", out obj);
            if (obj == null)
                return;
            t = (float)obj;

            cn = cn.Remove(0, 15);
            GameObject gObj = GameObject.Find(an);
            CommandPattern.Action action = GetClass(cn, gObj.GetComponent<PlayerController>());
            //Debug.Log("gobj : " + gObj.name);
            //Debug.Log("action : " + action);
            action.attackInfo.attacker = gObj.transform;

            actions.Enqueue(action);
            playTimes.Enqueue(t);
        }
    }

    CommandPattern.Action GetClass(string name, PlayerController playerController)
    {
        CommandPattern.Action action = null;

        switch(name)
        {
            case "Idle":
                action = new CommandPattern.Idle(playerController);
                break;
            case "ForwardWalk":
                action = new CommandPattern.ForwardWalk(playerController);
                break;
            case "ForwardDash":
                action = new CommandPattern.ForwardDash(playerController);
                break;
            case "BackwardWalk":
                action = new CommandPattern.BackwardWalk(playerController);
                break;
            case "BackwardDash":
                action = new CommandPattern.BackwardDash(playerController);
                break;
            case "Crouch":
                action = new CommandPattern.Crouch(playerController);
                break;
            case "CrouchForwardWalk":
                action = new CommandPattern.CrouchForwardWalk(playerController);
                break;
            case "CrouchBackwardWalk":
                action = new CommandPattern.CrouchBackwardWalk(playerController);
                break;

            case "LeftZap":
                action = new CommandPattern.LeftZap(playerController);
                break;
            case "LeftHook":
                action = new CommandPattern.LeftHook(playerController);
                break;
            case "LeftUppercut":
                action = new CommandPattern.LeftUppercut(playerController);
                break;
            case "LeftBackblow":
                action = new CommandPattern.LeftBackblow(playerController);
                break;
            case "LeftForwardHook":
                action = new CommandPattern.LeftForwardHook(playerController);
                break;
            case "LeftElbowHammer":
                action = new CommandPattern.LeftElbowHammer(playerController);
                break;
            case "LeftQuickHook":
                action = new CommandPattern.LeftQuickHook(playerController);
                break;
            case "LeftBodyblow":
                action = new CommandPattern.LeftBodyblow(playerController);
                break;

            case "RightTwo":
                action = new CommandPattern.RightTwo(playerController);
                break;
            case "RightHook":
                action = new CommandPattern.RightHook(playerController);
                break;
            case "RightUpper":
                action = new CommandPattern.RightUpper(playerController);
                break;
            case "RightStandUpper":
                action = new CommandPattern.RightStandUpper(playerController);
                break;
            case "RightUpperTwo":
                action = new CommandPattern.RightUpperTwo(playerController);
                break;
            case "RightBackRP":
                action = new CommandPattern.RightBackRP(playerController);
                break;

            case "LeftMiddleKick":
                action = new CommandPattern.LeftMiddleKick(playerController);
                break;
            case "LeftDoubleCutKick":
                action = new CommandPattern.LeftDoubleCutKick(playerController);
                break;
            case "LeftLowKick":
                action = new CommandPattern.LeftLowKick(playerController);
                break;
            case "LeftKickUppercut":
                action = new CommandPattern.LeftKickUppercut(playerController);
                break;
            case "LeftMiddleKickTwo":
                action = new CommandPattern.LeftMiddleKickTwo(playerController);
                break;
            case "LeftKneeKick":
                action = new CommandPattern.LeftKneeKick(playerController);
                break;

            case "RightHighKick":
                action = new CommandPattern.RightHighKick(playerController);
                break;
            case "RightLowKick":
                action = new CommandPattern.RightLowKick(playerController);
                break;
            case "RightLowKickCrouchRound":
                action = new CommandPattern.RightLowKickCrouchRound(playerController);
                break;
            case "RightMiddleDuckKick":
                action = new CommandPattern.RightMiddleDuckKick(playerController);
                break;
            case "RightStandKick":
                action = new CommandPattern.RightStandKick(playerController);
                break;
            case "RightMiddleStraightKick":
                action = new CommandPattern.RightMiddleStraightKick(playerController);
                break;
            case "RightLowKickRound":
                action = new CommandPattern.RightLowKickRound(playerController);
                break;
            case "RightHighKickRound":
                action = new CommandPattern.RightHighKickRound(playerController);
                break;

        }

        return action;
    }
}
