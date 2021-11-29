using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player1;
    GameObject player2;
    Transform center;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("1pPlayer");
        player2 = GameObject.Find("2pPlayer");
        center = GameObject.Find("CenterPoint").transform;

        transform.position = new Vector3(18, 1.5f, (player1.transform.position.z + player2.transform.position.z) / 2);
        Quaternion q = Quaternion.LookRotation(center.position - transform.position);
        transform.rotation = q;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v1 = player1.transform.position - center.position;
        Vector3 v2 = player2.transform.position - center.position;
        v2.y = 5;
        Vector3 perp = Vector3.Cross(v1, v2);

        transform.position = center.position + perp.normalized * 4 + new Vector3(0, 1.5f, 0);
        float distance = Vector3.Distance(player1.transform.position, player2.transform.position);
        if (distance > 7)
            transform.position += perp.normalized * (distance - 7);
        //Debug.Log("perp : " + perp);
        Vector3 temp = center.position - transform.position;
        temp.y = 0;
        Quaternion q = Quaternion.LookRotation(temp);
        transform.rotation = q;
    }
}
