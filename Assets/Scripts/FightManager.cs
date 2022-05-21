using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    PlayerController player;
    PlayerController enemy;
    AttackAreaManager playerAttackAreaManager;
    AttackAreaManager enemyAttackAreaManager;
    Status playerStatus;
    Status enemyStatus;

    Slider playerSlider;
    Slider enemySlider;

    Vector3 startPoint1p;
    Vector3 startPoint2p;

    GameObject Win1p;
    GameObject Win2p;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("1pPlayer").GetComponent<PlayerController>();
        enemy = GameObject.Find("2pPlayer").GetComponent<PlayerController>();
        playerAttackAreaManager = FindObjectOfType<AttackAreaManager>();

        playerStatus = GameObject.Find("1pPlayer").GetComponent<Status>();
        enemyStatus = GameObject.Find("2pPlayer").GetComponent<Status>();

        playerSlider = GameObject.Find("1pHPBar").GetComponent<Slider>();
        enemySlider = GameObject.Find("2pHPBar").GetComponent<Slider>();

        startPoint1p = new Vector3(14f, 0.25f, 12f);
        startPoint2p = new Vector3(14f, 0.25f, 16f);

        Win1p = GameObject.Find("WinPanel").transform.GetChild(0).gameObject;
        Win2p = GameObject.Find("WinPanel").transform.GetChild(1).gameObject;

        GameObject.Find("1pPlayer").GetComponent<PlayerController>().HitEvent.AddListener(GameEnd);
        GameObject.Find("2pPlayer").GetComponent<PlayerController>().HitEvent.AddListener(GameEnd);

        GameReset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ���X
        //public void ReceiveCall(string s)
        //{
        //    // ���º�ȭ �˸� > UI����
        //    if (s == "UIUpdate")
            
        //}

    void UIUpdate()
    {
        playerSlider.value = (float)playerStatus.Hp / (float)playerStatus.MaxHp;
        enemySlider.value = (float)enemyStatus.Hp / (float)enemyStatus.MaxHp;
    }

    public void GameReset()
    {
        Time.timeScale = 1;

        player.gameObject.transform.position = startPoint1p;
        enemy.gameObject.transform.position = startPoint2p;
        playerStatus.Init();
        enemyStatus.Init();
        UIUpdate();

        Win1p.SetActive(false);
        Win2p.SetActive(false);
    }

    public void GameEnd()
    {
        if (playerStatus.Hp <= 0)
            //2p win
            Win(2);
        else if (enemyStatus.Hp <= 0)
            //1p win
            Win(1);
        else
            return;
        // �ΰ��� �ð� ����
        Time.timeScale = 0;
        // ������ȹ, �ڵ����� - ���÷��̷� �����ֱ�
        // �����Ȳ���� �ȵǴ� ���� �߻�, �ذ� ���� > ��� ���÷��̸� �����ִ� ������ ����
        //FindObjectOfType<ReplaySystem>().SaveReplay();
        
        // �� �� �ϳ� �����ְ� �ڵ����� ���÷��� ���� �ǵ�����
    }

    void Win(int i)
    {
        if (i == 1)
            Win1p.SetActive(true);
        else if (i == 2)
            Win2p.SetActive(true);
    }
}
