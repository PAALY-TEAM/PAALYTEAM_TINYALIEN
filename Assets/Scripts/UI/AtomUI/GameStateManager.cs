using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityAtoms;
using UnityAtoms.BaseAtoms;

//The global Game State Manager
public class GameStateManager : MonoBehaviour{
    
    [SerializeField]
    private GameStateVariable GlobalGameState;

    [SerializeField]   
    private GameStateEvent GlobalGameStateChangedEvent;

    private void GameStateChange(GameState newState){
        switch(newState){
            case GameState.GamePlay:
                Debug.Log("GameStateManager GamePlay");

            break;
            case GameState.MainMenu:
                Debug.Log("GameStateManager MainMenu");


            break;
        }
    }

    void Awake(){
        GlobalGameStateChangedEvent.Register(this.GameStateChange);
        Debug.Log("GameStateManager Awake");
        
    }

    void OnDestroy(){
        GlobalGameStateChangedEvent.Unregister(this.GameStateChange);
        Debug.Log("GameStateManager OnDestroy");
    }
}

