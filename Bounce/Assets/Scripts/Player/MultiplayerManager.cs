using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviour
{
    public GameObject player2Prefab;
    public SpawnerManager spawnMgr;
    public Camera mainCamera;
    private bool player2InGame = false;
    [SerializeField]
    GameObject player1;
    [SerializeField]
    GameObject player2;

    private int numPlayers = 1;

    private void OnEnable()
    {
        PlayerCollision.thisPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerCollision.thisPlayerDeath -= HandlePlayerDeath;
    }

    void Update()
    {
        //if we connect a new joystick instantiate player 2 at the middle of the arena
        if (Input.GetJoystickNames().Length > 1 && !player2InGame)
        {
            player2 = Instantiate(player2Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            player2.GetComponent<PlayerMovement>().playerNum = 2;
            spawnMgr.FindPlayers();
            player2InGame = true;
            numPlayers++;
        }

        if(player2InGame)
        {
            AdaptCamera();
        }
    }

    private void AdaptCamera()
    {
        float distBtwn = Vector2.Distance(player1.transform.position, player2.transform.position);
        float halfPoint = distBtwn / 2;
        //double check math later
        Vector2 cameraTransform = (player1.transform.position - player2.transform.position).normalized * halfPoint;
        mainCamera.transform.position = cameraTransform;
    }

    private void HandlePlayerDeath()
    {
        numPlayers--;
        if (numPlayers <= 0)
            SceneManager.LoadScene(5);
    }
}
