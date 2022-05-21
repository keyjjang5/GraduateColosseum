using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour
{
    PlayerController playerController;
    EnemyController enemyController;
    void Start()
    {
        playerController = transform.root.GetComponent<PlayerController>();
        if(transform.name == "1pPlayer")
            enemyController = GameObject.Find("2pPlayer").GetComponent<EnemyController>();
        else if(transform.name == "2pPlayer")
            enemyController = GameObject.Find("1pPlayer").GetComponent<EnemyController>();
    }


    public class AttackInfo
    {
        public int attackPower; // 이 공격의 공격력.
        public Transform attacker; // 공격자.
        public AttackType attackType;
        public Vector3 force;
        public Vector3 hitPos;

        public AttackInfo(int power, Transform attacker, AttackType type, Vector3 force)
        {
            attackPower = power;
            this.attacker = attacker;
            attackType = type;
            this.force = force;
            hitPos = Vector3.zero;
        }
        public AttackInfo()
        {
            attackPower = 0;
            this.attacker = null;
            attackType = 0;
            this.force = Vector3.zero;
            hitPos = Vector3.zero;
        }
        public void Clear()
        {
            attackPower = 0;
            this.attacker = null;
            attackType = 0;
            this.force = Vector3.zero;
            hitPos = Vector3.zero;
        }
    }
    public class FrameData
    {
        // 액션이 끝날 때까지의 프레임
        public int actionFrame;
        // 공격 판정이 시작될 때의 프레임
        public int startHitFrame;
        // 공격 판정이 끝날 때의 프레임
        public int endHitFrame;
        // 연계 공격을 입력 할 수 있는 프레임
        public int startInputFrame;
        public int endInputFrame;

        public FrameData() { }
        public FrameData(int af, int shf, int ehf, int sif, int eif)
        {
            actionFrame = af;
            startHitFrame = shf;
            endHitFrame = ehf;
            startInputFrame = sif;
            endInputFrame = eif;
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
        playerController.attackInfo.hitPos = other.transform.position;
        //Debug.Log("other = " + other.transform.name);
        //Debug.Log("other = " + other.transform.root.name);
        other.transform.root.SendMessage("Hit", playerController.attackInfo);
        //other.SendMessage("Damage", playerController.attackInfo);
        //if (other.CompareTag("EnemyHit"))

        // 상대방에게 공격정보 전달
        //enemyController.Hit(playerController.attackInfo);
        //Debug.Log("TriggerCheck : " + other.name);
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
