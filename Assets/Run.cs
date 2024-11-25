using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Globalization;
using UnityEngine;
using System;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class Run : MonoBehaviour
{
    //Weather
    private DateTime timeCurrent = new DateTime();
    private float secondsNow;
    private XmlDocument doc;
    private string LightMapAccessor;

    //Images
    private int index;
    private Texture2D[] tex2D = { null, null, null };
    [SerializeField] private GameObject billboard;
    private readonly string[] texWWW = {"https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Cat_August_2010-4.jpg/2560px-Cat_August_2010-4.jpg",
                                        "https://upload.wikimedia.org/wikipedia/commons/c/cf/Curious_Raccoon.jpg",
                                        "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Mam%C3%B5es.jpg/1280px-Mam%C3%B5es.jpg"};

    private void Start()
    {
        timeCurrent = DateTime.Now;
    }

    //Weather
    public void UpdateCity(string city)
    {
        LightMapAccessor = "";
        StartCoroutine(WeatherManager.GetWeatherXML(OnXMLDataLoaded, city));
    }
    public void OnXMLDataLoaded(XmlDocument data)
    {
        Debug.Log("Loading");
        doc = data;
        UpdateWeather();
        UpdateTime();
        LoadMap();
    }

    private void UpdateWeather()
    {
        if (true)
        {
            LightMapAccessor += "Cloudy";
        }
    }

    private void UpdateTime()
    {
        Debug.Log("updating");
        DateTime now = DateTime.UtcNow;
        Debug.Log(now.ToString());

        XmlNode node = doc.SelectSingleNode("//city/timezone");
        int secondUpdate = Convert.ToInt32(node.InnerText);
        if (now.AddSeconds(secondUpdate).Hour >= 19)
        {
            LightMapAccessor += "Night";
        }
        else
        {
            LightMapAccessor += "Day";
        }
    }

    private void LoadMap()
    {
        Debug.Log(LightMapAccessor);
        RenderSettings.skybox = Resources.Load<Material>("SkyMaps/" + LightMapAccessor);
    }

    //Images


    public void BillboardButton()
    {
        GetWebImage(SetImage);
        Debug.Log("Calling");
    }

    public void SetImage(Texture2D tex)
    {
        billboard.GetComponent<Renderer>().material.SetTexture(tex.name, tex);
        Debug.Log("trying to set");
    }
    public void GetWebImage(Action<Texture2D> callback)
    {
        index++;
        index %= texWWW.Length;
        if (tex2D[index] != null)
        {
            Debug.Log(tex2D[index].name);
            callback(tex2D[index]);
        } else
        {
            Debug.Log(texWWW[index]);
            DownloadImage(SetImage, texWWW[index]);
        }
    }
    public IEnumerator DownloadImage(Action<Texture2D> callback, string www)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(www);
        Debug.Log("Request object made");
        yield return request.SendWebRequest();
        Debug.Log("Request returned");
        tex2D[index] = DownloadHandlerTexture.GetContent(request);
        callback(tex2D[index]);
    }
}
