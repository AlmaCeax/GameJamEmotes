using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Robot robot = other.GetComponent<Robot>();
        if(robot != null && robot.pView.IsMine)
        {
            Hashtable props = new Hashtable
            {
                {EmotesGame.PLAYER_SPAWN_INDEX, transform.GetSiblingIndex()}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }    
    }
}
