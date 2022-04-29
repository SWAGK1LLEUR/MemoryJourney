using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class overButtonSound : MonoBehaviour, IPointerEnterHandler
{
    // Start is called before the first frame update
    public AudioSource overButtonSound1;
    public void OnPointerEnter(PointerEventData eventData)
    {
        overButtonSound1.Play();
    }
}
