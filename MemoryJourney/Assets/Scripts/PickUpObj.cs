using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    // Start is called before the first frame update
    private bool guipick = false;
    private bool canPick = true;
    private bool picking = false;
    [SerializeField] private GameObject PickObj;
    [SerializeField] private GameObject PickRef;
    [SerializeField] private GameObject Playercenter;
    [SerializeField] private GameObject Playerhand;
    [SerializeField] private GameObject PlayerLeftHand;
    [SerializeField] private Vector3 objPos;
    [SerializeField] private Quaternion objRot;
    [SerializeField] private string tag;

    private StarterAssetsInputs _input;
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray raycheck = Camera.main.ScreenPointToRay(_input.look);
        RaycastHit hitcheck;


        objPos = Playercenter.transform.position;
        objRot = transform.rotation;
        
        Debug.DrawRay(Playercenter.gameObject.transform.position, Playercenter.transform.forward);
        if (_input.pick && canPick)
        {
            

            RaycastHit hit;

            if (Physics.Raycast(Playercenter.gameObject.transform.position, transform.forward, out hit, 100) && hit.collider.gameObject.tag == tag)
            {
                picking = true;

                
                PickObj = hit.collider.gameObject;
                hit.rigidbody.useGravity = false;
                hit.rigidbody.isKinematic = true;
                hit.collider.isTrigger = true;
                hit.transform.parent = gameObject.transform;
                
                hit.transform.position = Playerhand.transform.position;
                hit.transform.rotation = objRot;


            }


        }

        if (!_input.pick && picking)
        {
            picking = false;
            canPick = false;

        }

        if (!canPick)
        {

            PickObj.transform.parent = PlayerLeftHand.transform;
            PickObj.transform.position = PlayerLeftHand.transform.position;
            PickObj = PickRef;
            canPick = true;
        }

    }






}