using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TractorController : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject seedPrefab;
     Vector3 targetPosition;
    bool hasTarget = false;
    public float score = 0f;
    public float seedsLimit = 20f;
    public Image seedsLeft;
    float tractorScore = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        seedsLeft.fillAmount = tractorScore / seedsLimit;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.forward * 2);
        seedsLeft.transform.position = screenPos;

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                //Debug.Log(hit.transform.name);

                // when on tillable land and have seeds, plant seeds
                if(hit.transform.CompareTag("Tillable") && tractorScore > 0)
                {
                    targetPosition = hit.point;
                    hasTarget = true;
                    StartCoroutine(WaitForArrival());
                }
                
            }
        }
    }

    // most of WaitForArrival function written by chatGPT
    IEnumerator WaitForArrival()
    {
        // Wait until the agent is very close to the target
        while (hasTarget && agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        // Ensure the agent reached the target
        if (hasTarget && agent.remainingDistance <= agent.stoppingDistance)
        {
            // below three lines were from my original function 
            Instantiate(seedPrefab, targetPosition, Quaternion.identity);
            tractorScore -= 1;
            seedsLeft.fillAmount = tractorScore / seedsLimit;
            hasTarget = false; 
        }
    }
    
    // when collide with seed crate, refill seeds to seedsLimit
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SeedCrate") && tractorScore < seedsLimit)
        {
            tractorScore = seedsLimit;
            seedsLeft.fillAmount = tractorScore / seedsLimit;
        }
    }
}
