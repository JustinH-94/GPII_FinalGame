using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFearDaSphere : Unit
{
    // Start is called before the first frame update
    protected override void Start()
    {
        name = "E.FearDaSphere";
        damageType = "Melee";
        attackRange = 10.0f;
        moveSpeed = 3.0f;
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
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * fovDist, Color.red, 1, true);
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
                ChargeAtEnemy();
                break;
        }
        HealthDetector();
    }

    protected override void SeeEnemy()
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            if (EnemyUnits[i] == null)
            {
                EnemyUnits.RemoveAt(i);
            }
            direction = EnemyUnits[i].transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.position);


            if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit) && direction.magnitude < fovDist && angle < fovAngle)
            {
                if ((hit.collider.gameObject.tag == "PlayerUnit" || hit.collider.gameObject.tag == "PlayerBase") && (hit.collider.gameObject.tag != "EnemyUnit" || hit.collider.gameObject.tag != "EnemyBase"))
                {
                    Debug.Log(hit.collider.gameObject.tag.ToString());
                    currentTarget = hit.collider.gameObject;
                    state = State.attack;
                }
            }
        }
    }

    protected override void StartSearch()
    {   if(EnemySystem.NumOfUnits.Count >= 2)
        {
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
            state = State.search;
        }
    }
}
