using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public float currentDamage = 0f;
    public float timerSeconds = 1;
    float timer = 1;
    public TextMeshProUGUI text = null;

    [SerializeField] GameObject dmgSmall = null;

    [SerializeField] float offset = 0f;

    float startingScale;
    Vector3 startingPos;

    private void Awake() 
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        startingScale = transform.localScale.x;
        startingPos = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + offset, 0f);
        timer = timerSeconds;
    }

    private void Update() 
    {
        TickTimer();
        GoUp();
    }

    virtual public void UpdateDamageNumber(float damage)
    {
        currentDamage += damage;
        timer = timerSeconds;
        text.text = currentDamage.ToString();
        text.color = Color.white;

        if(currentDamage < 500)
            text.color = Color.Lerp(Color.grey, Color.white, (currentDamage)/500f);
        
        if(currentDamage >= 500)
            text.color = Color.Lerp(Color.white, Color.red, (currentDamage - 500f)/500f);
        
        if (currentDamage > 3000)
            text.color = Color.Lerp(Color.red, Color.yellow, (currentDamage - 1000)/3000f);
        
        float scale = Mathf.Lerp(startingScale * 1f, startingScale * 3, currentDamage/1000);
        transform.localScale = new Vector3(scale, scale, scale);

        DamageNumbersSmall miniDmg = Instantiate(dmgSmall, startingPos, Quaternion.identity).GetComponent<DamageNumbersSmall>();
        miniDmg.UpdateDamageNumber(damage);
    }

    void TickTimer()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);

        if(timer < 0)
        {
            HealthPool.currentDmgNumbers.Remove(this);
            Destroy(this.gameObject);
        }
    }

    void GoUp()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y + (0.2f * Time.deltaTime), transform.position.z);
        //transform.localScale -= transform.localScale * 0.1f * Time.deltaTime;
    }
}
