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
    public GameObject player2UI;

    private List<string> currJoysticks;

    private int numJoysticks = 0;

    [SerializeField]
    private int numPlayers = 1;

    private void Start()
    {
        StartCoroutine(CheckInputMethods());
        currJoysticks = new List<string>();
    }

    private void OnEnable()
    {
        PlayerCollision.ThisPlayerDeath -= HandlePlayerDeath;
        PlayerCollision.ThisPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerCollision.ThisPlayerDeath -= HandlePlayerDeath;
    }

    void Update()
    {
        //if we connect a new joystick or press the join key instantiate player 2 at the middle of the arena
        if (!player2InGame && (Input.GetJoystickNames().Length > 1 || Input.GetKeyDown(KeyCode.Return)))
        {
            player2 = Instantiate(player2Prefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            player2.GetComponent<PlayerMovement>().playerNum = 2;
            mainCamera.transform.parent = null;
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z - 1f);
            player2.GetComponent<PlayerMovement>().cam = mainCamera;
            player2UI.SetActive(true);
            spawnMgr.FindPlayers();
            player2InGame = true;
            numPlayers++;
        }

        if (numPlayers == 2 && mainCamera.orthographicSize <= 83)
        {
            mainCamera.orthographicSize += 1f;
        }
    }

    private IEnumerator CheckInputMethods()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1.5f);
            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                //if the name of the joystick isn't empty and wasn't already plugged in, we have one to add.
                if (!string.IsNullOrEmpty(Input.GetJoystickNames()[i]) && !currJoysticks.Contains(Input.GetJoystickNames()[i]))
                {
                    //joystick connected
                    print("connected");
                    currJoysticks.Add(Input.GetJoystickNames()[i]);
                    numJoysticks++;
                }
                //if it is empty, and it was in the list of currently plugged in ones
                //else if(string.IsNullOrEmpty(Input.GetJoystickNames()[i]) && !string.IsNullOrEmpty(currJoysticks[i]) && numJoysticks > 0)
                //{
                //    //joystick disconnected
                //    print("disconnected");
                //    currJoysticks.Remove(Input.GetJoystickNames()[i]);
                //    numJoysticks--;
                //}
            }
            if(player1 != null)
                player1.GetComponent<PlayerMovement>().numJoysticks = numJoysticks;
            if (numPlayers == 2)
                player2.GetComponent<PlayerMovement>().numJoysticks = numJoysticks;
        }
    }

    //private void AdaptCamera()
    //{
    //    float distBtwn = Vector2.Distance(player1.transform.position, player2.transform.position);
    //    //float halfPoint = distBtwn / 2;
    //    //double check math later
    //    //Vector3 cameraTransform = mainCamera.transform.position;
    //    if (distBtwn > 50 && mainCamera.orthographicSize < 83)
    //        mainCamera.orthographicSize += 1f;
    //    else if (distBtwn <= 50 && mainCamera.orthographicSize > 40)
    //        mainCamera.orthographicSize -= 1f;
    //    //might need to zoom this out at some point too
    //}

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
