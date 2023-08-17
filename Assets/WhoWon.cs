using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WhoWon : MonoBehaviour
{

    public int whoWon;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        text = FindObjectOfType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(whoWon == null) {
            text.text = "\n\n press Enter to start the game!";
        }
        else if(whoWon == 0)
        {
            text.text = "Player left won! \n press Enter to start the game!";
        }
        else
        {
            text.text = "Player right won! \n press Enter to start the game!";
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            print(whoWon);
        }
    }

    
}
