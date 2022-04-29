using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpObj : MonoBehaviour
{
    // Start is called before the first frame update
 
    private bool canPick = true;
    public bool picking = false;
    [SerializeField] public GameObject PickObj;
    [SerializeField] private GameObject PutObj;
    [SerializeField] private GameObject viewObj;
    [SerializeField] private GameObject Playercenter;
    [SerializeField] private GameObject Playerhand;
    [SerializeField] private GameObject PlayerLeftHand;
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject ObjDesk;
    [SerializeField] private Quaternion objRot;
    [SerializeField] private string tag;
    [SerializeField] private string tag2;
    [SerializeField] private string tag3;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float DistancePictureDesk;
    [SerializeField] private float DistanceSeeInteractibles;
    public GameObject Inventory;
    private List<ImageRendering> InteractibleEyeList;
    private List<ImageRendering> InteractibleHandList;
    private ImageRendering CamInteractible;
    private float ObjectPitch;
    private float ObjectYaw;
    private float distancehand;
    private float distanceLeftHand;
    private float OffsetLeftHand;
    private StarterAssetsInputs _input;
    private bool SecondHandFull = false;
    private Vector3 OriginalPos = Vector3.zero;

    // Interactibles
    [SerializeField] private float interactibleDistance;

    public bool HasCamera = false;
    void Start()
    {
        InteractibleEyeList = new List<ImageRendering>();
        InteractibleHandList = new List<ImageRendering>();

        _input = GetComponent<StarterAssetsInputs>();
        distancehand = Vector3.Distance(Camera.transform.position, Playerhand.transform.position);
        distanceLeftHand = Vector3.Distance(Camera.transform.position, PlayerLeftHand.transform.position);
        GameObject[] eyeObj = GameObject.FindGameObjectsWithTag(tag);
        for(int i = 0; i < eyeObj.Length; ++i)
        {
            InteractibleEyeList.Add(eyeObj[i].GetComponent<ImageRendering>());
        }
        CamInteractible = GameObject.FindGameObjectWithTag(tag2).GetComponent<ImageRendering>();
        GameObject[] handObj = GameObject.FindGameObjectsWithTag(tag3);
        for (int i = 0; i < handObj.Length; ++i)
        {
            InteractibleHandList.Add(handObj[i].GetComponent<ImageRendering>());
        }
    }

    // Update is called once per frame
    void Update()
    {
    
        Vector3 resultingPosition = Camera.transform.position + Camera.transform.forward * distanceLeftHand;
        //Playerhand.transform.position = resultingPosition;
     //   PlayerLeftHand.transform.position = resultingPosition;
        RaycastHit hitcheck;
                
        objRot = transform.rotation;

        for(int i = 0; i < InteractibleEyeList.Count; ++i)
        {
            if (PickObj != null)
            {
                if (Vector3.Distance(this.transform.position, InteractibleEyeList[i].transform.position) < DistanceSeeInteractibles && InteractibleEyeList[i].transform.position != PickObj.transform.position)
                {
                    InteractibleEyeList[i].ShowImage(true);
                }
                else
                {
                    InteractibleEyeList[i].ShowImage(false);
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, InteractibleEyeList[i].transform.position) < DistanceSeeInteractibles)
                {
                    InteractibleEyeList[i].ShowImage(true);
                }
                else
                {
                    InteractibleEyeList[i].ShowImage(false);
                }
            }
        }

        for (int i = 0; i < InteractibleHandList.Count; ++i)
        {
            if (Vector3.Distance(this.transform.position, InteractibleHandList[i].transform.position) < DistanceSeeInteractibles)
            {
                InteractibleHandList[i].ShowImage(true);
            }
            else
            {
                InteractibleHandList[i].ShowImage(false);
            }
        }

        if (Vector3.Distance(this.transform.position, CamInteractible.transform.position) < DistanceSeeInteractibles)
        {
            CamInteractible.ShowImage(true);
        }
        else
        {
            CamInteractible.ShowImage(false);
        }

        RaycastHit hit;
        Debug.DrawRay(Playercenter.gameObject.transform.position, Playercenter.gameObject.transform.forward);

        //Debug.DrawRay(Playercenter.gameObject.transform.position, Playercenter.transform.forward, Color.magenta);
        if (_input.pick && canPick)
        {
            if (Physics.Raycast(Playercenter.gameObject.transform.position, Playercenter.gameObject.transform.forward, out hit, interactibleDistance) && (hit.collider.gameObject.tag == tag || hit.collider.gameObject.tag == tag2))
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

                hit.transform.rotation = objRot;


            }

            if (Physics.Raycast(Playercenter.gameObject.transform.position, Playercenter.gameObject.transform.forward, out hit, interactibleDistance) && (hit.collider.gameObject.tag == tag3))
            {
                Animator anim = hit.collider.gameObject.GetComponentInParent<Animator>();
                bool isOpen = anim.GetBool("Open");
                anim.SetBool("Open", !isOpen);
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

        if (!canPick)
        {

            if (Vector3.Distance(ObjDesk.transform.position, PickObj.transform.position) < DistancePictureDesk)
            {
                Transform[] children = ObjDesk.GetComponentsInChildren<Transform>();
                for (int i = 1; i < children.Length; ++i)
                {
                    if(children[i].transform.childCount == 0 && children[i].transform.tag == "Place")
                    {
                        PickObj.transform.parent = children[i].transform;
                        PickObj.transform.tag = "Untagged";
                        PickObj.transform.position = children[i].transform.position;
                        for(int j = 0; j < InteractibleEyeList.Count; ++j)
                        {
                            if(PickObj.transform.position == InteractibleEyeList[j].transform.position)
                            {
                                InteractibleEyeList.Remove(InteractibleEyeList[j]);
                                break;
                            }

                        }
                        
                        break;
                    }
                }
                Inventory = null;


            }
            else
            {

                if(PickObj.transform.tag == "FlashObj")
                {
                    SecondHandFull = true;
                    PickObj.transform.tag = "Untagged";
                    PutObj = PickObj;
                    PickObj = null;
                    PutObj.transform.position = PlayerLeftHand.transform.position;
                    HasCamera = true;
                    OriginalPos = Vector3.zero;
                }
                else if(PickObj.transform.tag == "CanPick")
                {

                    if (Inventory == null)
                    {
                        PickObj.GetComponent<uiImage>().ShowImage(true);
                        Inventory = PickObj;
                        PickObj = null;
                        Inventory.transform.position = new Vector3(999, 999, 999);
                        OriginalPos = Vector3.zero;
                    }
                    else
                    {
                        PickObj.transform.position = OriginalPos;
                        OriginalPos = Vector3.zero;
                        PickObj.transform.parent = null;
                        PickObj = null;
                        OriginalPos = Vector3.zero;

                    }

                }

           
            }
            canPick = true;
        }

        if(_input.Put && Inventory != null && !picking)
        {
            picking = true;
            PickObj = Inventory;
            PickObj.GetComponent<uiImage>().ShowImage(false);
            Inventory = null;
            SecondHandFull = false;
            PickObj.transform.position = Playerhand.transform.position;
            
            PickObj.transform.rotation = objRot;

        }

        if (Physics.Raycast(Playercenter.gameObject.transform.position, Playercenter.gameObject.transform.forward, out hit, interactibleDistance) && (hit.collider.gameObject.tag == tag || hit.collider.gameObject.tag == tag2))
        {
            viewObj = hit.collider.gameObject;
            if (PickObj != viewObj && Inventory != viewObj)
            {
                ImageRendering type = viewObj.GetComponent<ImageRendering>();
                type.ShowCanvas(true);
            }
        }
        else
        {
            if (viewObj != null)
            {
                ImageRendering type = viewObj.GetComponent<ImageRendering>();
                type.ShowCanvas(false);
            }
        }
    }
}