using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example_CSV : MonoBehaviour
{
    public string dataPath;
    public List<Dictionary<string, object>> data;

    // Start is called before the first frame update
    void Start()
    {
        data = CSVReader.Read(dataPath);
        // Debug.Log($"CSV Data Loaded: {data.Count} rows");
        Debug.Log(data[0]["Lv"]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
