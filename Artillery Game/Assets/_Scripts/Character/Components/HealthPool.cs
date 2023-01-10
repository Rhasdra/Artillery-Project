using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HealthPool : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] float maxHealth = 3000f;
    public float currentHealth;
    [SerializeField] float offset = 1f;

    [SerializeField] CharManager owner;

    [SerializeField] GameObject healthBarPrefab = null;
    [SerializeField] GameObject healthBarInstance = null;
    Slider hb = null;
    [SerializeField] GameObject dmgNumbersPrefab = null;
    public static List<DamageNumbers> currentDmgNumbers = new List<DamageNumbers>();
    [SerializeField] Collider2D hurtbox;

    [SerializeField] bool invincible = false;

    public UnityEvent NoHealthLeft;

    void Awake() 
    {
        owner = GetComponentInParent<CharManager>();
    }

    private void Start() 
    {  
        healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        Billboard billboard = healthBarInstance.GetComponent<Billboard>();
        billboard.followPoint = this.transform;
        billboard.offset = offset;
        hb = healthBarInstance.GetComponentInChildren<Slider>();
        hb.maxValue = maxHealth;
        hb.value = maxHealth;

        var healthBarScript = healthBarInstance.GetComponent<HealthBar>();
        if(owner.team != null)
            healthBarScript.fill.color = owner.team.color;
        healthBarScript.owner = owner;

        healthBarInstance.SetActive(true);

        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage, Vector3 pos)
    {
        SpawnDamageNumbers(damage, pos);

        currentHealth = Mathf.RoundToInt(currentHealth - damage);

        if(invincible)
        currentHealth = maxHealth;

        hb.value = currentHealth;

        if(currentHealth <= 0f)
        {
            Death();
        }
    }

    void SpawnDamageNumbers(float damage, Vector3 pos)
    {
        DamageNumbers closestDmgNumber = null;
        float closestDistance = 100;

        if (currentDmgNumbers.Count == 0)
        {
            DamageNumbers newDmgNumber = Instantiate(dmgNumbersPrefab, pos, Quaternion.identity).GetComponent<DamageNumbers>();
            currentDmgNumbers.Add(newDmgNumber);
            closestDmgNumber = newDmgNumber;
            newDmgNumber.UpdateDamageNumber(damage);
        
        }else{
            float distance;

            foreach (var dmgNumber in currentDmgNumbers)
            {
                distance = Vector2.Distance(dmgNumber.transform.position, transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDmgNumber = dmgNumber;
                }
            }

            if(closestDistance > 1.5f)
            {
                DamageNumbers newDmgNumber = Instantiate(dmgNumbersPrefab, pos, Quaternion.identity).GetComponent<DamageNumbers>();
                currentDmgNumbers.Add(newDmgNumber);
                newDmgNumber.UpdateDamageNumber(damage);
            }else{
                closestDmgNumber?.UpdateDamageNumber(damage);
            }
        }
    }

    public void Intangible()
    {
        StartCoroutine(IntangiblePeriod());
    }

    IEnumerator IntangiblePeriod()
    {
        hurtbox.enabled = false;
        yield return new WaitForSeconds(0.1f);
        hurtbox.enabled = true;
    }

    void Death()
    {
        healthBarInstance.SetActive(false);
        NoHealthLeft.Invoke();
    }
}
