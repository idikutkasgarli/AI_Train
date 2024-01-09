using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
    //fires ball clicked position
    [SerializeField] private GameObject FirePoint;
    [SerializeField] private GameObject CanonBallPrefab;
    //[SerializeField] private float FireForce = 1000f;
    //[SerializeField] private float FireRate = 1f;


    void LookRotation()
    {
        //rotate canon to mouse position
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f; //The distance between the camera and object
        Vector3 objectPos = Camera.main.WorldToScreenPoint(FirePoint.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 10f));
    }
    void Update()
    {
        LookRotation();
        if (Input.GetMouseButtonDown(0))
        {
            //fire canon ball
            GameObject ball = Instantiate(CanonBallPrefab, FirePoint.transform.position, FirePoint.transform.rotation);
            ball.GetComponent<Rigidbody>().AddForce(FirePoint.transform.forward * 10000f);
            Destroy(ball, 5f);
        }
    }

}
