using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiImage : MonoBehaviour
{
    [SerializeField] private Image image;
    public void ShowImage(bool a)
    {
        image.enabled = a;


    }



}
