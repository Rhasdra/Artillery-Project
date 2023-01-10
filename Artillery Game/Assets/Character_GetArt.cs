using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_GetArt : MonoBehaviour
{
    SpriteRenderer sr;
    CharManager charManager;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
        charManager = GetComponentInParent<CharManager>();
    }

    public void Initialize() 
    {
        sr.sprite = charManager.jobInfo.sprite;
        transform.position += new Vector3(charManager.jobInfo.artOffset.x, charManager.jobInfo.artOffset.y, 0);
    }
}
