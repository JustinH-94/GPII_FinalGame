using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRadiostation : Building
{
    public EnemyRadiostation()
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
    }

    public override void CreateUnit()
    {
        if (!EnemySystem.UnitLimitReached)
        {
            GameObject a = Instantiate(unitObject1) as GameObject;
            EnemySystem.NumOfUnits.Add(a);
        }
    }

    public override void CreateUnit2()
    {
        if (!EnemySystem.UnitLimitReached)
        {
            GameObject a = Instantiate(unitObject2) as GameObject;
            EnemySystem.NumOfUnits.Add(a);
        }
    }
}
