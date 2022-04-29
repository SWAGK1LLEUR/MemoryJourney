using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto_Apparition : MonoBehaviour
{
    public AudioSource tutoAudioSource;
    public GameObject tutoDeplacement;
    public GameObject tutoCameraMove;
    public int tutoDeplacementWait = 5;
    public int tutoCameraMoveWait = 10;
    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine(CoroutineDeplacement());
        StartCoroutine(CoroutineCameraMove());
    }

    private IEnumerator CoroutineDeplacement()
    {
        yield return new WaitForSeconds(tutoDeplacementWait);
        tutoDeplacement.SetActive(true);
        tutoAudioSource.Play();
        yield return new WaitForSeconds(5);
        tutoDeplacement.SetActive(false);
    }

    private IEnumerator CoroutineCameraMove()
    {
        yield return new WaitForSeconds(tutoCameraMoveWait);
        tutoCameraMove.SetActive(true);
        tutoAudioSource.Play();
        yield return new WaitForSeconds(5);
        tutoCameraMove.SetActive(false);
    }

}
