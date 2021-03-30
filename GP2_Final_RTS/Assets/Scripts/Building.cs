using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Building : MonoBehaviour
{
    protected enum State { Active, Destroyed }

    protected State state;
    public GameObject panel;
    public GameObject unitObject1;
    public GameObject unitObject2;
    public Text[] text;
    protected string[] unitNames;
    protected string Name;
    public int Health;
    protected bool isDestroyed;
    protected bool isBarrackSelected;
    protected bool isRadioSelected;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isDestroyed = false;
        state = State.Active;
        GetComponent<TakeDamage>().SetHealth(Health);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected bool BuildingDestroyed()
    {
        if (Health <= 0)
            return true;
        return false;
    }

    protected void BuildingActive()
    {
        if (BuildingDestroyed())
            state = State.Destroyed;
    }

    protected void DestroyBuilding()
    {
        Destroy(this.gameObject);
    }

    public virtual void CreateUnit()
    {
        if (!PlayerInfo.atUnitLimit)
        {
            GameObject a = Instantiate(unitObject1) as GameObject;
            a.transform.position = GameObject.Find("SpawnLocation Barrack").transform.position;
            PlayerInfo.numOfUnits.Add(a);
        }
        else
        {
            Debug.Log("At Max Unit Capacity");
        }
    }

    public virtual void CreateUnit2()
    {
        if (!PlayerInfo.atUnitLimit)
        {
            GameObject a = Instantiate(unitObject2) as GameObject;
            a.transform.position = GameObject.Find("SpawnLocation Barrack").transform.position;
            PlayerInfo.numOfUnits.Add(a);
        }
        else
        {
            Debug.Log("At Max Unit Capacity");
        }
    }

    public void UnitOnButton()
    {
        if (isBarrackSelected)
        {
            for (int i = 0; i < unitNames.Length; i++)
            {
                text[i].text = unitNames[i];
            }
        }
    }

    public void BuildingSelected(bool isSelected, string building)
    {
        if (isSelected)
        {
            panel.SetActive(true);
            isBarrackSelected = true;
            UnitOnButton();
        }
        else
        {
            panel.SetActive(false);
            isBarrackSelected = false;
        }
    }


}
