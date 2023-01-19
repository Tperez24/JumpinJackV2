using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMenuCreator : MonoBehaviour
{
    public List<GameObject> gameObjectsToHide;
    public PlayerInputManager creator;
    public Transform canvas;

    public Transform player1, player2;
    private InputAction _start;
    private int _playerIndex;
    private void Start()
    {
        SuscribeInputs();
    }

    private void OnDisable()
    {
        UnsuscribeInputs();
    }

    private void SuscribeInputs()
    {
        creator.onPlayerJoined += Join;
    }

    private void UnsuscribeInputs()
    {
        creator.onPlayerJoined -= Join;
    }

    private void Join(PlayerInput obj)
    {
        if (creator.playerCount == 1)
        {
            obj.GetComponent<Image>().color = Color.blue;
            obj.transform.SetParent(canvas); 
            obj.gameObject.transform.position = player1.position;
        }

        if (creator.playerCount == 2)
        {
            obj.GetComponent<Image>().color = Color.red;
            obj.transform.SetParent(canvas);
            obj.gameObject.transform.position = player2.position;
            
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        Debug.Log("FadeScreen");
        var newScene = SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        do
        {
            yield return null;
        } while (newScene.progress != 1);
        
        var newCreator = GameObject.Find("Creator").GetComponent<Creator>();

        newCreator.Initialize();
     
        foreach (var go in gameObjectsToHide)
        {
            go.SetActive(false);
        }

       
        
    }
}
