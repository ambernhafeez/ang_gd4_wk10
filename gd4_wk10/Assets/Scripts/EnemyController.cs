using System.Linq.Expressions;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int currentHealth = 3;
    public float speed = 5.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // turn off the ragdoll by default
        foreach(Rigidbody ragdollBone in GetComponentsInChildren<Rigidbody>())
        {
            ragdollBone.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void RagdollTrigger()
    {
        GetComponent<Animator>().enabled = false;

        foreach(Rigidbody ragdollBone in GetComponentsInChildren<Rigidbody>())
        {
            ragdollBone.isKinematic = false;
            Debug.Log("bones not kinematic");
        }
        
        GetComponent<Collider>().enabled = false;
        Debug.Log("collider disabled");
    }

    public void Damage(int damageAmount)
	{
		//subtract damage amount when Damage function is called
		currentHealth -= damageAmount;
        Debug.Log("Took damage");

		//Check if health has fallen below zero
		if (currentHealth <= 0) 
		{
            
			RagdollTrigger();
            Debug.Log("ragdoll trigger function called");
		}
	}
}
