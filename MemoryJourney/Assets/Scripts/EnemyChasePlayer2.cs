using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyChasePlayer2 : MonoBehaviour
{
    [SerializeField] private GameObject Player;


    private int MinDist = 2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player.transform);

        if (Vector3.Distance(transform.position, Player.transform.position) >= MinDist)
        {

            transform.position += transform.forward * 2 * Time.deltaTime;




        }
        else
        {
            SceneManager.LoadScene(3, LoadSceneMode.Single);
        }

    }
}
