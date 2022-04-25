using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChasingAI : MonoBehaviour
{
   

    //[SerializeField] private AnimationCurve curve;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    [SerializeField] private Transform spotter;
    [SerializeField] private LayerMask whatIsGround, whatIsPlayer, whatIsNotGround, whatIsNotPlayer;
    //[SerializeField] private bool showDebugLine = false;
    [SerializeField] private float detectionFOV;

    //Patroling
    [SerializeField] private bool freeRoamingEnemy;
    [SerializeField] private Vector3 patrolPointOrigin;
    [SerializeField] private Vector3 walkPoint;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float heightDetectionMax;
    private bool walkPointSet;

    //Attacking
    [SerializeField] private float minTimeBeforeAttack;
    [SerializeField] private float maxTimeBeforeAttack;
    [SerializeField] private float additionalChasingSpeed;

    //States
    [SerializeField] private float soundDetectionRange;
    [SerializeField] private float chaseRange;
    [SerializeField] private float attackRange;

    //Sounds
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private List<AudioClip> soundList;
    [SerializeField] private float minSoundTime;
    [SerializeField] private float maxSoundTime;

    //Echo    
    [SerializeField] private float soundRange;
    [SerializeField] private float soundTravelSpeed;
    [SerializeField] private int soundRepetition;


    private bool playerInChaseRange;
    private bool playerInAttackRange;

    private int currentPriority = int.MaxValue;
    private bool canSeePlayer = false;
    private bool isAware = false;
    private float lostSightTimer = 0;
    private float normalSpeed;
    private float CurrentFloor;
    private bool IsChangingFloors;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private bool showLine;

    //private float offsetmodif;
    public bool WalkPointSet { get => walkPointSet; set => walkPointSet = value; }
    public float DetectionFOV { get => detectionFOV; set => detectionFOV = value; }
    public float SoundDetectionRange { get => soundDetectionRange; set => soundDetectionRange = value; }
    public bool CanSeePlayer { get => canSeePlayer; set => canSeePlayer = value; }
    public Transform Player { get => player; set => player = value; }
    public float AttackRange { get => attackRange; set => attackRange = value; }
    public float WalkPointRange { get => walkPointRange; set => walkPointRange = value; }
    public Vector3 PatrolPointOrigin { get => patrolPointOrigin; set => patrolPointOrigin = value; }
    public float ChaseRange { get => chaseRange; set => chaseRange = value; }
    public Vector3 WalkPoint { get => walkPoint; set => walkPoint = value; }
    public bool FreeRoamingEnemy { get => freeRoamingEnemy; set => freeRoamingEnemy = value; }
    public Transform Spotter { get => spotter; set => spotter = value; }

    // Start is called before the first frame update
    void Start()
    {
        //offsetmodif = 0.5f;
        StartCoroutine(FOVRoutine());
        normalSpeed = agent.speed;

        walkPointSet = false;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;

        CurrentFloor = 0;
        IsChangingFloors = false;
    }

    private void Update()
    {
        print(CurrentFloor);
        lostSightTimer += Time.deltaTime;
        if(!IsChangingFloors)
        {
            StartCoroutine(ChangeFloors());
        }

        if (lostSightTimer > 2)
        {
            canSeePlayer = false;
            isAware = false;
        }

        if (agent.hasPath && showLine)
            DrawPath();

        else if (!showLine)
        {
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            lineRenderer.positionCount = 0;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        playerInChaseRange = Physics.CheckSphere(Spotter.position, ChaseRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(Spotter.position, AttackRange, whatIsPlayer);

        if (playerInChaseRange && canSeePlayer)        
            ChasePlayer();            
        
        else
            Patroling();

        //if (playerInAttackRange)
        //    AttackPlayer();        

        Spotter.LookAt(agent.pathEndPosition);
    }

    private void Patroling()
    {
        agent.speed = normalSpeed;
        if (!WalkPointSet)
            SearchNewWalkPoint();

        if (WalkPointSet && isAware)
            return;
        else
            agent.SetDestination(WalkPoint);

        Vector3 distanceToWalkPoint = transform.position - WalkPoint;

        if (distanceToWalkPoint.magnitude < 1.5f)
            WalkPointSet = false;
    }

    private void SearchNewWalkPoint()
    {
        RaycastHit hit;
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
       

        if(FreeRoamingEnemy)
        {
            WalkPoint = new Vector3(this.transform.position.x + randomX, this.transform.position.y, this.transform.position.z + randomZ);
        }
            
        else
        {
            if(CurrentFloor == 1)
            {
                WalkPoint = new Vector3(patrolPointOrigin.x + randomX, patrolPointOrigin.y + 25f, patrolPointOrigin.z + randomZ);
            }
            else
            {
                WalkPoint = new Vector3(patrolPointOrigin.x + randomX, patrolPointOrigin.y, patrolPointOrigin.z + randomZ);
            }
            
        }
           

        if (Physics.Raycast(WalkPoint + new Vector3(0, 10, 0), -transform.up, out hit, 11, whatIsGround))
        {
            WalkPoint = new Vector3(hit.point.x, hit.point.y, hit.point.z);

            if (SetDestination(WalkPoint))
            {
                walkPointSet = true;
                return;
            }
        }

        SearchNewWalkPoint();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Player.position);
        agent.speed = normalSpeed + additionalChasingSpeed;
    }

    private void AttackPlayer()
    {

        agent.SetDestination(transform.position);
        transform.LookAt(Player);
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            CheckConeRange();
        }
    }

    private IEnumerator ChangeFloors()
    {
        IsChangingFloors = true;
        yield return new WaitForSeconds(100);
        if(CurrentFloor == 0)
        {
            CurrentFloor = 1;
        }
        else
        {
            CurrentFloor = 0;
        }
        IsChangingFloors = false;

    }



    private void CheckConeRange()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(Spotter.position, ChaseRange, whatIsPlayer);

        if (rangeCheck.Length != 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector3 dir = (target.position - Spotter.position).normalized;

            if (Vector3.Angle(Spotter.forward, dir) < DetectionFOV / 2)
            {
                float dist = Vector3.Distance(Spotter.position, target.position);

                if (Physics.Raycast(Spotter.position, dir, dist, whatIsPlayer) && !(Physics.Raycast(Spotter.position, dir, dist, whatIsNotPlayer)))
                {
                    CanSeePlayer = true;
                    lostSightTimer = 0;
                }
            }
        }
    }

    private bool SetDestination(Vector3 targetDestination)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetDestination, out hit, 1f, NavMesh.AllAreas))
        {
            WalkPoint = hit.position;
            return true;
        }

        return false;
    }

    private void DrawPath()
    {
        if (showLine)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPosition(0, transform.position);

            if (agent.path.corners.Length < 2)
                return;

            for (int i = 1; i < agent.path.corners.Length; ++i)
            {
                Vector3 ptPos = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);
                lineRenderer.SetPosition(i, ptPos);
            }
        }
    }
}

