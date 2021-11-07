using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour
{
    PlayerController playerController;
    EnemyController enemyController;
    void Start()
    {
        playerController = GameObject.Find("1pPlayer").GetComponent<PlayerController>();
        enemyController = GameObject.Find("2pPlayer").GetComponent<EnemyController>();
    }


    public class AttackInfo
    {
        public int attackPower; // 이 공격의 공격력.
        public Transform attacker; // 공격자.
        public AttackType attackType;
        public bool counter;

        public AttackInfo(int power, Transform attacker, AttackType type, bool counter)
        {
            attackPower = power;
            this.attacker = attacker;
            attackType = type;
            this.counter = counter;
        }
        public AttackInfo()
        {
            attackPower = 0;
            this.attacker = null;
            attackType = 0;
            this.counter = false;
        }
        public void Init()
        {
            attackPower = 0;
            this.attacker = null;
            attackType = 0;
            this.counter = false;
        }
    }
    public enum AttackType
    {
        upper,
        middle,
        lower
    }

    // 맞았다.
    void OnTriggerEnter(Collider other)
    {
        // 공격 당한 상대의 Damage 메시지를 보낸다.
        //other.SendMessage("Damage", GetAttackInfo());
        //if (other.CompareTag("EnemyHit"))

        // 상대방에게 공격정보 전달
        enemyController.Hit(playerController.attackInfo);
        Debug.Log("TriggerCheck : " + other.name);
    }


    // 공격 판정을 유효로 한다.
    void OnAttack()
    {
        GetComponent<Collider>().enabled = true;
    }


    // 공격 판정을 무효로 한다.
    void OnAttackTermination()
    {
        GetComponent<Collider>().enabled = false;
    }
}
