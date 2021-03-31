﻿using UnityEngine;
public class EmotesGame
{
    public const float PLAYER_RESPAWN_TIME = 4.0f;
    public const int PLAYER_MAX_LIVES = 3;

    public const string PLAYER_LIVES = "PlayerLives";
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
    public const string PLAYER_SPAWN_INDEX = "spawnIndex";
    public const string PLAYER_REACHED_END = "reachedEnd";
    public const string CURRENT_LEVEL = "currentLevel";
    public const string PLAYER_NUMBER = "playerNumber";

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 0: return Color.red;
            case 1: return Color.green;
            case 2: return Color.blue;
            case 3: return Color.yellow;
            case 4: return Color.cyan;
            case 5: return Color.grey;
            case 6: return Color.magenta;
            case 7: return Color.white;
        }

        return Color.black;
    }
}