using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    public enum State { Prepare, Attack, Defend, Lost}
    public State state;
    public EnemyBarrack eb;
    public GameObject EnemyBarrack, EnemyBase, EnemyRS;
    public static List<GameObject> NumOfUnits = new List<GameObject>();
    public static List<GameObject> NumOfBuildings = new List<GameObject>();
    const int UnitLimit = 20;
    public static bool isBaseAttacked;

    public static bool UnitLimitReached;
    public static bool isAllBuildingDestroyed;
    public static bool isPrepared;
    public static int KillCount;
    bool isBarrackActive;
    bool isDefenceSet;
    float buildTimer = 0f;
    float UnitBuildTime =2f;
    //int BlockCount = 0;
    private void Start()
    {
        NumOfBuildings.Add(EnemyRS);
        NumOfBuildings.Add(EnemyBarrack);
        NumOfBuildings.Add(EnemyBase);
        isBaseAttacked = false;
        UnitLimitReached = false;
        state = State.Prepare;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Prepare:
                SetUpUnits();
                Ready();
                break;
            case State.Attack:
                break;
            case State.Defend:
                break;
            case State.Lost:
                GameOver();
                break;
        }
        AllBuildingDestroyed();
        UnitAtLimit();
    }

    void SetUpUnits()
    {
        buildTimer += Time.deltaTime;
        if (buildTimer > UnitBuildTime && !isDefenceSet)
            SetUpDefenceUnit();
        if (buildTimer > UnitBuildTime && isDefenceSet)
            SetUpOffenseUnit();
    }

    void SetUpDefenceUnit()
    {   
        buildTimer = 0f;
        eb.CreateUnit2();
        if (NumOfUnits.Count >= 2)
        {
            isDefenceSet = true;
            UnitBuildTime = 10f;
        }
    }

    void SetUpOffenseUnit()
    {
        buildTimer = 0f;
        eb.CreateUnit();
    }

    void Ready()
    {
        if (NumOfUnits.Count >= 10)
        {
            isPrepared = true;
            state = State.Attack;
        }
    }

    void UnitAtLimit()
    {
        if (NumOfUnits.Count >= UnitLimit)
            UnitLimitReached = true;
        else
            UnitLimitReached = false;
    }

    void AllBuildingDestroyed()
    {
        if (NumOfBuildings.Count <= 0)
            state = State.Lost;
    }

    void GameOver()
    {
        Debug.Log("GameOver, the player wins");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerUnit")
        {
            isBaseAttacked = true;
        }
    }
}
