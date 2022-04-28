using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour
{
    public AudioSource secretAudioSource;
    bool istriggered = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider Player)
    {
        if (istriggered == false)
        {
            secretAudioSource.Play();
        }

    }

    private void OnTriggerExit(Collider Player)
    {
        istriggered = true;
    }
}
