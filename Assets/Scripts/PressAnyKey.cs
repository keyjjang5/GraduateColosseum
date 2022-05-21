using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAnyKey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void childActiveFalse()
    {
        Time.timeScale = 1;
        for(int i = 0;i<transform.childCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            //Destroy(transform.GetChild(i).gameObject, 5f);
        }
    }
}
