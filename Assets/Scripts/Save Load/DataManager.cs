using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-100)]
public class DataManager : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();

    public Data saveData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        saveData = new Data();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaise += Save;
        loadDataEvent.OnEventRaise += Load;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaise -= Save;
        loadDataEvent.OnEventRaise -= Load;
    }
    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            Load();
        }
    }
    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }

        foreach (var item in saveData.characterPosDict)
        {
            Debug.Log(item.Key + " " + item.Value);
        }
    }
    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}
