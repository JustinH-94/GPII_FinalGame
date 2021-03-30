using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPlane : Unit
{
    protected override void Start()
    {
        name = "BirdPlane";
        damageType = "Range";
        attackSpeed = 1.5f;
        attackRange = 10.0f;
        moveSpeed = 8.0f;
        rotSpeed = 5.0f;
        health = 100;
        attackDamage = 5;
        buildTime = 6.0f;
        //this.transform.position = new Vector3(spawnLocation.transform.position.x, 20f, spawnLocation.transform.position.z);
        base.Start();
        agent.baseOffset = 9.7f;
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

    protected override void MoveToWaitArea()
    {
        agent.SetDestination(waitLocation.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.rotSpeed);

        if (Vector3.Distance(this.transform.position, waitLocation.transform.position) < 10.0f)
            state = State.wait;
    }
}
