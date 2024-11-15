using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : MonoBehaviour
{
    // Start is called before the first frame update
    public void UpdateLight(string city)
    {
        StartCoroutine(WeatherManager.GetWeatherXML(OnXMLDataLoaded, city));
    }
    public void OnXMLDataLoaded(string data)
    {
        Debug.Log(data);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
