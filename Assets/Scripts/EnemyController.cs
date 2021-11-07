using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    bool isHit;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        isHit = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 피격 당함
    public void Hit(AttackArea.AttackInfo attackInfo)
    {
        if (!isHit)
        {
            Debug.Log("Hited");
            animator.SetBool("Hited", true);
            isHit = true;

            GetComponent<Rigidbody>().AddForce(new Vector3(0, 400f, 20f));
        }
    }

    void EndHit()
    {
        isHit = false;
        animator.SetBool("Hited", false);
    }
}
