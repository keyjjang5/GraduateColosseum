using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Colosseum;

public class Status : MonoBehaviour
{
    int maxHp;
    public int MaxHp { get { return maxHp; } }
    int hp;
    public int Hp { get { return hp; } }
    public State currentState;
    public State CurrentState { get { return currentState; } set { currentState = value; } }

    Guard guard;
    public Guard Guard { get { return guard; } set { guard = value; } }

    public HitState HitState;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = 100;
        hp = maxHp;
        currentState = State.Standing;
        guard = Guard.Stand;
        HitState = HitState.NoHit;
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

    public void Damaged(int damage)
    {
        hp -= damage;
    }
}
