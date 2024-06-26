using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEventSO newGameEvent;
    [Header("基本属性")]
    public float maxHealth;     //最大血量
    public float currentHealth; //当前血量

    [Header("受伤无敌")]
    public float invalnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;

    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void NewGame()
    {
        currentHealth = maxHealth;

        OnHealthChange?.Invoke(this);
    }

    private void OnEnable()
    {
        newGameEvent.OnEventRaise += NewGame;
        ISaveable saveable= this;
        saveable.RegisterSaveData();
    }

    private void OnDisable()
    {

        newGameEvent.OnEventRaise -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Water"))
        {
            if (currentHealth > 0)
            {
                //死亡，血量更新
                currentHealth = 0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }

        }
    }
    public void TakeDamage(Attack attacker)
    {
        
        if (invulnerable)
        {
            return;
        }
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //播放受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            //死亡
            OnDie.Invoke();
        }

        OnHealthChange?.Invoke(this);
    }


    /// <summary>
    /// 受伤无敌
    /// </summary>
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invalnerableDuration;
        }
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID)){
            data.characterPosDict[GetDataID().ID] = transform.position;
            data.floatSavedData[GetDataID().ID + "health"]= this.currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
            data.floatSavedData.Add(GetDataID().ID + "health", this.currentHealth);
        }
    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID];
            this.currentHealth = data.floatSavedData[GetDataID().ID + "health"];

            OnHealthChange?.Invoke(this);
        }
    }
}
