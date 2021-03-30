using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    protected enum State
    {
        spawn, wait, search, attack, retreat, defend, attackUnit, attackBuilding
    }

    protected Vector3 direction, baseDirection;
    protected NavMeshAgent agent;
    protected float fovDist = 50.0f;
    protected float attackRange;
    protected float attackTimer;
    protected float attackSpeed;
    protected float fovAngle = 90.0f;
    protected float timer;
    protected float waittimer;
    protected float sitAtPos = 2.0f;
    protected float range = 10.0f;
    protected Rigidbody rb;
    protected State state;
    protected string unitName;
    protected string damageType;
    protected float buildTime;
    protected int health;
    protected int currentHealth;
    protected int attackDamage;
    protected int counter;
    public GameObject currentTarget;
    protected Vector3 enemyPosition;
    protected bool isReadyToAttack;
    protected RaycastHit hit;
    bool isAttacking;

    public float moveSpeed;
    public float rotSpeed;
    public GameObject spawnLocation;
    public GameObject waitLocation;
    public GameObject enemyBaseLoc;
    public LayerMask layerMask;
    public List<GameObject> EnemyUnits;
    public List<GameObject> EnemyBuildings;
    public bool isAttacked;
    public GameObject attackingUnit;
    protected virtual void Start()
    {
        counter = EnemyBuildings.Count - 1;
        isAttacking = false;
        state = State.spawn;
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        GetComponent<TakeDamage>().SetHealth(health);
        agent.baseOffset = .6f;
        Invoke("EnableNavMeshAgent", 0.025f);
    }

    protected void CheckForNullGO()
    {
        if(EnemyUnits.Count > 0)
        {
            if(EnemyUnits[0] == null)
            {
                EnemyUnits.RemoveAt(0);
            }
        }
    }

    protected void EnableNavMeshAgent()
    {
        agent.enabled = true;
    }

    protected virtual void MoveToWaitArea()
    {
        agent.SetDestination(waitLocation.transform.position);

        if (Vector3.Distance(this.transform.position, waitLocation.transform.position) < 5.0f)
            state = State.wait;
    }

    protected virtual void StartSearch()
    {
        if(PlayerInfo.numOfUnits.Count >=2)
        {
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
            state = State.search;
        }
    }

    protected void HeadToEnemyBase()
    {
        baseDirection = enemyBaseLoc.transform.position - this.transform.position;

        if (baseDirection.magnitude < 10f)
            state = State.attackBuilding;
        else
        {
            agent.SetDestination(enemyBaseLoc.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(enemyBaseLoc.transform.position), Time.deltaTime * this.rotSpeed);
        }
    }

    protected void RandWalk()
    {
        timer += Time.deltaTime;

        if (timer >= sitAtPos)
        {
            Vector3 newPos = RandomNavSphere(transform.position, range, -1); //sets new position for the agent to go to from within the nav mesh
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    protected Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * dist;

        randDir += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, dist, layermask);

        return navHit.position;
    }

    protected virtual void SeeEnemy()
    {
        if (Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, fovDist, ~layerMask))
        {
            if ((hit.collider.gameObject.tag == "EnemyUnit"))
            {
                currentTarget = hit.collider.gameObject;
                state = State.attackUnit;
            }
        }
    }

    protected void SearchForEnemy()
    {
        SeeEnemy();
    }

    protected virtual void ChargeAtEnemy()
    {
        if (currentTarget == null)
        {
            state = State.search;
        }
        else
        {
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = true;
            this.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
            direction = currentTarget.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.position);
            if (direction.magnitude < attackRange)
            {
                AttackEnemy();
            }
            else
            {
                this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(enemyBaseLoc.transform.position), Time.deltaTime * this.rotSpeed);
            }
        }
    }

    protected virtual void NoDetection()
    {
        if (!Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out hit, fovDist, ~layerMask))
        {
            state = State.search;
        }
    }

    protected virtual void AttackBuilding()
    {
        direction = currentTarget.transform.position - this.transform.position;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * this.rotSpeed);
        if (direction.magnitude < attackRange)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackSpeed)
            {
                currentTarget.GetComponent<TakeDamage>().DamageTaken(attackDamage, this.gameObject);
                attackTimer = 0;
            }
        }   
        else
        {
            this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
        }
    }

    protected virtual void AimBuilding()
    {
        for(int i = 0; i < EnemyBuildings.Count; i++)
        {
            currentTarget = EnemyBuildings[i];
            AttackBuilding();
        }
    }

    protected virtual void DefendBase()
    {
        if(PlayerInfo.EnemiesInVicinity[0].Equals(null))
        {
            PlayerInfo.EnemiesInVicinity.RemoveAt(0);
        }
        Vector3 TargetPosition = PlayerInfo.EnemiesInVicinity[0].transform.position - this.transform.position;
        if(TargetPosition.magnitude < attackRange)
        {
            PlayerInfo.EnemiesInVicinity[0].GetComponent<TakeDamage>().DamageTaken(attackDamage, this.gameObject);
        }
        else
        {
            this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(TargetPosition), Time.deltaTime * this.rotSpeed);
            waittimer = 0;
        }
    }

    protected virtual void ReturnToBase()
    {
        direction = waitLocation.transform.position - this.transform.position;
        

        if (direction.magnitude < 10.0f)
        {
            waittimer += Time.deltaTime;
            if(waittimer >= 5f)
            {
                state = State.search;
            }
            DefendBase();
        }
        else
        {
            agent.SetDestination(waitLocation.transform.position);
        }
    }

    protected virtual void AttackEnemy()
    {
            attackTimer += Time.deltaTime;
            if(attackTimer > attackSpeed)
            {
                currentTarget.GetComponent<TakeDamage>().DamageTaken(attackDamage, this.gameObject);
                attackTimer = 0;
            }
    }



    protected void HealthDetector()
    {
        if(health <= 0)
            Destroy(this.gameObject);
    }

    protected void BaseAttacked()
    {
        if (PlayerInfo.isBaseAttacked)
        {
            state = State.defend;
        }
    }

    protected void GettingAttacked()
    {
        if (isAttacked)
        {
            ReturnFire();
        }
    }

    protected void ReturnFire()
    {
        currentTarget = attackingUnit;
        Vector3 target = currentTarget.transform.position - this.transform.position;
        if(target.magnitude < attackRange)
        {
            currentTarget.GetComponent<TakeDamage>().DamageTaken(attackDamage, this.gameObject);
        }
        else
        {
            this.transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(target), Time.deltaTime * this.rotSpeed);
        }
        if (currentTarget == null)
            state = State.search;
    }
}
