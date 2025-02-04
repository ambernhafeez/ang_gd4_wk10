using UnityEngine;

public class RayView : MonoBehaviour
{
    public float weaponRange = 50f;
    private Camera fpsCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0));
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.red);
    }
}
