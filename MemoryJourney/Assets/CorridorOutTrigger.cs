using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorOutTrigger : MonoBehaviour
{
    public GameObject exitCorridorDoor;
    bool istriggered = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider Player)
    {
        if (istriggered == false)
        {
            exitCorridorDoor.SetActive(false);
        }

    }

    private void OnTriggerExit(Collider Player)
    {
        istriggered = true;
    }
}
