using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    [Header("Listening To")]
    [SerializeField] InputReader inputReader;
    [SerializeField] MovementEventsChannelSO moveEventsChannel;
    [SerializeField] WeaponEventsChannelSO weaponEventsChannel;
    [SerializeField] ProjectileEventsChannelSO projectileEventsChannel; 
    [SerializeField] TurnsManagerEventsChannelSO turnsManagerEvents; 

    [Header("Infos")]
    [SerializeField] Vector2 mousePos;
    [SerializeField] bool canSwitchTarget = true;
    [SerializeField] bool isDragging = false;
    [SerializeField] Vector3 dragStart = new Vector2();
    [SerializeField] int camIndex = 0;
    
    [Header("Dependancies")]
    [SerializeField] GameObject mainCamPrefab;
    [SerializeField] GameObject vCamPrefab;
    [SerializeField] CinemachineVirtualCamera[] vCams = new CinemachineVirtualCamera[2];
    

    [Header("Settings")]
    [Tooltip("Events that the player doesn't control have a cooldown timer before triggering a camera transition.")]
    [SerializeField] float switchTimerSeconds = 0.5f;
    [SerializeField] float zoomSpeed = 0.05f;
    [SerializeField] float panZone = 0.05f;
    [SerializeField] float panSpeed = 1f;
    [SerializeField] float dragSpeed = 20f;
    [SerializeField] float shakeAmplitude = 3f;
    [SerializeField] float shakeFrequency = 3f;
    [SerializeField] float shakeTime = 0.1f;

    private void Start() 
    {
        GameObject separator = new GameObject();
        separator.name = "---------- CAMERAS ----------";

        GameObject instance = Instantiate(mainCamPrefab, Vector3.zero, Quaternion.identity);
        instance.transform.parent = separator.transform;

        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = new Vector3(0, 0, -10);
            instance = Instantiate(vCamPrefab, pos, Quaternion.identity);
            vCams[i] = instance.GetComponent<CinemachineVirtualCamera>();

            instance.transform.parent = separator.transform;
        }
    }

    private void OnEnable() 
    { 
        //Movement
        moveEventsChannel.MoveStartEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.MoveStartEvent.OnEventRaised += FrameLower;

        moveEventsChannel.LongJumpEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.LongJumpEvent.OnEventRaised += FrameLower;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised += GetNewTarget;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised += FrameLower;

        //Projectiles
        projectileEventsChannel.SpawnEvent.OnEventRaised += GetNewTarget;
        projectileEventsChannel.SpawnEvent.OnEventRaised += FrameCenter;

        //Hits
        projectileEventsChannel.HitEvent.OnEventRaised += GetNewTarget;

        //Turns
        turnsManagerEvents.StartTurn.OnEventRaised += GetNewTarget;
        turnsManagerEvents.StartTurn.OnEventRaised += FrameLower;

        //Zoom
        inputReader.ZoomEvent += Zoom;

        //Pan
        inputReader.MousePositionEvent += GetMousePos;

        //Drag
        inputReader.PanPressEvent += StartDragCamera;
        inputReader.PanCanceledEvent += StopDragCamera;

        //Shake
        projectileEventsChannel.HitEvent.OnEventRaised += ShakeCameraRequest;
    }

    private void OnDisable() 
    {
        //Movement
        moveEventsChannel.MoveStartEvent.OnEventRaised -= GetNewTarget;
        moveEventsChannel.MoveStartEvent.OnEventRaised -= FrameLower;

        moveEventsChannel.LongJumpEvent.OnEventRaised -= GetNewTarget;
        moveEventsChannel.LongJumpEvent.OnEventRaised -= FrameLower;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised -= GetNewTarget;
        moveEventsChannel.BackFlipJumpEvent.OnEventRaised -= FrameLower;

        //Projectiles
        projectileEventsChannel.SpawnEvent.OnEventRaised -= GetNewTarget;
        projectileEventsChannel.SpawnEvent.OnEventRaised -= FrameCenter;

        //Hits
        projectileEventsChannel.HitEvent.OnEventRaised -= GetNewTarget;

        //Turns
        turnsManagerEvents.StartTurn.OnEventRaised -= GetNewTarget;
        turnsManagerEvents.StartTurn.OnEventRaised -= FrameLower;

        //Zoom
        inputReader.ZoomEvent -= Zoom;

        //Pan
        inputReader.MousePositionEvent -= GetMousePos;

        //Drag
        inputReader.PanPressEvent -= StartDragCamera;
        inputReader.PanCanceledEvent -= StopDragCamera;

        //Shake
        projectileEventsChannel.HitEvent.OnEventRaised -= ShakeCameraRequest;
    }

    void Update() 
    {
        PanScreen(mousePos);
        DragCamera(mousePos);   
    }

    void SwitchTarget(Transform target)
    {
        if(target == vCams[camIndex].Follow || isDragging == true)
            return;

        
        Vector3 camPos = new Vector3(target.position.x, target.position.y, vCams[camIndex].transform.position.z);
        // if(Vector2.Distance(target.position, camPos) > 0f) //Only switchs camera if new target is far enough
        // {
            if(camIndex < vCams.Length -1) //adds up the index
            {
                camIndex++;
            }else
            {
                camIndex = 0;
            }

            for (int i = 0; i < vCams.Length; i++) //shifts current index camera to top priority
            {
                if(i != camIndex)
                {
                    vCams[i].Priority = 0;
                }
                else if (i == camIndex)
                {
                    //Reposition camera on top of target. It has to be disabled so it does not interpolate on its own
                    vCams[i].enabled = false;
                    vCams[i].transform.position = camPos;
                    vCams[i].enabled = true;
                    
                    vCams[i].Follow = target;
                    vCams[i].Priority ++;
                }
            }
        // }
        // else
        // {
        //     vCams[index].Follow = target;
        // }
    }

    #region GetNewTarget overloads
    public void GetNewTarget()
    {
        SwitchTarget(turnsManagerEvents.charTakingTurn.transform);
    }

    public void GetNewTarget (Transform target)
    {
        SwitchTarget(target);
    }
    public void GetNewTarget (GameObject target)
    {
        SwitchTarget(target.transform);
    }
    public void GetNewTarget (GameObject target, Vector3 pos, Quaternion rotation)
    {
        SwitchTarget(target.transform);
    }
    public void GetNewTarget (Vector3 pos, int damage, IDamageable damageable)
    {
        if(canSwitchTarget == true)
        {
            StartCoroutine(TickTimer());

            GameObject temp = new GameObject();
            temp.name = "Temporary_Camera_TrackPoint";
            temp.transform.position = pos;
            SwitchTarget(temp.transform);
            FrameCenter();
            Destroy(temp, 3f);
        }
    }
    #endregion

    #region Framing Methods
    void FrameLower(Transform target = null) //Position target at the bottom of screen
    {
        CinemachineFramingTransposer transposer = vCams[camIndex].GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_ScreenY = 0.65f;
    }

    void FrameLower()
    {
        CinemachineFramingTransposer transposer = vCams[camIndex].GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_ScreenY = 0.65f;
    }

    void FrameCenter(GameObject target = null) //Position target at the center of screen
    {
        CinemachineFramingTransposer transposer = vCams[camIndex].GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_ScreenY = 0.5f;
    }

    void FrameCenter()
    {
        CinemachineFramingTransposer transposer = vCams[camIndex].GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_ScreenY = 0.5f;
    }
    #endregion

    IEnumerator TickTimer() //Timer should be only used for events that the player doesn't control
    {
        canSwitchTarget = false;
        yield return new WaitForSeconds(switchTimerSeconds);
        canSwitchTarget = true;
    }


    /////////////// CAMERA CONTROLS ///////////////
    void Zoom(float input)
    {
        float zoom = vCams[camIndex].m_Lens.OrthographicSize;

        if(input < 0f)
        {
            if(zoom <= 4f)
            zoom += zoomSpeed;
        }
        else if (input > 0f)
        {
            if(zoom >= 1f)
            zoom -= zoomSpeed;
        }

        zoom = Mathf.Clamp(zoom, 1f, 4f);
        vCams[camIndex].m_Lens.OrthographicSize = zoom;
    }

    Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;
        if(y >= Screen.height * (1 - panZone))
        {
            direction.y += 1;
        }
        else if (y <= Screen.height * panZone)
        {
            direction.y -= 1;
        }

        if(x >= Screen.width * (1 - panZone))
        {
            direction.x += 1;
        }
        else if(x <= Screen.width * panZone)
        {
            direction.x -= 1;
        }

        return direction;
    }

    void PanScreen(Vector2 mousePos)
    {
        Vector2 direction = PanDirection(mousePos.x, mousePos.y);
        Vector3 desiredPos = vCams[camIndex].transform.position + (Vector3)direction * panSpeed;

        if(direction.x != 0 || direction.y != 0)
        {
            if (vCams[camIndex].Follow != null)
            vCams[camIndex].Follow = null;
        }

        vCams[camIndex].transform.position = Vector3.Lerp(vCams[camIndex].transform.position, desiredPos, Time.deltaTime);    
    }

    void GetMousePos(Vector2 mouseInput)
    {
        // if(MouseScreenCheck(mouseInput))
            mousePos = mouseInput;
        // else
        //     mousePos = new Vector2(Screen.width/2, Screen.height/2);
    }

    bool MouseScreenCheck(Vector2 mouseInput)
    {
        #if UNITY_EDITOR
        if (mouseInput.x == 0 || mouseInput.y == 0 || mouseInput.x >= Handles.GetMainGameViewSize().x - 1 || mouseInput.y >= Handles.GetMainGameViewSize().y - 1)
        {
            return false;
        }
        #else
        if (mouseInput.x == 0 || mouseInput.y == 0 || mouseInput.x >= Screen.width - 1 || mouseInput.y >= Screen.height - 1) 
        {
            return false;
        }
        #endif
        else 
        {
            return true;
        }
    }

    void StartDragCamera()
    {
        if(isDragging == false)
        {
            isDragging = true;
            dragStart = Camera.main.ScreenToWorldPoint(mousePos);
        }
} 
    
    void DragCamera(Vector2 mousePos)
    {
        if (isDragging == false)
        return;

        Vector3 difference = Camera.main.ScreenToWorldPoint(mousePos) - vCams[camIndex].transform.position;
        
        if(vCams[camIndex].Follow != null)
            vCams[camIndex].Follow = null;
        
        vCams[camIndex].transform.position = Vector3.Lerp(vCams[camIndex].transform.position, (dragStart - difference), dragSpeed * Time.deltaTime);
    }

    void StopDragCamera()
    {
        isDragging = false;
    }


    ////////////// CAMERA SHAKE ///////////////
    void ShakeCameraRequest(Vector3 location, int damage, IDamageable victim)
    {
        StartCoroutine(ShakeCamera(shakeAmplitude, shakeFrequency, shakeTime));
    }

    IEnumerator ShakeCamera(float amplitude = 1f, float frequency = 2f, float timeSeconds = 1f)
    {
        foreach (var cam in vCams)
        {
            CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency; 
        }
        
        float timer = 0f;
        while(timer < timeSeconds)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var cam in vCams)
        {
            CinemachineBasicMultiChannelPerlin noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f; 
        }
    }
}
