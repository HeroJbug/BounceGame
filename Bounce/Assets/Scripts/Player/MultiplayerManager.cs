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
    public GameObject Player2UI;

    private int numJoysticks = 0;

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
        //if we connect a new joystick or press the join key instantiate player 2 at the middle of the arena
        if (!player2InGame && (numJoysticks == 2 || Input.GetKeyDown(KeyCode.Return)))
        {
            player2 = Instantiate(player2Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            player2.GetComponent<PlayerMovement>().playerNum = 2;
            mainCamera.transform.parent = null;
            spawnMgr.FindPlayers();
            Player2UI.SetActive(true);
            player2InGame = true;
            numPlayers++;
        }

        if (player2InGame)
        {
            AdaptCamera();
        }
    }

    private IEnumerator CheckInputMethods()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1.5f);
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                if (!string.IsNullOrEmpty(Input.GetJoystickNames()[i]))
                {
                    //joystick connected
                    numJoysticks++;
                }
                else
                {
                    //joystick disconnected
                    numJoysticks--;
                }
            }
            player1.GetComponent<PlayerMovement>().numJoysticks = numJoysticks;
            if (player2InGame)
                player2.GetComponent<PlayerMovement>().numJoysticks = numJoysticks;
        }
    }

    private void AdaptCamera()
    {
        float distBtwn = Vector2.Distance(player1.transform.position, player2.transform.position);
        float halfPoint = distBtwn / 2;
        //double check math later
        Vector2 cameraTransform = (player1.transform.position - player2.transform.position).normalized * halfPoint;
        mainCamera.transform.position = cameraTransform;
        //might need to zoom this out at some point too
    }

    public int GetNumJoysticksConnected()
    {
        return numJoysticks;
    }

    private void HandlePlayerDeath()
    {
        numPlayers--;
        if (numPlayers <= 0)
            SceneManager.LoadScene(5);
    }
}
