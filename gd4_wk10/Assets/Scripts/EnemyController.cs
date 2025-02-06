using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    public int currentHealth = 3;
    public float speed = 5.5f;
    Transform player;
    NavMeshAgent _agent;
    Animator anim;
    public bool dead = false;
    Vector3 currentDestination;
    [SerializeField] float followDistance = 20;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();

        // turn off the ragdoll by default
        foreach(Rigidbody ragdollBone in GetComponentsInChildren<Rigidbody>())
        {
            ragdollBone.isKinematic = true;
        }

        _agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == false)
        {
            // follow player when within followDistance but not too close
            if(Vector3.Distance(transform.position, player.position) < followDistance && Vector3.Distance(transform.position, player.position) > 2)
            {
                Follow();
            }
            // when very close, do not follow player and attack
            else if(Vector3.Distance(transform.position, player.position) <= 2)
            {
                Idle();
                Attack();
            }
            else 
            {
                Search();
            }
        }
    }

    public void RagdollTrigger()
    {
        GetComponent<Animator>().enabled = false;

        foreach(Rigidbody ragdollBone in GetComponentsInChildren<Rigidbody>())
        {
            ragdollBone.isKinematic = false;
        }
        
        GetComponent<Collider>().enabled = false;
    }

    public void Damage(int damageAmount)
	{
		//subtract damage amount when Damage function is called
		currentHealth -= damageAmount;

		//Check if health has fallen below zero
		if (currentHealth <= 0) 
		{
			RagdollTrigger();
            dead = true;
		}
	}

    void Follow()
    {
        _agent.destination = player.position;
        anim.SetBool("isRunning", true);
    }

    void Idle()
    {
        _agent.destination = transform.position;
        anim.SetBool("isRunning", false);
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
        anim.SetBool("isRunning", true);
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
