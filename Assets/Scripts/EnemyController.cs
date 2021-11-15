using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    bool isHit;
    Animator animator;
    Status status;
    public UnityEvent HitEvent;
    GameObject manager;
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        animator = GetComponent<Animator>();
        status = GetComponent<Status>();

        manager = GameObject.Find("Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 피격 당함
    public void Hit(AttackArea.AttackInfo attackInfo)
    {
        if (isHit)
            return;

        Debug.Log("Hited");
        animator.SetBool("Hited", true);
        HitEvent.Invoke();
        status.Hit(attackInfo.attackPower);
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 400f, 20f));

        manager.SendMessage("UIUpdate");
    }

    void EndHit()
    {
        isHit = false;
        animator.SetBool("Hited", false);
    }
}
