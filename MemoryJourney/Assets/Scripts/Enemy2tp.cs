using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2tp : MonoBehaviour
{
    private bool IsActivated = true;
    private bool IsCounting = false;
    [SerializeField] private GameObject TpLocationMaster;
    [SerializeField] private GameObject Player;
    Transform[] children;


    void Start()
    {
        children = TpLocationMaster.GetComponentsInChildren<Transform>();
       // transform.position = children[0
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActivated)
        {
            if(!IsCounting)
            {
                StartCoroutine(Tp());

            }

        }



        
    }


    private IEnumerator Tp()
    {
        IsCounting = true;
        yield return new WaitForSeconds(2);

        int i = FindPos();

        transform.position = children[i].transform.position;
        transform.rotation = children[i].transform.rotation;
        IsCounting = false;
    }


    private int FindPos()
    {
        int i = Random.Range(1, children.Length);


        if (Vector3.Distance(children[i].transform.position,Player.transform.position) < 50)
        {
            return i;
        }
        else
        {
            return FindPos();

        }
    }


}
