using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinInScene : MonoBehaviour
{
    [SerializeField] private GameObject player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        if(Vector3.Distance(player.transform.position, transform.position) < 2)
        {
            SceneManager.LoadScene("WinningScreen");
            
        }


        
    }

}
