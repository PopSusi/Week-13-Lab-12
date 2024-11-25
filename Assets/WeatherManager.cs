using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class WeatherManager
{

    private const string xmlApifirsthalf = "http://api.openweathermap.org/data/2.5/weather?mode=xml&q=";
    private const string xmlApisechalf = "&appid=a202d7af555b33476ce91905e43dd07c";

    private static IEnumerator CallAPI(string url, Action<XmlDocument> callback)
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
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(request.downloadHandler.text);
                callback(xml);
            }
        }
    }

    public static IEnumerator GetWeatherXML(Action<XmlDocument> callback, string city)
    {
        string xmlApi = xmlApifirsthalf + city + xmlApisechalf; 
        return CallAPI(xmlApi, callback);
    }
}