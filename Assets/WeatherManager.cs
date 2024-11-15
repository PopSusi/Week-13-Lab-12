using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WeatherManager
{

    private const string xmlApifirsthalf = "http://api.openweathermap.org/data/2.5/weather?q=";
    private const string xmlApisechalf = "&appid=a202d7af555b33476ce91905e43dd07c";

    private static IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public static IEnumerator GetWeatherXML(Action<string> callback, string city)
    {
        string xmlApi = xmlApifirsthalf + city + xmlApisechalf; 
        return CallAPI(xmlApi, callback);
    }
}