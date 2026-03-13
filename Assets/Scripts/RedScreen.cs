using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedScreen : MonoBehaviour
{
    public void ScreenOn()
    {
        gameObject.SetActive(true);
        StartCoroutine(ScreenRoutine());
    }

    private IEnumerator ScreenRoutine()
    {
        yield return new WaitForSeconds(5);
        Application.Quit();
    }
    
}
