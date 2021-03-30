using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingBlock : Unit
{
    protected override void Start()
    {
        name = "BlockingBlock";
        damageType = "Melee";
        moveSpeed = 2.0f;
        rotSpeed = 2.0f;
        attackRange = 5.0f;
        health = 150;
        attackSpeed = 4f;
        attackDamage = 40;
        buildTime = 8.0f;
        //this.transform.position = spawnLocation.transform.position;
        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        CheckForNullGO();
        Debug.DrawRay(transform.position, transform.forward * fovDist, Color.blue, 1, true);
        SearchForEnemy();
        switch (state)
        {
            case State.spawn:
                MoveToWaitArea();
                break;
            case State.wait:
                StartSearch();
                break;
            case State.search:
                HeadToEnemyBase();
                break;
            case State.attackUnit:
                NoDetection();
                ChargeAtEnemy();
                break;
            case State.attackBuilding:
                AimBuilding();
                break;
            case State.retreat:
                break;
            case State.defend:
                ReturnToBase();
                break;
        }
        BaseAttacked();
        HealthDetector();
    }

}
