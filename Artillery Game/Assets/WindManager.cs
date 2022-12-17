using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WindManager : MonoBehaviour
{
    [Header("Listening To: ")]
    [SerializeField] TurnsManagerEventsChannelSO turnsEvents;

    [Header("Broadcasting To: ")]
    [SerializeField] WindManagerEventsChannelSO windEvents;

    [Header("UI: ")]
    [SerializeField] GameObject windUIPrefab;

    AreaEffector2D _effector;
    
    [Header("Arrays: ")]
    [SerializeField] int[] directions = new int[8] { 0, 45, 90, 135, 180, 225, 270, 315};
    [SerializeField] float[] strengths = new float[5] { 0, 0.25f, 0.5f, 0.75f, 1f};

    [Header("Settings: ")]
    [SerializeField] float windStrength = 5f;

    public UnityEvent ChangeWindDirectionEvent;

    void Awake() 
    {
        _effector = GetComponentInChildren<AreaEffector2D>();
    }

    void SpawnUI() 
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float offsetY = canvas.transform.position.y + (0.75f * (canvasRect.sizeDelta.y / 2f));
        Vector3 uiPosition = new Vector3(canvas.transform.position.x, offsetY, canvas.transform.position.z);
        GameObject UIinstance = Instantiate(windUIPrefab, uiPosition, Quaternion.identity);
        UIinstance.transform.SetParent(canvas.transform);
    }

    void OnEnable() 
    {
        SpawnUI();

        // Randomize at start
        turnsEvents.SetupFinishEvent.OnEventRaised += ChangeWindDirection;
        turnsEvents.SetupFinishEvent.OnEventRaised += ChangeWindStrength;

        //Randomize at each new cycle
        turnsEvents.NewCycle.OnEventRaised += ChangeWindDirection;
        turnsEvents.NewCycle.OnEventRaised += ChangeWindStrength;
    }

    void OnDisable() 
    {
        // Randomize at start
        turnsEvents.SetupFinishEvent.OnEventRaised -= ChangeWindDirection;
        turnsEvents.SetupFinishEvent.OnEventRaised -= ChangeWindStrength;

        //Randomize at each new cycle
        turnsEvents.NewCycle.OnEventRaised -= ChangeWindDirection;
        turnsEvents.NewCycle.OnEventRaised -= ChangeWindStrength;
    }

    public void ChangeWindDirection()
    {
        int index = Random.Range(0, directions.Length);
        _effector.forceAngle = directions[index];

        windEvents.WindDirectionChange.RaiseEvent(directions[index]);
    }

    public void ChangeWindStrength()
    {
        int index = Random.Range(0, strengths.Length);
        _effector.forceMagnitude = windStrength * strengths[index];

        windEvents.WindStrengthChange.RaiseEvent(index);
    }
}
