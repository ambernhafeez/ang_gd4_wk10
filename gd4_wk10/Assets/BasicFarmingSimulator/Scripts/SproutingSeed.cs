using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SproutingSeed : MonoBehaviour
{
    float sproutTime = 10;
    public GameObject plantPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        StartCoroutine(SproutSeed());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SproutSeed()
    {
        //Debug.Log("starting to sprout");
        yield return new WaitForSeconds(sproutTime);
        //Debug.Log("time to sprout");
        Destroy(gameObject);
        Instantiate(plantPrefab, transform.position, Quaternion.identity);
    }
}
