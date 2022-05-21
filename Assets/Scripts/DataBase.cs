using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public static DataBase instance;


    private List<Dictionary<string, object>> logData = new List<Dictionary<string, object>>();

    public List<Dictionary<string, object>> LogData { get { return logData; } }

    private void Awake()
    {
        instance = this;
        ReadData();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReadData()
    {
        logData.Clear();
        logData = CSVParser.Read("Log/Log");
    }
}
