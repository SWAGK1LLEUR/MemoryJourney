using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy2tp : MonoBehaviour
{
    private bool IsActivated = true;
    private bool IsCounting = false;
    [SerializeField] private GameObject TpLocationMaster;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject thisEnemy;
    [SerializeField] private Light Flash;
    private bool a = false;
    Transform[] children;
    private bool PlayerSeesEnemy = false;
    private float currentGazeTimeInSeconds;
    [SerializeField] private float holdGazeTimeInSeconds;
    private bool HasSeen = false;

    private bool isDummy = false;
    void Start()
    {
        children = TpLocationMaster.GetComponentsInChildren<Transform>();
        //FindPos();
    }

    // Update is called once per frame
    void Update()
    {
        
        PlayerSeesEnemy = Player.GetComponent<CanPlayerSeeEnemy>().a;

       

        Vector3 targetPostition = new Vector3(Player.transform.position.x,
                                       this.transform.position.y,
                                       Player.transform.position.z);
        this.transform.LookAt(targetPostition);
        if(PlayerSeesEnemy)
        {
            HasSeen = true;

        }

        if(HasSeen)
        {
            currentGazeTimeInSeconds += Time.deltaTime;
            if (currentGazeTimeInSeconds >= holdGazeTimeInSeconds)
            {
                currentGazeTimeInSeconds = 0;
                SceneManager.LoadScene(1, LoadSceneMode.Single);

            }
        }
        if(isDummy)
        {
            if (Flash.intensity != 0 && Vector3.Distance(Player.transform.position, transform.position) < Flash.range / 10)
            {
                thisEnemy.SetActive(false);
                Player.GetComponent<EnemyActiveManager>().CallRealEnemy2();
            }

        }
        else if (Flash.intensity != 0 && Vector3.Distance(Player.transform.position, transform.position) < Flash.range / 2 && PlayerSeesEnemy && !a)
        {
            HasSeen = false;
            currentGazeTimeInSeconds = 0;
            StopAllCoroutines();
            int i = FindPos();
            a = true;
            transform.position = children[i].transform.position;
            Vector3 targetPostition2 = new Vector3(Player.transform.position.x,
                               this.transform.position.y,
                               Player.transform.position.z);
            this.transform.LookAt(targetPostition2);
            IsCounting = false;

            StartCoroutine(Wait());

        }


        if (IsActivated && currentGazeTimeInSeconds != 0)
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
        yield return new WaitForSeconds(10);

        int i = FindPos();



        IsCounting = false;
    }


    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);


        a = false;
    }


    private int FindPos()
    {
        int current = 0;
        for(int j = 0; j < children.Length; ++j)
        {
            if(transform.position == children[j].transform.position)
            {
                current = j;
            }

        }


        int i = Random.Range(1, children.Length);
       

        if (Vector3.Distance(children[i].transform.position,Player.transform.position) < 50 && Vector3.Distance(children[i].transform.position, Player.transform.position) > 10 && children[i].transform.position.y - Player.transform.position.y < 10 && i != current)
        {
            transform.position = children[i].transform.position;
            Vector3 targetPostition = new Vector3(Player.transform.position.x,
                               this.transform.position.y,
                               Player.transform.position.z);
            this.transform.LookAt(targetPostition);
            //if (!TestAngle(children[i], this.gameObject))
            //{
            //    return FindPos();
            //}

            return i;
        }
        else
        {


            return FindPos();

        }
    }

    private bool TestAngle(Transform obj1, GameObject obj2)
    {
       return Mathf.Abs(obj1.rotation.y - obj2.transform.rotation.y) < 10 * Mathf.PI / 180;

    }


    public void ChangeTodummy()
    {
        isDummy = true;

    }

}
