using System;
using System.Collections;
using System.Collections.Generic;
using Others;
using PlayerComponents;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace UI
{
    public class CanvasManager : MonoBehaviour
    {
        public bool red, blue;
        
        private GameObject redWins;
        private GameObject blueWins;
        private TextMeshProUGUI textBlue;
        private TextMeshProUGUI textRed;
        
        public TextMeshProUGUI timer;
        public GameData data;
        public string losser;
    
        private float _targetTime;
        private bool _stopTimer;

        public List<GameObject> redParticles;
        public List<Images> redImages;
        public List<GameObject> blueParticles;
        public List<Images> blueImages;

        private void Start()
        {
            _targetTime = data.gameTime;
            Player.OnLifeLost.AddListener(RemoveHp);
        
            DontDestroyOnLoad(gameObject);
        }

        private void RemoveHp(string player,int life)
        {
            if (player == "Blue")
            {
                blueParticles[life].SetActive(true);

                Destroy(blueImages[life].inside);
                Destroy(blueImages[life].outside);
            }
            else
            {
                redParticles[life].SetActive(true);
                Destroy(redImages[life].inside);
                Destroy(redImages[life].outside);
            }

            if (life == 2)
            {
                TimerEnded();
                losser = player;
            }
        }

        private void Update()
        {
            if(_stopTimer) return;
            _targetTime -= Time.deltaTime;

            string minutes = Math.Floor((int) _targetTime / 60f).ToString();
            string sec = Mathf.Ceil(_targetTime % 60).ToString();

            if (sec.Length == 1) sec = "0" + sec;
            if (minutes.Length == 1) minutes = "0" + minutes;
        
            if (_targetTime <= 0.0f)
            {
                TimerEnded();
            }

            timer.text = minutes + " : " + sec;
        }
 
        private void TimerEnded()
        {
            _stopTimer = true;
            Time.timeScale = 0.25f;
            StartCoroutine(EndGame());
            //do your stuff here.
        }

        private IEnumerator EndGame()
        {
            yield return new WaitForSecondsRealtime(5);
            Time.timeScale = 1f;
            var newScene = SceneManager.LoadSceneAsync(sceneBuildIndex: 3, LoadSceneMode.Single);
            
            do
            {
                yield return null;
            } while (newScene.progress != 1);

            var input = gameObject.AddComponent<PlayerInputManager>();
            input.joinBehavior = PlayerJoinBehavior.JoinPlayersWhenButtonIsPressed;
            input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            input.playerPrefab = Resources.Load<GameObject>("ControllerOnWin");

            Initialize();
        }

        private void Initialize()
        {
            blueWins = GameObject.Find("BlueBackground");
            redWins = GameObject.Find("RedBackground");

            if (losser == "Blue")
            {
                redWins.SetActive(true);
                blueWins.SetActive(false);
                //blueWins.GetComponentInChildren<TextMeshProUGUI>().text = data.winText[Random.Range(0, data.winText.Count)];
            }
            else
            {
                blueWins.SetActive(true);
                redWins.SetActive(false);
                /*textRed = redWins.GetComponentInChildren<TextMeshProUGUI>().text;
                textRed.text = data.winText[Random.Range(0, data.winText.Count)];*/
            }
        }
    }

    [Serializable]
    public class Images
    {
        public GameObject outside;
        public GameObject inside;
    }
}