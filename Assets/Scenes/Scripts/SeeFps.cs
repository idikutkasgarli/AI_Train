using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeFps : MonoBehaviour
{
    public float timer, refresh, avgFramerate;
    public string display = "{0} FPS";

    // Update is called once per frame
    void Update()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if(timer <=0) avgFramerate = (int) (1f / timelapse);
        //Debug.Log(avgFramerate);
    }
}
