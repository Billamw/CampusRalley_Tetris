using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WhoWon : MonoBehaviour
{
    public static WhoWon Instance { get; private set; }

    public int whoWon = 2;
    public TextMeshProUGUI text;

    void Awake()
    {
        // Maintain singleton instance across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        text = FindObjectOfType<TextMeshProUGUI>();

    }



    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            text = FindObjectOfType<TextMeshProUGUI>();
        }

        if (whoWon == 2) {
            text.text = "\n\n press Enter to start the game!";
        }
        else if(whoWon == 0)
        {
            text.text = "Player left lost! \n press Enter to start the game!";
        }
        else
        {
            text.text = "Player right lost! \n press Enter to start the game!";
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }


    }



    
}
