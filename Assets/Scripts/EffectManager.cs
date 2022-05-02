using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject HitEffect;
    public GameObject GuardEffect;
    GameObject Camera;
    // Start is called before the first frame update
    void Start()
    {
        HitEffect = Resources.Load("HitEffect") as GameObject;
        GuardEffect = Resources.Load("GuardEffect") as GameObject;
        Camera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHitEffect(Vector3 pos)
    {
        GameObject newEffect = Instantiate(HitEffect);
        newEffect.transform.position = Vector3.Lerp(pos, Camera.transform.position, 0.2f);
        Destroy(newEffect, 1f);
    }

    public void PlayGuardEffect(Vector3 pos)
    {
        GameObject newEffect = Instantiate(GuardEffect);
        newEffect.transform.position = Vector3.Lerp(pos, Camera.transform.position, 0.2f);
        Destroy(newEffect, 1f);
    }
}
