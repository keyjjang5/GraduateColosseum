using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Animator animator;
    int frameCounter = 0;
    bool nowAction = false;
    // float timeCheck = 0f;
    float stiffTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("1pPlayer").GetComponent<Animator>();
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B) && !nowAction)
        {
            animator.PlayInFixedTime("Base Layer.KB_p_Jab_L_1", 0, Time.fixedDeltaTime * 0);
            frameCounter = 10;
            stiffTime = 10f;
            nowAction = true;
        }

        if (frameCounter == 0 && nowAction)
        {
            animator.PlayInFixedTime("Base Layer.Standing");
        }

        if (stiffTime <= 0)
            nowAction = false;

        if (frameCounter <= 0)
        {
            stiffTime -= 1;
            //Debug.Log(frameCounter);
        }
        
        frameCounter -= 1;

    }
}
