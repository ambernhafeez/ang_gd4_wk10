using UnityEngine;
using UnityEngine.AI;

public class SlimeController : MonoBehaviour
{
    NavMeshAgent _agent;
    Transform enemy;
    Animator anim;
    Vector3 currentDestination;
    [SerializeField] float followDistance = 10f;
    EnemyController enemyScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        // follow player when within followDistance but not too close
        if(Vector3.Distance(transform.position, enemy.position) < followDistance && Vector3.Distance(transform.position, enemy.position) > 2)
        {
            Follow();
        }
        // when very close, do not follow player and attack
        else if(Vector3.Distance(transform.position, enemy.position) <= 2)
        {
            Idle();
            Attack();
        }
        else 
        {
            Search();
        }
    }

    void Follow()
    {
        _agent.destination = enemy.position;
        anim.SetFloat("Speed", 2);
    }

    void Idle()
    {
        _agent.destination = transform.position;
        anim.SetFloat("Speed", 0);
    }

    void Search()
    {
        if (Vector3.Distance(currentDestination, transform.position) < 5)
        {
            currentDestination = transform.position + (new Vector3(Random.Range(-10,10), 0, Random.Range(-10,10)));

            // can also use an array of waypoint gameobjects and do 
            // currentDestination = waypoints[Random.Range(0, waypoints.length)].transform.position;
        }

        _agent.destination = currentDestination;
        anim.SetFloat("Speed", 1);
    }

    void Attack()
    {
        enemyScript.Damage(1);
    }
}
