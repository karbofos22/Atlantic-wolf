using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3 leftSpawnPos = new(400, 23, 420f);

    public GameObject[] targets;

    public GameObject[] spawnPos;

    //[HideInInspector]
    public bool isLeftSpawnAllowed;
    public bool isRightSpawnAllowed;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        isLeftSpawnAllowed = true;
        isRightSpawnAllowed = true;
        gameManager = GameObject.Find("Game manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            LeftSpawner();
            RightSpawner();
        }
    }
    void LeftSpawner()
    {
        if (GameObject.FindGameObjectWithTag("LeftTarget") == null && isLeftSpawnAllowed)
        {
            Instantiate(targets[Random.Range(0, 2)], spawnPos[0].transform.position, spawnPos[0].transform.rotation);
        }
    }
    void RightSpawner()
    {
        if (GameObject.FindGameObjectWithTag("RightTarget") == null && isRightSpawnAllowed)
        {
            Instantiate(targets[Random.Range(2, 4)], spawnPos[1].transform.position, spawnPos[1].transform.rotation);
        }
    }
}
