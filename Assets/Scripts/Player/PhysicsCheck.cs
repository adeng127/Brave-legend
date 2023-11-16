using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private PlayerController playerController;
    [Header("¼ì²â²ÎÊý")]
    public Vector2 bottomOffset;
    
    public float chaeckRaduis;
    public LayerMask groudLayer;
    [Header("×´Ì¬")]
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
        
        //¼ì²âµØÃæ
       isGround=Physics2D.OverlapCircle((Vector2)transform.position+bottomOffset,chaeckRaduis,groudLayer);

    }
    private void OnDrawGizmosSelected() //Ñ¡ÔñÊ±»æÖÆ¼ì²â·¶Î§
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset,chaeckRaduis);
    }
}
