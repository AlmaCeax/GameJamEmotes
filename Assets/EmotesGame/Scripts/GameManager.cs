using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    static public GameManager Instance;
    #endregion

    #region Private Fields
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject[] playerPrefabs;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if (playerPrefabs == null)
        { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

            Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            //if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate(this.playerPrefabs[PhotonNetwork.LocalPlayer.ActorNumber - 1].name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
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
}
