using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour,IInteractable
{
    public SpriteRenderer spriteRenderer;
    public Sprite openChest;
    public Sprite closeChest;
    public bool isDone;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openChest : closeChest;
    }
    public void TriggerAction()
    {
        Debug.Log("Chesk Open");
        if (!isDone)
        {
            OpenChest();
        }
    }
    public void OpenChest()
    {
        GetComponent<AudioDefination>().PlayAudioClip();
        spriteRenderer.sprite = openChest;
        isDone = true;
        BoxCollider2D boxCollider = this.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }
  
}
