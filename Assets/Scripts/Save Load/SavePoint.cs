using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour, IInteractable
{
    [Header("广播")]
    public VoidEventSO SaveGameEvene;
    [Header("变量")]
    public SpriteRenderer spriteRenderer;
    public Sprite SaveImage;
    public Sprite LoadImage;
    public GameObject LightObj;
    public bool isDone;

    
    
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? LoadImage : SaveImage;
        LightObj.SetActive(isDone);
    }
    public void TriggerAction()
    {
        Debug.Log("save");
        if (!isDone)
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        GetComponent<AudioDefination>().PlayAudioClip();
        isDone = true;
        spriteRenderer.sprite = LoadImage;
        Collider2D collider = this.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        LightObj.SetActive(true);

        //保存数据
        SaveGameEvene.RaiseEvent();
    }
}
