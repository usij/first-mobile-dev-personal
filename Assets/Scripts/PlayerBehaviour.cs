using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// to process Player moves and input 
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    /// <summary>
    /// refrence to Rigidbody component
    /// </summary>
    private Rigidbody rb;

    [Tooltip("how to move fast left/right that ball")]
    public float dodgeSpeed = 5;

    [Tooltip("how to move fast automatically that ball")]
    public float rollSpeed = 5;

    // Start is call before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// FixedUpdate
    /// It is good to put time-based functions.
    /// </summary>
    void FixedUpdate()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
    }
}
