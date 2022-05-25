using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthPool : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 3000f;
    public float currentHealth;
    [SerializeField] float offset = 1.3f;

    [SerializeField] GameObject healthBarGO;
    Slider hb;

    public UnityEvent CharacterDied;

    private void OnEnable() 
    {
        CharacterDied.AddListener(Death);
    }

    private void OnDisable()
    {
        CharacterDied.RemoveListener(Death);
    }

    private void Start() 
    {  
        var hbgo = Instantiate(healthBarGO, transform.position, Quaternion.identity);
        Billboard billboard = hbgo.GetComponent<Billboard>();
        billboard.followPoint = this.transform;
        billboard.offset = offset;
        hb = hbgo.GetComponentInChildren<Slider>();
        hb.maxValue = maxHealth;
        hb.value = maxHealth;

        currentHealth = maxHealth;  
    }
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.RoundToInt(currentHealth - damage);
        hb.value = currentHealth;

        if(currentHealth <= 0f)
        {
            CharacterDied.Invoke();
        }
    }

    void Death()
    {
        gameObject.SetActive(false);
    }
}
