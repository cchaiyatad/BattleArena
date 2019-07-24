using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DungeonSceneController : MonoBehaviour
{
    public GameObject PauseMenu;
    public Text text;
    public CharacterBase Player;
    public CharacterBase Enemy;
    public int count;

    private bool isPause;
    private bool isFinish;
    private List<Vector3> corners = new List<Vector3> { };
    private float nextSpawnTime;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 size = GameObject.Find("Plane").GetComponent<Renderer>().bounds.size - (2 * Vector3.one);

        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                corners.Add(new Vector3(size.x / 2 * i, 0f, size.z / 2 * j));
            }
        }
		nextSpawnTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {

        if (nextSpawnTime < Time.time && !isPause && !isFinish)
        {
            nextSpawnTime += 12f;
            Instantiate(Enemy, corners[Random.Range(0, 4)], Quaternion.identity);
        }

        if (isPause & !isFinish)
            Time.timeScale = 0f;

        else
            Time.timeScale = 1f;


        if (!isFinish)
        {
            if (Input.GetKeyDown(KeyCode.P))
                isPause = !isPause;
        }

        if (Player.hp == 0)
        {
            isFinish = true;
            isPause = true;
            if (Player.hp == 0)
            {
                text.text = "Kill: " + count;
            }
        }

        PauseMenu.SetActive(isPause);

    }
}
