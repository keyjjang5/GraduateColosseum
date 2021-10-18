using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPoint : MonoBehaviour
{
    private Transform player1;
    private Transform player2;
    Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("1pPlayer").transform;
        player2 = GameObject.Find("2pPlayer").transform;

        center = (player1.position + player2.position) / 2;
        transform.position = center;

        Quaternion q = Quaternion.LookRotation(player1.position - center);
        transform.rotation = q;
    }

    // Update is called once per frame
    void Update()
    {
        center = (player1.position + player2.position) / 2;
        transform.position = center;
        
        Quaternion q = Quaternion.LookRotation(player1.position - center);
        transform.rotation = q;
        
    }
}
