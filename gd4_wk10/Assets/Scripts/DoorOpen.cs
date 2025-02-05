using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            anim.SetBool("isDoorOpen", true);
            Debug.Log("collsion");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            CloseDoor();
        }
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(60);
        anim.SetBool("isDoorOpen", false);
    }
}
