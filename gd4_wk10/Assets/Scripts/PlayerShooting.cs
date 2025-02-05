using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] int gunDamage = 1;
    [SerializeField] float fireRate = 0.25f;
    [SerializeField] float range = 60;
    [SerializeField] float hitForce = 20;
    float nextFire;
    float ammoReload = 10f;
    public float ammo;
    public Image ammoBar;

    [Header("Weapon Shooting Visuals")]
    [SerializeField] float shotDuration = 0.2f;
    public ParticleSystem plasmaExplosion;
    // referenced in Start
    AudioSource _as;
    LineRenderer _lr;

    [Header("Camera and Positioning")]
    [SerializeField] Transform gunEnd;
    Camera playerCam;

    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _as = GetComponent<AudioSource>();
        playerCam = GetComponentInParent<Camera>();
        ammo = ammoReload;
    }

    void Update()
    {
        // Time.time will count how mcuh time has passed since started the game
        // nextFire will start out being zero. So definitely will be able to click Fire1 the first time
        // then nextFire becomes equal to the current time plus the fire rate, so always need to wait at least as long as the fireRate between shots.
        if(Input.GetButtonDown("Fire1") && Time.time > nextFire && ammo > 0)
        {
            nextFire = Time.time + fireRate;
            ammo -= 1;
            AmmoBarUpdate();

            StartCoroutine(ShootingEffect());

            // raycast origin is set to 0.5,0.5,0 as this is the centre of the x and y axes (they go from 0 to 1 in the viewport)
            Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,0));

            // Resolve raycast hit
            RaycastHit hit;

            // the line renderer starts at the gunEnd
            _lr.SetPosition(0,gunEnd.transform.position);

            // for Physics.Raycast, need the origin, direction, output and range
            // hit works similar to OnTriggerEnter 'other'
            // the raycast will originate from the player camera for ease but the line rendered will be from the gun as set above
            if(Physics.Raycast(rayOrigin, playerCam.transform.forward, out hit, range))
            {
                // hit.point gives access to the exact point that the raycast hit
                // if we hit something, this is the end point of they raycast
                _lr.SetPosition(1, hit.point);

                Debug.Log(hit.transform.name);

                // shootablebox - when hit target we look for shootablebox component
                ShootableBox targetBox = hit.transform.GetComponent<ShootableBox>();
                
                // do damage if hit a target
                if (targetBox != null)
                {
                    targetBox.Damage(gunDamage);
                }

                // if there is a rigidbody, return the angle at which we hit the object (hit.normal)
                // and apply a force at this angle multiplied by the hitforce
                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * hitForce, ForceMode.Impulse);
                }

                // trigger the ragdoll
                EnemyController enemy = hit.collider.GetComponentInParent<EnemyController>();
                if(enemy != null) 
                { 
                    enemy.Damage(gunDamage);
                }

                // hit particle effect
                Destroy(Instantiate(plasmaExplosion, hit.point, Quaternion.identity), 2);

                // add muzzle particle effect on shoot
                //gunEnd.GetChild(0).GetComponent<ParticleSystem>().Play()
            }
            else
            {
                // if we don't hit anything, the end point is just a big number away
                _lr.SetPosition(1, playerCam.transform.forward * 100000);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ammo = ammoReload;
            AmmoBarUpdate();
        }
    }

    IEnumerator ShootingEffect()
    {
        _as.Play();
        // turn the laser on!
        _lr.enabled = true;
        //Debug.Log("Enabled line renderer");

        yield return new WaitForSeconds(shotDuration);

        _lr.enabled = false;
    }

    void AmmoBarUpdate()
    {
        ammoBar.fillAmount = ammo / 10f;
    }
    
}