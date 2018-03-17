using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalonConnect : MonoBehaviour 
{    
    public Text logText;
    private static TalonConnect _instance;
    private bool connecting;
    private TalonSDK.TalonRing talonRing;

    void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("More than one TalonConnect instance was found in your scene");
            enabled = false;
            return;
        }
        _instance = this;
        connecting = false;
        talonRing = null;
    }

    void log(string log)
    {
        if (logText != null)
            logText.text = log;
    }

    void Start()
    {
        log("Scanning...");
        StartCoroutine(Scanning());
    }

    IEnumerator Scanning()
    {
        TalonSDK.Talon.Scan();
        yield return new WaitForSeconds(10);
        if (talonRing == null)
        {
            StartCoroutine(Scanning());
        }
    }

    public void OnDiscovered(string deviceId)
    {
        log("OnDiscovered: " + deviceId);
        if(!connecting && talonRing == null)
        {
            connecting = true;
            TalonSDK.Talon.Connect(deviceId);
        }
    }

    public void OnConnected(string deviceId)
    {
        log("OnConnected: " + deviceId);
        talonRing = TalonSDK.Talon.GetConnectedRing(deviceId);
        connecting = false;
    }

    public void OnDisconnected(string deviceId)
    {
        log("OnDisconnected: " + deviceId);
        talonRing = null;
        connecting = false;
        TalonSDK.Talon.Scan(); //retry?
    }

    public static TalonSDK.TalonRing TalonRing
    {
        get 
        {
            return _instance == null ? null : _instance.talonRing;
        }
    }
}
