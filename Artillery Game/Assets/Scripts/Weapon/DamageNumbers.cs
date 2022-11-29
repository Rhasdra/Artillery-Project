using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public float currentDamage = 0f;
    public float timer = 1;
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
    }

    private void Update() 
    {
        TickTimer();
        GoUp();
    }

    virtual public void UpdateDamageNumber(float damage)
    {
        currentDamage += damage;
        text.text = currentDamage.ToString();
        text.color = Color.Lerp(Color.white, Color.red, currentDamage/1000f);
        if (currentDamage > 3000)
            text.color = Color.Lerp(Color.red, Color.yellow, (currentDamage - 1000)/3000f);
        
        float scale = Mathf.Lerp(startingScale * 1f, startingScale * 3, currentDamage/1000);
        transform.localScale = new Vector3(scale, scale, scale);

        timer = 3;

        DamageNumbersSmall miniDmg = Instantiate(dmgSmall, startingPos, Quaternion.identity).GetComponent<DamageNumbersSmall>();
        miniDmg.UpdateDamageNumber(damage);
    }

    void TickTimer()
    {
        timer -= Time.deltaTime;

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
