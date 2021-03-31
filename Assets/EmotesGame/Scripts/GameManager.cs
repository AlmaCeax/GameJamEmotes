using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.Simple;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using Cinemachine;
using System;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    static public GameManager Instance;
    public Robot player1 = null;
    public Robot player2 = null;
    #endregion

    #region Private Fields
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject[] playerPrefabs;

    [Tooltip("The paths of each player camera")]
    [SerializeField]
    private CinemachineSmoothPath[] paths;

    [Tooltip("The spawn points of each player")]
    [SerializeField]
    private Transform[] spawnParents;
    private Transform[][] spawnPoints;

    [Tooltip("Virtual Camera")]
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    bool finishing = false;
    #endregion


    private void Awake()
    {
        Instance = this;
        finishing = false;

        spawnPoints = new Transform[2][];
        spawnPoints[0] = new Transform[spawnParents[0].childCount];
        spawnPoints[1] = new Transform[spawnParents[1].childCount];
        for (int i = 0; i < spawnPoints[0].Length; ++i)
        {
            spawnPoints[0][i] = spawnParents[0].GetChild(i);
        }
        for (int i = 0; i < spawnPoints[1].Length; ++i)
        {
            spawnPoints[1][i] = spawnParents[1].GetChild(i);
        }  
    }
    // Start is called before the first frame update
    void Start()
    {
        Hashtable props = new Hashtable
        {
            {EmotesGame.PLAYER_LOADED_LEVEL, true},
            {EmotesGame.PLAYER_REACHED_END, false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);


        if (playerPrefabs == null)
        { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                int spawnIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties[EmotesGame.PLAYER_SPAWN_INDEX];
                int playerNumber = (int)PhotonNetwork.LocalPlayer.CustomProperties[EmotesGame.PLAYER_NUMBER];
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject localObject = PhotonNetwork.Instantiate(this.playerPrefabs[playerNumber].name, spawnPoints[playerNumber][spawnIndex].position, Quaternion.identity, 0);
                //vCam.LookAt = localObject.transform;
                if (playerNumber == 1)
                    vCam.transform.rotation = Quaternion.Euler( 10, 161.744f, 0);

                vCam.Follow = localObject.transform;
                vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = paths[playerNumber];
            }
           /* else
            {

                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }*/
        }
    }
    internal void RespawnPlayer(Player player, Robot robot)
    {
        int spawnIndex = (int)player.CustomProperties[EmotesGame.PLAYER_SPAWN_INDEX];
        int playerNumber = (int)player.CustomProperties[EmotesGame.PLAYER_NUMBER];

        robot.GetComponent<SyncTransform>().FlagTeleport();
        robot.movement.controller.enabled = false;
        robot.transform.position = spawnPoints[playerNumber][spawnIndex].position;
        robot.movement.controller.enabled = true;
    }

    private bool CheckAllPlayerLoadedLevel()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLoadedLevel;

            if (p.CustomProperties.TryGetValue(EmotesGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
            {
                if ((bool)playerLoadedLevel)
                {
                    continue;
                }
            }

            return false;
        }

        return true;
    }

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Lobby");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {

        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(EmotesGame.PLAYER_REACHED_END))
        {
            CheckEndOfGame();
            return;
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
        /*int startTimestamp;
        bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

        if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LOADED_LEVEL))
        {
            if (CheckAllPlayerLoadedLevel())
            {
                if (!startTimeIsSet)
                {
                    CountdownTimer.SetStartTime();
                }
            }
            else
            {
                // not all players loaded yet. wait:
                Debug.Log("setting text waiting for players! ", this.InfoText);
                InfoText.text = "Waiting for other players...";
            }
        }*/
    }

    private void CheckEndOfGame()
    {
        if (finishing)
            return;

        bool allReached = true;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object hasReached;
            if (p.CustomProperties.TryGetValue(EmotesGame.PLAYER_REACHED_END, out hasReached))
            {
                if (!(bool)hasReached)
                {
                    allReached = false;
                    break;
                }
            }
        }

        if (allReached)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            StartCoroutine(EndOfGame());
        }
    }

    private IEnumerator EndOfGame()
    {
        float timer = 5.0f;
        finishing = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { EmotesGame.PLAYER_SPAWN_INDEX, 0 } });
        while (timer > 0.0f)
        {
            //InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));
            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            object obj;
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(EmotesGame.CURRENT_LEVEL, out obj);
            int currentLevel = (int)obj;
            currentLevel++;

            Hashtable props = new Hashtable
            {
                {EmotesGame.CURRENT_LEVEL, currentLevel}
            };

            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            PhotonNetwork.LoadLevel(currentLevel);
        }
    }

    internal void OnReachFinishLine(Player localPlayer)
    {
        Hashtable props = new Hashtable
        {
            {EmotesGame.PLAYER_REACHED_END, true}
        };
        localPlayer.SetCustomProperties(props);
    }

    internal void OnLeaveFinishLine(Player localPlayer)
    {
        Hashtable props = new Hashtable
        {
            {EmotesGame.PLAYER_REACHED_END, false}
        };
        localPlayer.SetCustomProperties(props);
    }

    #endregion
}
