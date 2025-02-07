using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class HarvesterController : MonoBehaviour
{
    NavMeshAgent agent;
    public float score = 0f;
    public TMP_Text scoreText;
    public float harvestLimit = 20f;
    public Image harvestLeft;
    float harvesterScore = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        harvestLeft.fillAmount = score / harvestLimit;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 2);
        harvestLeft.transform.position = screenPos;

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                //Debug.Log(hit.transform.name);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Plant") && harvesterScore < harvestLimit)
        {
            //Debug.Log(other.transform.name);
            Destroy(other.gameObject);
            harvesterScore += 1;
            Debug.Log(harvesterScore);
            harvestLeft.fillAmount = harvesterScore / harvestLimit;
        }
        
        if(other.CompareTag("HarvestCrate"))
        {
            score += harvesterScore;
            scoreText.text = score + "";
            harvesterScore = 0;
            harvestLeft.fillAmount = harvesterScore / harvestLimit;
        }
    }
}