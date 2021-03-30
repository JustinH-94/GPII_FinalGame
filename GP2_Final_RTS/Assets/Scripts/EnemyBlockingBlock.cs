using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlockingBlock : Unit
{
    public Transform[] defenseLoc;
    int random;
    // Start is called before the first frame update
    protected override void Start()
    {
        random = Random.Range(0, defenseLoc.Length);
        name = "E.BlockingBlock";
        damageType = "Melee";
        moveSpeed = 2.0f;
        attackRange = 5.0f;
        rotSpeed = 2.0f;
        health = 150;
        attackSpeed = 4f;
        attackDamage = 30;
        buildTime = 8.0f;
        this.transform.position = spawnLocation.transform.position;
        base.Start();
    }
    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * fovDist, Color.red, 1, true);
        SearchForEnemy();
        switch (state)
        {
            case State.spawn:
                MoveToWaitArea();
                break;
            case State.attack:
                ChargeAtEnemy();
                break;
            case State.defend:
                ReturnToBase();
                break;
        }
        EnemyAtBase();
        HealthDetector();
    }

    protected override void MoveToWaitArea()
    {
        agent.SetDestination(defenseLoc[random].position);
        if (Vector3.Distance(this.transform.position, waitLocation.transform.position) < 5.0f)
        {
            direction = enemyBaseLoc.transform.position - this.transform.position;
            Quaternion lookAt = Quaternion.LookRotation(direction);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAt, Time.deltaTime * this.rotSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "E.BlockingBlock")
            agent.isStopped = true;
    }

    void EnemyAtBase()
    {
        if (EnemySystem.isBaseAttacked)
        {
            state = State.defend;
        }
    }

    protected override void ReturnToBase()
    {
        direction = spawnLocation.transform.position - this.transform.position;
        agent.SetDestination(spawnLocation.transform.position);

        if (direction.magnitude < 4.0f )
        {
            RandWalk();
        }
    }

    protected override void SeeEnemy()
    {
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit) && direction.magnitude < fovDist)
        {
            if ((hit.collider.gameObject.tag == "PlayerUnit" || hit.collider.gameObject.tag == "PlayerBase"))
            {
                Debug.Log(hit.collider.gameObject.tag.ToString());
                currentTarget = hit.collider.gameObject;
                state = State.attack;
            }
        }
    }
}
