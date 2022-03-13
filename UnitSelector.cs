using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public Unit selectedUnit;
    public Unit prevselectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse is down");

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                if (hitInfo.transform.gameObject.tag == "Player")
                {
                    if (prevselectedUnit)
                    {
                        prevselectedUnit.DeactivateSelection();
                    }

                    Debug.Log("Hit a unit!");
                    selectedUnit = hitInfo.transform.gameObject.GetComponent<Unit>();
                    selectedUnit.ActivateSelection();
                    prevselectedUnit = selectedUnit;
                    
                }
                else
                {
                    if (hitInfo.transform.gameObject.tag == "AI")
                    {
                        Debug.Log("Hit the AI");

                        if (selectedUnit)
                        {
                            selectedUnit.Attack(hitInfo.transform, hitInfo.transform.gameObject);
                        }

                    }
                    else
                    {
                        if (hitInfo.transform.gameObject.tag == "Floor")
                        {
                            Debug.Log("Hit the floor");
                            if (selectedUnit)
                            {
                                selectedUnit.Move(hitInfo.transform);
                            }
                        }
                    }
          
                }
            }
            else
            {
                Debug.Log("No hit");
            }
            Debug.Log("Mouse is down");
        }
    }
}
