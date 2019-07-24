using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaGameController : MonoBehaviour
{
    public GameObject PauseMenu;
    public Text text;
    public CharacterBase Player;
    public CharacterBase Enemy;

    private bool isPause;
    private bool isFinish;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPause && !isFinish)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;


        if (!isFinish)
        {
            if (Input.GetKeyDown(KeyCode.P))
                isPause = !isPause;
        }

        if (Player.hp == 0 || Enemy.hp == 0)
        {
            isFinish = true;
            isPause = true;
            if (Player.hp == 0)
            {
                text.text = "You lose";
            }
            if (Enemy.hp == 0)
            {
                text.text = "You win";
            }
        }

        PauseMenu.SetActive(isPause);

    }
}
