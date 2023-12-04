using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSFMPlayerEffect : MonoBehaviour
{
    public HSFMPlayerPhysicsCheck physicsCheck;
    public Animator animator;
    public Transform pivotPoint;
    public Vector2 Punch_R_Pos;
    public Vector2 Punch_L_Pos;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    public void SetThunderBoolTrue()
    {
        animator.SetBool("IsEffectPlaying", true);
        Invoke("SetThunderBoolfalse", 0.1f);
    }
    public void SetThunderBoolfalse()
    {
        animator.SetBool("IsEffectPlaying", false);
    }

    public void SetAttackBoolTrue()
    {
        // bool faceDir = physicsCheck.GetIsFacingRight();
        // if(faceDir)
        // {
        animator.SetBool("IsEffectPlaying", true);
        Invoke("SetAttackBoolfalse", 0.1f);
        // }
        // else
        // {

        // }


    }

    public void SetAttackBoolfalse()
    {
        animator.SetBool("IsEffectPlaying", false);
    }

    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + Punch_R_Pos, 0.2f);
        Gizmos.DrawWireSphere((Vector2)pivotPoint.position + Punch_L_Pos, 0.2f);

    }
    #endregion
}
