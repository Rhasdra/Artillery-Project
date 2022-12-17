using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject controlsPrefab;
    GameObject controlsInstance;
    

    public void ResumeGame()
    {
        Destroy(this.gameObject);
        Time.timeScale = 1;
    }

    public void ShowControls()
    {
        if(controlsInstance == null)
        {
            controlsInstance = Instantiate(controlsPrefab, transform.position, Quaternion.identity);
            controlsInstance.transform.SetParent(this.transform, true);
        }    
        controlsInstance.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Game Should Close!");
        Application.Quit();
    }
}
