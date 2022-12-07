using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class HealthPool : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth = 3000f;
    public float currentHealth;
    [SerializeField] float offset = 1f;

    [SerializeField] GameObject healthBarPrefab = null;
    Slider hb = null;
    [SerializeField] GameObject dmgNumbersPrefab = null;
    public static List<DamageNumbers> currentDmgNumbers = new List<DamageNumbers>();
    [SerializeField] Collider2D hurtbox;

    [SerializeField] bool invincible = false;

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
        var hbgo = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        Billboard billboard = hbgo.GetComponent<Billboard>();
        billboard.followPoint = this.transform;
        billboard.offset = offset;
        hb = hbgo.GetComponentInChildren<Slider>();
        hb.maxValue = maxHealth;
        hb.value = maxHealth;

        //hurtbox = GetComponent<Collider2D>();

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
            CharacterDied.Invoke();
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
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.SetActive(false);
    }
}
