using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoBehavior : MonoBehaviour
{
    private GameObject indicator;

    private GameManager gameManager;
    private PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.GetChild(0).gameObject;
        gameManager = GameObject.Find("Game manager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke(nameof(SetIndicatorActive), 5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("LeftTarget") || collision.gameObject.CompareTag("RightTarget") || collision.gameObject.CompareTag("DestroyPanel"))
        {
            Destroy(gameObject);
            playerController.projectileCount--;
        }
    }
    void SetIndicatorActive()
    {
        indicator.SetActive(true);
    }
}
