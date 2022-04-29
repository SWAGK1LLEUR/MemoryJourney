using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OncollisionDoorClose : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject DoorToClose;


    public bool a = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(Player.transform.position, transform.position) < 10)
        {
            a = true;
            Enemy1.SetActive(false);
        }
        else
        {
            a = false;
        }
    }


}
