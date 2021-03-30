using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    Unit unit;
    public static int unitNum = 0;
    public GameObject barrack, radiostation, Base;
    public static List<GameObject> numOfUnits = new List<GameObject>();
    public static List<GameObject> NumOfBuildings = new List<GameObject>();
    public static List<GameObject> EnemiesInVicinity = new List<GameObject>();
    const int UnitLimit = 20;
    public static bool isBaseAttacked;
    public static bool atUnitLimit;
    public static bool isAllBuildingDestroyed;
    public static int KillCount;
    // Start is called before the first frame update
    void Start()
    {
        NumOfBuildings.Add(barrack);
        NumOfBuildings.Add(radiostation);
        NumOfBuildings.Add(Base);
    }

    // Update is called once per frame
    void Update()
    {
        UnitOnFloor();
        AllBuildingDestroyed();
        AllEnemiesDestroyed();
        GameOver();
        KillCount = 0;
    }

    void AllEnemiesDestroyed()
    {
        if (EnemiesInVicinity.Count <= 0)
            isBaseAttacked = false;
    }

    void UnitOnFloor()
    {
        if(numOfUnits.Count >= UnitLimit)
        {
            atUnitLimit = true;
        }
        else
        {
            atUnitLimit = false;
        }
    }

    void AllBuildingDestroyed()
    {
        if (NumOfBuildings.Count <= 0)
            isAllBuildingDestroyed = true;
        else
            isAllBuildingDestroyed = false;
    }

    void GameOver()
    {
        if (isAllBuildingDestroyed)
            Debug.Log("GameOver, the Enemy wins");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyUnit")
        {
            isBaseAttacked = true;
            EnemiesInVicinity.Add(other.gameObject);
        }
    }
}
