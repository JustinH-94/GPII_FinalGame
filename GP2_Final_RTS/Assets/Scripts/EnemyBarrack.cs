using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBarrack : Building
{
    // Start is called before the first frame update
    public EnemyBarrack()
    {
        Name = "Barracks";
        Health = 500;
        unitNames = new string[] { "FearDaSphere", "BlockingBlock" };
        state = State.Active;
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        Debug.Log(GetComponent<TakeDamage>().Health.ToString());
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
