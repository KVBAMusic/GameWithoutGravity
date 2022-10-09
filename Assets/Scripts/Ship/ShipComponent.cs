using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [HideInInspector] public ShipBrain ship;

    public void Init()
    {
        ship = GetComponent<ShipBrain>();
    }

    public void Reset()
    {
        
    }

}
