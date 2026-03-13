using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IRedScreenEffectProvider
{
    public void OnRedScreen();
}

public class RedScreen : MonoBehaviour, IRedScreenEffectProvider
{
    public void OnRedScreen()
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
