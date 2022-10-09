using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : ShipComponent
{
    [SerializeField] private float acceleration;
    [SerializeField] private float gravityAmount;
    [SerializeField] private float turningSpeed;
    [SerializeField] private LayerMask trackMask;

    public Vector3 localUp {get; private set;}

    private float axisH;
    private float axisV;

    void Awake()
    {
        Init();

    }

    private void Start() {
        localUp = transform.up;
        ship.RB.transform.parent = null;
    }

    private void Update() {
        axisH = Input.GetAxis("Horizontal");
        axisV = Input.GetAxis("Vertical");
        transform.position = ship.RB.transform.position;

        // adjust rotation according to normal
        Quaternion newRot = transform.rotation * Quaternion.FromToRotation(transform.up, localUp);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, 17 * Time.deltaTime);

        // turn
        transform.localEulerAngles += new Vector3(0, axisH * turningSpeed * Time.deltaTime);
    }

    private void FixedUpdate() {
        RaycastHit hit;
        if (Physics.Raycast(ship.RB.transform.position, -localUp, out hit, 3f, trackMask))
        {
            localUp = hit.normal;
        }

        //forward force
        ship.RB.AddForce(transform.forward * acceleration * axisV, ForceMode.Force);

        // gravity
        ship.RB.AddForce(-localUp * gravityAmount * ship.RB.mass, ForceMode.Force);

        // correct velocity
        Vector3 localVel = transform.InverseTransformDirection(ship.RB.velocity);
        Vector3 localH = localVel;
        localH.y = 0;
        float speedH = localH.magnitude;
        ship.RB.velocity = speedH * transform.forward + localVel.y * transform.up;
    }

}

