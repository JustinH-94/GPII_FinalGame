using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadioStation : Building
{
    public RadioStation()
    {
        Name = "Radio Station";
        Health = 400;
        unitNames = new string[] { "BirdPlane", "CruiserBruiser" };
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
