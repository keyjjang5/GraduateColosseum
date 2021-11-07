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
        center.y = 0.2f;
        transform.position = center;

        Vector3 temp = player1.position;
        temp.y = 0.2f;
        Quaternion q = Quaternion.LookRotation(temp - center);
        transform.rotation = q;
    }

    // Update is called once per frame
    void Update()
    {
        center = (player1.position + player2.position) / 2;
        center.y = 0.2f;
        transform.position = center;

        Vector3 temp = player1.position;
        temp.y = 0.2f;
        Quaternion q = Quaternion.LookRotation(temp - center);
        transform.rotation = q;

    }
}
