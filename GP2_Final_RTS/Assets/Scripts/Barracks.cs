using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Barracks : Building
{

    public Barracks()
    {
        Name = "Barracks";
        Health = 500;
        unitNames = new string[]{"FearDaSphere", "BlockingBlock"};
        state = State.Active;
    }
    protected override void Update()
    {
        switch (state)
        {
            case State.Active:
                BuildingActive();
                break;
            case State.Destroyed:
                DestroyBuilding();
                break;
        }
        UnitOnButton();
    }

    
}
