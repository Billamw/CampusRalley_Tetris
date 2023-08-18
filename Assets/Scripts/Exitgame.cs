using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exitgame : MonoBehaviour
{
    private bool ispaused;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        ispaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !ispaused)
        {
            ispaused = true;
            Time.timeScale = 0;
            canvas.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && ispaused)
        {
            ispaused = false;
            Time.timeScale = 1;
            canvas.SetActive(false);
        }



    }

    public void LoadStart()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void DestroyIt()
    {
        Application.Quit();
    }
}
