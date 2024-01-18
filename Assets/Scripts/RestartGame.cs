using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //restart game
        if (Input.GetKeyDown(KeyCode.R))
        {
            //use scene manager to load scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Update is called once per frame  
    void Update()
    {
        
    }
}
