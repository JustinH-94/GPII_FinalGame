using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearDaSphere : Unit
{

    // Start is called before the first frame update
    protected override void Start()
    {
        name = "FearDaSphere";
        damageType = "Melee";
        moveSpeed = 3.0f;
        attackRange = 10.0f;
        rotSpeed = 2.0f;
        attackSpeed = 1;
        health = 100;
        attackDamage = 25;
        buildTime = 5.0f;
        this.transform.position = spawnLocation.transform.position;
        rb = GetComponent<Rigidbody>();
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
        HealthDetector();
        BaseAttacked();
    }
}
