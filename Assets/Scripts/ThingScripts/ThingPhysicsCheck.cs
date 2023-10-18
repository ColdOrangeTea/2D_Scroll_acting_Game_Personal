using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingPhysicsCheck : MonoBehaviour
{
    #region CHECK PARAMETERS
    [SerializeField] protected Vector2 effected_check_offset; // 測試用
    [SerializeField] protected Transform effected_checkpoint;
    [SerializeField] protected float effected_radius = 1f;
    public bool isTouchingPlayer { get; set; }
    #endregion

    #region EDITOR METHODS
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (effected_checkpoint != null)
            Gizmos.DrawWireSphere((Vector2)effected_checkpoint.position + effected_check_offset, effected_radius);

        Gizmos.color = Color.blue;
        // Gizmos.DrawWireCube((Vector2)roof_checkpoint.position + roof_check_offset, roof_checkSize);
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere((Vector2)punch_checkpoint.position + punch_check_offset, punch_radius);

    }
    #endregion
}
