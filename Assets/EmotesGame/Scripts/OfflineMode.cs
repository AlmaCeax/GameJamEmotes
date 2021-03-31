using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMode : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Awake()
    {
        PhotonNetwork.OfflineMode = true;
    }

    private void OnConnectedToServer()
    {
        PhotonNetwork.CreateRoom("OfflineRoom");
    }

}
