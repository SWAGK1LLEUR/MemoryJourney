using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPlayerSeeEnemy : MonoBehaviour
{
    [SerializeField] private Camera Cam;

    [SerializeField] private GameObject Enemy;
    private RaycastHit hit;
    public bool a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        a = IsTargetVisible(Cam, Enemy);
        if(a)
        {
            
        }
        else
        {
            
        }

    }

    bool IsTargetVisible(Camera c, GameObject go)
    {
        Vector3 screenPoint = c.WorldToViewportPoint(go.transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            if (Physics.Linecast(c.transform.position, go.GetComponentInChildren<Renderer>().bounds.center, out hit))
            {
                if(hit.collider.tag == "Kevin" && Vector3.Distance(transform.position, Enemy.transform.position) < 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

            return false;

        }
        else
        {
            
            return false;
        }
    }

}
