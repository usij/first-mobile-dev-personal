using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Let the camera follow you while looking at the goal.
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    [Tooltip("target")]
    public Transform target;

    [Tooltip("distance camera - target")]
    public Vector3 offset = new Vector3(0,3,-6);

    /// <summary>
    /// Update is called once per frame 
    /// </summary>
    void Update()
    {
        if(target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}
