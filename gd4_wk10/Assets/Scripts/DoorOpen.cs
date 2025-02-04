using Unity.VisualScripting;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator anim;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            anim.SetBool("doorOpen", true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            anim.SetBool("doorOpen", false);
        }
    }
}
