using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Listening To:")]
    [SerializeField] BattleManagerEventsChannelSO battleEvents;

    private void OnEnable() 
    {
        battleEvents.EndBattleEvent.OnEventRaised += BackToMainMenu;
    }
    
    private void OnDisable() 
    {
        battleEvents.EndBattleEvent.OnEventRaised -= BackToMainMenu;
    }

    void BackToMainMenu()
    {
        StartCoroutine(MainMenuCoroutine(5));
    }

    IEnumerator MainMenuCoroutine(int time)
    {
        float timer = time;

        while ( timer > 0 )
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(0);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Debug.Log("Game Should Close!");
        Application.Quit();
    }
}
