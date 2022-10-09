using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : ShipComponent
{
    [SerializeField] private float gravityAmount;
    [SerializeField] private float turningSpeed;
    [SerializeField] ThrusterSettings[] thrusters;

    private Vector3 localUp {get {
        Vector3 output = Vector3.zero;
        for (int i = 1; i <= 5; i++)
        {
            output += GetThruster(i).Normal;
        }
        Debug.Log(output.normalized);
        return output.normalized;
    }}

    void Awake()
    {
        Init();
    }

    private Thruster GetThruster(string name) => Array.Find(thrusters, t => t.name == name).thruster;
    private Thruster GetThruster(int index) => thrusters[index].thruster;

    private void Update() {
        GetThruster("Forward").isActive = Input.GetKey(KeyCode.W);
        float horizontal = Input.GetAxis("Horizontal");
        transform.RotateAround(transform.position, transform.up, horizontal * turningSpeed * Time.deltaTime);
        var localVel = transform.InverseTransformDirection(ship.RB.velocity);
        ship.RB.velocity = localVel.x * transform.right + localVel.y * transform.up + localVel.z * transform.forward;
    }

    private void FixedUpdate() {
        ship.RB.AddForce(-localUp * gravityAmount * ship.RB.mass, ForceMode.Force);
    }

}

[Serializable]
public struct ThrusterSettings
{
    public string name;
    public Thruster thruster;
}
