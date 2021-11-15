using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    PlayerController player;
    EnemyController enemy;
    AttackAreaManager playerAttackAreaManager;
    AttackAreaManager enemyAttackAreaManager;
    Status playerStatus;
    Status enemyStatus;

    Slider playerSlider;
    Slider enemySlider;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        enemy = FindObjectOfType<EnemyController>();
        playerAttackAreaManager = FindObjectOfType<AttackAreaManager>();

        playerStatus = GameObject.Find("1pPlayer").GetComponent<Status>();
        enemyStatus = GameObject.Find("2pPlayer").GetComponent<Status>();

        playerSlider = GameObject.Find("1pHPBar").GetComponent<Slider>();
        enemySlider = GameObject.Find("2pHPBar").GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveCall(string s)
    {
        // 상태변화 알림 > UI갱신
        if (s == "UIUpdate") ;
            
    }

    void UIUpdate()
    {
        playerSlider.value = (float)playerStatus.Hp / (float)playerStatus.MaxHp;
        enemySlider.value = (float)enemyStatus.Hp / (float)enemyStatus.MaxHp;
    }
}
