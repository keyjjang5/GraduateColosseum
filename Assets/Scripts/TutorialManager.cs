using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int level;
    int maxLevel;
    List<GameObject> gameObjects;
    // Start is called before the first frame update
    void Start()
    {
        level = 0;
        maxLevel = GameObject.Find("TutorialPanel").transform.childCount;
        gameObjects = new List<GameObject>();
        while (GameObject.Find("TutorialPanel").transform.childCount > level)
        {
            gameObjects.Add(GameObject.Find("TutorialPanel").transform.GetChild(level).gameObject);
            level++;
        }
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        UIClose(level);
        level++;
        UIOpen(level);
    }

    public void TutorialOpen()
    {
        level = 0;
        UIOpen(level);
    }

    public void TutorialClose()
    {
        UIClose(level);
        level = 0;
    }

    public void UIOpen(int lv)
    {
        gameObjects[lv].SetActive(true);

    }

    public void UIClose(int lv)
    {
        gameObjects[lv].SetActive(false);

    }
}
