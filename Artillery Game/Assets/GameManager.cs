using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Listening to:")]
    [SerializeField] InputReader inputReader;

    [Header("Prefabs:")]
    [SerializeField] GameObject pauseMenuPrefab;
    GameObject pauseMenuInstance;


    void OnEnable() 
    {
        inputReader.PauseGameEvent += PauseGame;

    }   

    void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            SpawnPauseMenu();
        }
        else
        {
            ResumeGame();
        }
    }

    void SpawnPauseMenu()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        pauseMenuInstance = Instantiate(pauseMenuPrefab, canvas.transform.position, Quaternion.identity);
        pauseMenuInstance.transform.SetParent(canvas.transform);
    }

    void ResumeGame()
    {
        Destroy(pauseMenuInstance);
        Time.timeScale = 1;
    }
}
