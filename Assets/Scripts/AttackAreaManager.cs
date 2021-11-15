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
            // AttackArea 스크립트가 추가된 오브젝트의 컬라이더를 배열에 저장한다.
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].GetComponent<Collider>();
            attackAreaColliders[attackAreaCnt].enabled = false;  // 초깃값은 false로 한다.
        }

        // Hit시 바로 작동정지
        GameObject.Find("2pPlayer").GetComponent<EnemyController>().HitEvent.AddListener(EndAttackHit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 애니메이션 이벤트의 StartAttackHit로 컬라이더를 유효로 한다.
    void StartAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = true;
    }

    // 애니메이션 이벤트의 EndAttackHit로 컬라이더를 무효로 한다.
    void EndAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = false;
    }
}
