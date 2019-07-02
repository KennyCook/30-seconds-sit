using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets;

namespace SitOrDie
{
    public enum DisplayPanel
    {
        HUD, PAUSE, WELCOME, GAMEOVER, VICTORY
    }

    public class GameController : MonoBehaviour
    {
        public GameObject Player;
        public GameObject Chair;
        public GameObject Laser;
        public GameObject GlobeLightPrefab;
        public GameObject[] Lights;

        public AudioSource ChairNoise;

        public GameObject PanelWelcome;
        public GameObject PanelHUD;
        public GameObject PanelPause;
        public GameObject PanelGameover;
        public GameObject PanelVictory;
        public Text TimerText;
        public Text PieceText;
        public Text CompTimeText;

        private float _timer, _winTime;
        private int _chairProgress;
        private bool _isPaused, _isWelcome, _isGameOver;

        private void Start()
        {
            //_timerText = uiMain.transform.
            //UnpauseGame();
            ShowWelcomeMenu();

            // Init Values
            _timer = 30f;
            _winTime = 0f;
            _chairProgress = 1;

            _isPaused = _isGameOver = false;
            AudioListener.pause = false;

            // Lights intensity and mat emission
            GlobeLightPrefab.transform.GetChild(0).GetComponent<Light>().intensity = 1.5f;
            Lights[0].GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", Color.white * 1f);
        }

        private void Update()
        {
            GetInput();
            UpdateTimer();

            if (_timer <= 5f && _timer > 4f)
            {
                foreach (var l in Lights)
                {
                    l.transform.GetChild(0).GetComponent<Light>().intensity = 0.5f;
                }

                Lights[0].GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", Color.white * 0.3f);
            }

            if (_timer <= 0 && !Laser.activeSelf)
            {
                ActivateLaser();
            }
        }

        public bool IsChairComplete()
        {
            if (_chairProgress == 6)
            {
                return true;
            }

            return false;
        }

        public void AddChairPiece()
        {
            ChairNoise.Play();
            _chairProgress += 1;
            PieceText.text = System.String.Format("{0}/6", _chairProgress);
        }

        public bool IsGamePaused() { return _isPaused; }

        public void OnLaserCollide()
        {
            _isGameOver = true;

            // UI
            PanelHUD.SetActive(false);
            PanelGameover.SetActive(true);

            // Player
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Player.GetComponentInChildren<HandController>().enabled = false;
        }

        public void Sit()
        {
            // store completion time
            //CompTimeText.text = System.String.Format("Completion Time: {0} seconds", (30f - _timer).ToString("F"));
            CompTimeText.text = System.String.Format("Completion Time: {0} seconds", _winTime.ToString("F"));

            // turn player collider off
            Player.GetComponent<CharacterController>().enabled = false;
            // set player rb to kinematic
            Player.GetComponent<Rigidbody>().useGravity = false;
            // move player down and rotate toward the back
            Player.transform.SetPositionAndRotation(new Vector3(Chair.transform.position.x, 0.5f, Chair.transform.position.z), new Quaternion(0, 180, 0, 0));
            Camera.main.transform.rotation = new Quaternion(0, 0, 0, 0);
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Player.GetComponentInChildren<HandController>().enabled = false;

            // show vicotry screen
            _isGameOver = true;
            PanelVictory.SetActive(true);
            PanelHUD.SetActive(false);
        }

        private void GetInput()
        {
            if (_isWelcome)
            {
                if (Input.anyKeyDown)
                {
                    BeginGame();
                    return;
                }
            }

            if (_isGameOver)
            {
                if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("Game Scene", LoadSceneMode.Single); }
                if (Input.GetKeyDown(KeyCode.Q)) { SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single); }
                return;
            }

            if (_isPaused)
            {
                if (Input.GetKeyDown(KeyCode.P)) { UnpauseGame(); }
                if (Input.GetKeyDown(KeyCode.R)) { SceneManager.LoadScene("Game Scene", LoadSceneMode.Single); }
                if (Input.GetKeyDown(KeyCode.Q)) { SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single); }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.P)) { PauseGame(); }
            }
        }

        private void ShowWelcomeMenu()
        {
            _isWelcome = true;
            Time.timeScale = 0;

            PanelHUD.SetActive(false);
            PanelWelcome.SetActive(true);

            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Player.GetComponentInChildren<HandController>().enabled = false;
        }

        private void BeginGame()
        {
            _isWelcome = false;
            Time.timeScale = 1;

            PanelWelcome.SetActive(false);
            PanelHUD.SetActive(true);

            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            Player.GetComponentInChildren<HandController>().enabled = true;
        }

        private void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0;

            // UI
            PanelHUD.SetActive(false);
            PanelPause.SetActive(true);

            // Player
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Player.GetComponentInChildren<HandController>().enabled = false;
            AudioListener.pause = true;
        }

        private void UnpauseGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            // UI
            PanelHUD.SetActive(true);
            PanelPause.SetActive(false);

            // Player
            Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            Player.GetComponentInChildren<HandController>().enabled = true;
            AudioListener.pause = false;
        }

        private void UpdateTimer()
        {
            _timer = _timer - Time.deltaTime > 0f ? _timer - Time.deltaTime : 0f;
            TimerText.text = _timer.ToString("F");

            if (_timer <= 10f)
            {
                TimerText.color = Color.red;
            }

            _winTime += Time.deltaTime;
        } 

        private void ActivateLaser()
        {
            Laser.SetActive(true);
            // Reduce light intensity 
                // maybe do this at 5 seconds left?
            //foreach (var l in Lights)
            //{
            //    l.transform.GetChild(0).GetComponent<Light>().intensity = 0.5f;
            //}

            // Reduce material emission intensity
            //var mats = GlobeLightPrefab.GetComponent<MeshRenderer>();
            //foreach (var l in Lights)
            //{
                //var renderer = Lights[0].GetComponent<MeshRenderer>();
                //DynamicGI.SetEmissive(renderer, Color.white * 0.5f);
            //}
        }
    }
}