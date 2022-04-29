using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OncollisionDoorClose : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy1dummy;
    [SerializeField] private GameObject DoorToClose;


    public bool a = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(Player.transform.position, transform.position) < 5)
        {
            a = true;
            StartCoroutine(SpawnEnemy());
            DoorToClose.SetActive(true);
        }
        else
        {
            a = false;
        }
    }


    private IEnumerator SpawnEnemy()
    {

        yield return new WaitForSeconds(5);

        Enemy1dummy.SetActive(true);
        Enemy1dummy.transform.position = transform.position;


    }


}
