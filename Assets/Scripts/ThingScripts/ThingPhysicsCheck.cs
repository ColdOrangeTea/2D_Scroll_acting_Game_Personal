using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingPhysicsCheck : MonoBehaviour
{
    #region CHECK PARAMETERS
    [SerializeField] protected Vector2 effected_check_offset; // 測試用
    [SerializeField] protected Transform effected_checkpoint;
    [SerializeField] protected float effected_radius = 1f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #region EDITOR METHODS
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)effected_checkpoint.position + effected_check_offset, effected_radius);
        Gizmos.color = Color.blue;
        // Gizmos.DrawWireCube((Vector2)roof_checkpoint.position + roof_check_offset, roof_checkSize);
        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere((Vector2)punch_checkpoint.position + punch_check_offset, punch_radius);

        //Gizmos.color=Color.white;
        //Gizmos.DrawSphere(_slashPoint.position,_slashRadius);//3Dball WTF!!
        //Gizmos.DrawWireCube(_barrelCheckPoint.position, _pushCheckSize);
        //Gizmos.DrawRay(_groundCheckPoint.position , Vector2.down * slopeRaycastDistance);
    }
    #endregion
}
