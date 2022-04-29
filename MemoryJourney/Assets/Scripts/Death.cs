using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private LevelChanger lvl;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Next());
    }

    private IEnumerator Next()
    {
        yield return new WaitForSeconds(1);
        lvl.FadeInToNextLevel(1);
    }
}
