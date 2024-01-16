using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {





        //basic player controller jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.Translate(Vector3.up * Time.deltaTime * 5f);
        }
    
        
        
    }
}
