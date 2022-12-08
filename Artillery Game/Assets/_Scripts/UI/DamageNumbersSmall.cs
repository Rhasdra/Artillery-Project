using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class DamageNumbersSmall : DamageNumbers
{
    Rigidbody2D rb = null;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() 
    {
        LaunchUp();
    }

    void FixedUpdate() 
    {
        // GetSmaller();
    }

    public override void UpdateDamageNumber(float damage)
    {
        text.text = damage.ToString();
        Timer = TimerSeconds;
    }

    public void LaunchUp()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(0f, 1f);
        var dir = new Vector2(x, y);

        rb.AddForce(dir * 200f);
    }

    // void GetSmaller()
    // {
    //     transform.localScale -= transform.localScale * Time.deltaTime;
    // }
}
