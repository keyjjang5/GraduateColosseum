using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    GameObject player;
    GameObject enemy;
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
        player = GameObject.Find("1pPlayer");
        enemy = GameObject.Find("2pPlayer");
        playerAttackAreaManager = GameObject.Find("1pPlayer").GetComponent<AttackAreaManager>();
        enemyAttackAreaManager = GameObject.Find("2pPlayer").GetComponent<AttackAreaManager>();

        playerStatus = GameObject.Find("1pPlayer").GetComponent<Status>();
        enemyStatus = GameObject.Find("2pPlayer").GetComponent<Status>();

        playerSlider = GameObject.Find("1pHPBar").GetComponent<Slider>();
        enemySlider = GameObject.Find("2pHPBar").GetComponent<Slider>();

        startPoint1p = new Vector3(14f, 0.25f, 12f);
        startPoint2p = new Vector3(14f, 0.25f, 16f);

        Win1p = GameObject.Find("WinPanel").transform.GetChild(0).gameObject;
        Win2p = GameObject.Find("WinPanel").transform.GetChild(1).gameObject;

        GameObject.Find("1pPlayer").GetComponent<PlayerController>().HitEvent.AddListener(GameEnd);
        if (GameObject.Find("2pPlayer").tag == "Player")
            GameObject.Find("2pPlayer").GetComponent<PlayerController>().HitEvent.AddListener(GameEnd);
        else if (GameObject.Find("2pPlayer").tag == "Enemy")
            GameObject.Find("2pPlayer").GetComponent<EnemyController_child>().HitEvent.AddListener(GameEnd);

        GameReset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 사용X
        //public void ReceiveCall(string s)
        //{
        //    // 상태변화 알림 > UI갱신
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
        // 인게임 시간 정지
        Time.timeScale = 0;
        // 원래기획, 자동저장 - 리플레이로 보여주기
        // 빌드상황에서 안되는 문제 발생, 해결 못함 > 즉시 리플레이를 보여주는 것으로 변경
        //FindObjectOfType<ReplaySystem>().SaveReplay();
        
        // 둘 중 하나 보여주고 자동으로 리플레이 저장 되도록함
    }

    void Win(int i)
    {
        if (i == 1)
            Win1p.SetActive(true);
        else if (i == 2)
            Win2p.SetActive(true);
    }
}
