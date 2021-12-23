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
    public float dodgeSpeed = 5f;

    [Tooltip("how to move fast automatically that ball")]
    public float rollSpeed = 5f;

    // Start is call before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }
    /// <summary>
    /// FixedUpdate
    /// It is good to put time-based functions.
    /// </summary>
    void FixedUpdate()
    {
        var horizontalSpeed = Input.GetAxis("Horizontal") * dodgeSpeed;
        var verticalSpeed = Input.GetAxis("Vertical") * rollSpeed;
        rb.AddForce(horizontalSpeed, 0, rollSpeed);
        //Vector3 getVel = new Vector3(horizontalSpeed, 0, verticalSpeed);
        //rb.velocity = getVel;
    }
}
