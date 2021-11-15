using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    int maxHp;
    public int MaxHp { get { return maxHp; } }
    int hp;
    public int Hp { get { return hp; } }
    State currentState;
    public State CurrentState { get { return currentState; } set { currentState = value; } }
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 100;
        hp = maxHp;
        currentState = State.Standing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        hp = maxHp;
        currentState = State.Standing;
    }

    public void Hit(int damage)
    {
        hp -= damage;
    }
}
