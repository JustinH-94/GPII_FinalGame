using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    public GameObject[] gobj;
    public Barracks barrack;
    public RadioStation radioS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                for(int i = 0; i < gobj.Length; i++)
                {
                    if (hitInfo.transform.gameObject == gobj[i])
                    {
                        if(gobj[i].gameObject.name == "Barracks")
                        {
                            barrack.BuildingSelected(true, "Barrack");
                            radioS.BuildingSelected(false, "RadioStation");
                        }
                        else if(gobj[i].gameObject.name == "RadioStation")
                        {
                            radioS.BuildingSelected(true, "RadioStation");
                            barrack.BuildingSelected(false, "Barrack");
                        }
                        else
                        {
                            //barrack.BuildingSelected(false, "");
                        }
                        break;
                    }
                }
            }
        }
    }
}
