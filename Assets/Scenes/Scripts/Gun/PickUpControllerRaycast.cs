using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpControllerRaycast : MonoBehaviour
{
    public RayCastGun gunScript;
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public Transform playerController, gunContainer, fpsCam, currentWeapon;

    public float pickUpRange, pickUpTime;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    private bool pickingUp;
    public static bool slotFull;

    void Start()
    {
        if(!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            boxCollider.isTrigger = false;
        }
        if(equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            boxCollider.isTrigger = true;
            slotFull = true;
        }
    }
    void FixedUpdate()
    {
        if(pickingUp && currentWeapon != null)
        {
            Vector3 zero = Vector3.zero;
            
            currentWeapon.transform.localRotation = Quaternion.Slerp(currentWeapon.transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * pickUpTime * 10f);
            currentWeapon.transform.localPosition = Vector3.SmoothDamp(currentWeapon.transform.localPosition, Vector3.zero, ref zero, 1f / (pickUpTime * 10f));
        }
    }
    private void Update()
    {
        //check if playerController is in range of gun and "E" is pressed
        Vector3 distanceToPlayer = playerController.position - transform.position;
        if(!equipped && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull)
        {
            PickUp();
        }

        //drop if equipped and "Q" is pressed
        if(equipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    void PickUp()
    {
        equipped = true;
        slotFull = true;

        //make rb kinematic and collider a trigger
        rb.isKinematic = true;
        boxCollider.isTrigger = true;

        //set parent to gun container
        transform.SetParent(gunContainer);
        Invoke(nameof(PickUpFinished), pickUpTime);
        //set rotation to 0
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        //transform.localRotation = Quaternion.Euler(Vector3.zero);
        //transform.localScale = Vector3.one;

        //enable script
        gunScript.enabled = true;
    }

    void Drop()
    {
        equipped = false;
        slotFull = false;

        //make rb not kinematic and collider not a trigger
        rb.isKinematic = false;
        boxCollider.isTrigger = false;

        //disable script
        gunScript.enabled = false;

        //gun carries momentum of playerController
        rb.velocity = playerController.GetComponent<Rigidbody>().velocity;

        //add force to gun
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //add random rotation
        float rand = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(rand, rand, rand) *10f);

        //set parent to null
        transform.SetParent(null);
    }

    void PickUpFinished()
    {
        pickingUp = false;
    }
}
