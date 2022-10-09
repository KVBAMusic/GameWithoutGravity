using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBrain : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ShipMovement _shipMovement;

    public Rigidbody RB => _rigidbody;
    public ShipMovement Movement => _shipMovement;

    public event EventHandler OnReset;

    public void ResetShip()
    {
        OnReset?.Invoke(this, EventArgs.Empty);
    }

}
