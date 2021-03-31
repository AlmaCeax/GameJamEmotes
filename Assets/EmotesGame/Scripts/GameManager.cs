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

    internal void RespawnPlayer(Player player, Robot robot)
    {
        int spawnIndex = (int)player.CustomProperties["spawnIndex"];
        robot.GetComponent<SyncTransform>().FlagTeleport();
        robot.movement.controller.enabled = false;
        robot.transform.position = spawnPoints[player.ActorNumber - 1][spawnIndex].position;
        robot.movement.controller.enabled = true;
    }
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

    #endregion


    private void Awake()
    {
        Instance = this;

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
            {EmotesGame.PLAYER_LOADED_LEVEL, true}
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

                int spawnIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["spawnIndex"];
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject localObject = PhotonNetwork.Instantiate(this.playerPrefabs[PhotonNetwork.LocalPlayer.ActorNumber - 1].name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1][spawnIndex].position, Quaternion.identity, 0);
                //vCam.LookAt = localObject.transform;
                if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
                    vCam.transform.rotation = Quaternion.Euler( 10, 161.744f, 0);
                vCam.Follow = localObject.transform;
                vCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_Path = paths[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            }
           /* else
            {

                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {

        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        /*if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LIVES))
        {
            CheckEndOfGame();
            return;
        }*/

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

    #endregion
}
