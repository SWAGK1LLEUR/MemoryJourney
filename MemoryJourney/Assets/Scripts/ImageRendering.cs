using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRendering : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform imageSocket;
    public SpriteRenderer image;
    public Image canvas;

    public void ShowImage(bool active)
    {
        image.enabled = active;
    }

    public void ShowCanvas(bool active)
    {
        canvas.enabled = active;
    }

    private void Update()
    {
        imageSocket.transform.LookAt(player.position);
    }
}
