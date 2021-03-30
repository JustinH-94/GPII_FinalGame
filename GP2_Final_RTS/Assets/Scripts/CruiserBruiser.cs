using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiserBruiser : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        name = "CruiserBruiser";
        damageType = "Range";
        moveSpeed = 5.0f;
        rotSpeed = 5.0f;
        attackRange = 15f;
        health = 250;
        attackDamage = 50;
        attackSpeed = 5f;
        buildTime = 6.0f;
        this.transform.position = new Vector3(spawnLocation.transform.position.x, 20f, spawnLocation.transform.position.z);
        base.Start();
        agent.baseOffset = 9.7f;
    }

    // Update is called once per frame
    void Update()
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
