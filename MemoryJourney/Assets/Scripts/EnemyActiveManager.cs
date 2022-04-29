using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActiveManager : MonoBehaviour
{
    [SerializeField] private GameObject Enemy1;
    [SerializeField] private GameObject Enemy2;
    [SerializeField] private GameObject DummyEnemy2;
    [SerializeField] private GameObject Table;
    [SerializeField] private GameObject WinPlace;
    [SerializeField] private GameObject doorClose;

    [SerializeField] private List<GameObject> DoorsAfterOnePhoto;

    [SerializeField] private List<GameObject> DoorsAftertwoPhoto;
    
    [SerializeField] private List<GameObject> DoorsAfterthreePhoto;
    [SerializeField] private List<GameObject> DoorsAfterfourPhoto;
    [SerializeField] private GameObject ThirdPhotoEventPointEnemyone;

    [SerializeField] private GameObject ThirdPhotoEventPointEnemytwo;

    List<Transform> a;
    private bool once = false;
    private int oldInt;
    void Start()
    {
        Enemy1.SetActive(false);
        Enemy2.SetActive(false);
   
        a = GetAllChilds(Table.transform);

    }

    // Update is called once per frame
    void Update()
    {


        int j = 0;
        for (int i = 0; i < a.Count; ++i)
        {
            if (a[i].childCount != 0)
            {
                j++;
            }

        }
        if(j != oldInt)
        {
            oldInt = j;
           
            once = false;
        }

        if(j == 1 && !once)
        {
            once = true;
            for (int i = 0; i < DoorsAfterOnePhoto.Count; ++i)
            {
                UnlockDoor(DoorsAfterOnePhoto[i]);
            }
            

        }
        if(j == 2 && !once)
        {
            once = true;
            Enemy1.SetActive(true);
            for (int i = 0; i < DoorsAftertwoPhoto.Count; ++i)
            {
                UnlockDoor(DoorsAftertwoPhoto[i]);
            }

        }
        if(j == 2 && this.GetComponent<PickUpObj>().Inventory != null && Enemy1.activeInHierarchy == false && !doorClose.GetComponent<OncollisionDoorClose>().a)
        {
            once = true;
            Enemy1.SetActive(true);
            Vector3 startpos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
            Enemy1.transform.position = startpos;
            CallDummyEnemy2();
            DummyEnemy2.GetComponent<Enemy2tp>().ChangeTodummy();
            for (int i = 0; i < DoorsAfterthreePhoto.Count; ++i)
            {
                UnlockDoor(DoorsAfterthreePhoto[i]);
            }
        }
        if(j == 4 && !once)
        {
            once = true;
            for (int i = 0; i < DoorsAfterfourPhoto.Count; ++i)
            {
                UnlockDoor(DoorsAfterfourPhoto[i]);
            }

        }

        if(j == 5 && !once)
        {
            once = true;
            WinPlace.SetActive(true);

        }


    }

    List<Transform> GetAllChilds(Transform _t)
    {
        List<Transform> ts = new List<Transform>();

        foreach (Transform t in _t)
        {
            ts.Add(t);
            if (t.childCount > 0)
                ts.AddRange(GetAllChilds(t));
        }

        return ts;
    }

    private void UnlockDoor(GameObject door)
    {



    }
    private void CallDummyEnemy2()
    {
        DummyEnemy2.SetActive(true);
        DummyEnemy2.transform.position = ThirdPhotoEventPointEnemytwo.transform.position;
    }

    public void CallRealEnemy2()
    {
        Enemy2.SetActive(true);


    }

}
