using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    // Start is called before the first frame update
 
    private bool canPick = true;
    public bool picking = false;
    [SerializeField] private GameObject PickObj;
    [SerializeField] private GameObject PutObj;
    [SerializeField] private GameObject Playercenter;
    [SerializeField] private GameObject Playerhand;
    [SerializeField] private GameObject PlayerLeftHand;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject ObjDesk;
    [SerializeField] private Quaternion objRot;
    [SerializeField] private string tag;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float DistancePictureDesk;
    private float ObjectPitch;
    private float ObjectYaw;
    private float distancehand;
    private float distanceLeftHand;
    private float OffsetLeftHand;
    private StarterAssetsInputs _input;
    private bool SecondHandFull = false;
    private Vector3 OriginalPos = Vector3.zero;
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        distancehand = Vector3.Distance(Camera.transform.position, Playerhand.transform.position);
        distanceLeftHand = Vector3.Distance(Camera.transform.position, PlayerLeftHand.transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {

        print(SecondHandFull);
        Vector3 resultingPosition = Camera.transform.position + Camera.transform.forward * distancehand;
        Playerhand.transform.position = resultingPosition;

        Vector3 resultingPosition2 = Camera.transform.position + Camera.transform.forward * distanceLeftHand;
        resultingPosition2 -= transform.right;
         PlayerLeftHand.transform.position = resultingPosition2;


        RaycastHit hitcheck;


        
        objRot = transform.rotation;
        
        Debug.DrawRay(Playercenter.gameObject.transform.position, Playercenter.transform.forward);
        if (_input.pick && canPick)
        {
            

            RaycastHit hit;

            if (Physics.Raycast(Playercenter.gameObject.transform.position, transform.forward, out hit, 100) && hit.collider.gameObject.tag == tag)
            {
                picking = true;
                if(OriginalPos == Vector3.zero)
                {
                    OriginalPos = hit.transform.position;
                }

                PickObj = hit.collider.gameObject;
                hit.rigidbody.useGravity = false;
                hit.rigidbody.isKinematic = true;
                hit.collider.isTrigger = true;
                hit.transform.parent = gameObject.transform;

                hit.transform.position = Playerhand.transform.position;
               // StartCoroutine(lerpPosition(hit.collider.gameObject, hit.transform.position, Playerhand.transform.position, 0.001f));
                hit.transform.rotation = objRot;


            }


        }

        if(picking)
        {
            float deltaTimeMultiplier = Time.deltaTime;

            ObjectPitch -= _input.look.x * RotationSpeed * deltaTimeMultiplier;
            ObjectYaw -= _input.look.y * RotationSpeed * deltaTimeMultiplier;

            PickObj.transform.Rotate(new Vector3(0, ObjectPitch, 0));
            PickObj.transform.Rotate(new Vector3(ObjectYaw, 0, 0));

        }


        if (!_input.pick && picking)
        {
            picking = false;
            canPick = false;

        }

        if (!canPick && !SecondHandFull)
        {

            if (Vector3.Distance(ObjDesk.transform.position, PickObj.transform.position) < DistancePictureDesk)
            {
                Transform[] children = ObjDesk.GetComponentsInChildren<Transform>();
                for (int i = 1; i < children.Length; ++i)
                {
                    if(children[i].transform.childCount == 0 && children[i].transform.tag == "Place")
                    {
                        PickObj.transform.parent = children[i].transform;
                        PickObj.transform.position = children[i].transform.position;
                        
                        break;
                    }
                }



            }
            else
            {



                SecondHandFull = true;

                PutObj = PickObj;
                PickObj = null;
                PutObj.transform.position = PlayerLeftHand.transform.position;

                OriginalPos = Vector3.zero;
           
            }
            canPick = true;
        }
        else if(!canPick && SecondHandFull)
        {
           
            //PickObj.gameObject.GetComponent<Rigidbody>().useGravity = true;
            //PickObj.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //PickObj.gameObject.GetComponent<Collider>().isTrigger = false;
            PickObj.transform.position = OriginalPos;
            //StartCoroutine(lerpPosition(PickObj, PickObj.transform.position, OriginalPos, 0.01f));
            OriginalPos = Vector3.zero;
            PickObj.transform.parent = null;
            PickObj = null;
            canPick = true;
        }

        if(_input.Put && SecondHandFull && !picking)
        {
            picking = true;
            PickObj = PutObj;
            PutObj = null;
            SecondHandFull = false;
            PickObj.transform.position = Playerhand.transform.position;
            
            //StartCoroutine(lerpPosition(PickObj, PickObj.transform.position, Playerhand.transform.position, 0.01f));
            PickObj.transform.rotation = objRot;

        }


        if(SecondHandFull)
        {
            PutObj.transform.position = PlayerLeftHand.transform.position;

        }

    }




    IEnumerator lerpPosition(GameObject obj, Vector3 StartPos, Vector3 EndPos, float LerpTime)
    {
        float StartTime = Time.time;
        float EndTime = StartTime + LerpTime;

        while (Time.time < EndTime)
        {
            float timeProgressed = (Time.time - StartTime) / LerpTime;  // this will be 0 at the beginning and 1 at the end.
            obj.transform.position = Vector3.Slerp(StartPos, EndPos, timeProgressed);

            yield return new WaitForFixedUpdate();
        }
        obj.transform.position = EndPos;
    }




}