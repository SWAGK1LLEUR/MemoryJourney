using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photoOne : MonoBehaviour
{
    private bool a = false;
    [SerializeField] private GameObject Player;
    [SerializeField] private AudioSource audio;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!a && Player.GetComponent<PickUpObj>().PickObj == this.gameObject)
        {
            audio.Play();
            a = true;
        }



    }
}
