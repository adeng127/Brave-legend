using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private PlayerController playerController;
    [Header("������")]
    public Vector2 bottomOffset;
    
    public float chaeckRaduis;
    public LayerMask groudLayer;
    [Header("״̬")]
    public bool isGround;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        Check();    
    }
    public void Check()
    {
        
        //������
       isGround=Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset,chaeckRaduis,groudLayer);

    }
    private void OnDrawGizmosSelected() //ѡ��ʱ���Ƽ�ⷶΧ
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset,chaeckRaduis);
    }
}
