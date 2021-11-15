using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaManager : MonoBehaviour
{
    Collider[] attackAreaColliders;
    // Start is called before the first frame update
    void Start()
    {
        AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
        attackAreaColliders = new Collider[attackAreas.Length];

        for (int attackAreaCnt = 0; attackAreaCnt < attackAreas.Length; attackAreaCnt++)
        {
            // AttackArea ��ũ��Ʈ�� �߰��� ������Ʈ�� �ö��̴��� �迭�� �����Ѵ�.
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].GetComponent<Collider>();
            attackAreaColliders[attackAreaCnt].enabled = false;  // �ʱ갪�� false�� �Ѵ�.
        }

        // Hit�� �ٷ� �۵�����
        GameObject.Find("2pPlayer").GetComponent<EnemyController>().HitEvent.AddListener(EndAttackHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �ִϸ��̼� �̺�Ʈ�� StartAttackHit�� �ö��̴��� ��ȿ�� �Ѵ�.
    void StartAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = true;
    }

    // �ִϸ��̼� �̺�Ʈ�� EndAttackHit�� �ö��̴��� ��ȿ�� �Ѵ�.
    void EndAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = false;
    }
}
