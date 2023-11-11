using System;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NpcTextBox : MonoBehaviour
    {
        private GameObject _thisCanvas;
        [Header("Panel 0: Only Text")] 
        [Header("Panel 1: Text and Image")]
        [SerializeField] private GameObject[] panelPrefab;
        [SerializeField] private Sprite npcImg;
        [SerializeField] private string[] npcName;
        [SerializeField] private GameObject[] cameraFocus;
        [SerializeField] private string[] textOnPanel;
        private int _currentText;
        [SerializeField] private Sprite[] imgOnPanel;
        private int _currentImg;
        private Button _prev,_next;

        private CinemachineVirtualCamera _cam;
            
        [Header("Order the type of panels appear in, write only numbers (0 or 1)")] 
        [SerializeField] private string panelsToSpawn;
        private int[] _order;
        [Header("Disables movement of playerColor and camera")]
        [SerializeField] private TempDisableMovement  tempDisableMovement;
    
        private int _sumPages;
        private int _currentPage;
        private int _currentName;
        private GameObject _thisPage;
        
       
        private Transform cameraTarget;

        private bool _activeDialogue;

        private void Start()
        {
            cameraTarget = GameObject.FindGameObjectWithTag("Player").transform.Find("CameraTarget").transform;
        }

        public void DialogueStart()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            _activeDialogue = true;
            //Finds Camera In Scene so that it can swap focus during scenes  
            _cam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        
            
            _thisCanvas = GameObject.Find("CanvasCrayon");
            _currentText = 0;
            _currentImg = 0;
            _currentPage = 0;
            _sumPages = textOnPanel.Length;
            _order = new int[panelsToSpawn.Length];
            for (int  i = 0;  i < panelsToSpawn.Length;  i++)
            {
                _order[i] = int.Parse(panelsToSpawn[i].ToString());
            }
        
            tempDisableMovement.OnPauseGame(_order[0]!=2);
            DialogueContinue();
        }
        private void DialogueContinue()
        {
            if (panelPrefab.Length > 0)
            {
                _thisPage = Instantiate(panelPrefab[_order[_currentPage]], _thisCanvas.transform.position, 
                    Quaternion.identity, _thisCanvas.transform);
                //Panel with only text
                if (_order[_currentPage] == 0)
                {
                    _thisPage.transform.Find("Profile").GetComponent<Image>().sprite = npcImg;
                    _thisPage.transform.Find("Profile/Name").GetComponent<TextMeshProUGUI>().text = npcName[0];
                    _thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[_currentText];
                } 
                //Panel with text and Image
                else if (_order[_currentPage] == 1)
                {
                    _thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[_currentText];
                    _thisPage.transform.Find("MainImage").GetComponent<Image>().sprite = imgOnPanel[_currentImg];
                }
                //Smaller panel for quick and shorter dialogue
                else if (_order[_currentPage] == 2)
                {
                    _thisPage.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
                    _thisPage.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                    _thisPage.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = npcName[_currentText];
                    _thisPage.transform.Find("MainText").GetComponent<TextMeshProUGUI>().text = textOnPanel[_currentText];
                    _cam.LookAt = cameraFocus[_currentText].transform;
                    _cam.Follow = cameraFocus[_currentText].transform;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    return;
                }
            }
            //Find New Buttons
            _prev = _thisPage.transform.Find("Prev").GetComponent<Button>();
            _next = _thisPage.transform.Find("Next").GetComponent<Button>();
            // Add new listeners
            _prev.onClick.AddListener(PrevPanel);
            _next.onClick.AddListener(NextPanel);
        
            if (_currentPage == 0)
            {
                _prev.interactable = false; 
            }
            if (_currentPage + 1 == _sumPages)
            {
                _next.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Exit";
            }
        }

        private void Update()
        {
            if (_activeDialogue)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DialogueEnd();
                }

                if (Input.GetButtonDown("Interact") && _order[_currentPage] == 2)
                {
                    NextPanel();
                }
            }
        }

        private void DialogueEnd()
        {
            print("Ended Dialogue");
            Destroy(_thisPage);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _activeDialogue = false;
            
            tempDisableMovement.OnResumeGame();
        
            //Set so player is the focus of camera
            _cam.LookAt = cameraTarget;
            _cam.Follow = cameraTarget;
        }
        private void PrevPanel()
        { 
            SwapPanel(-1);
        }
        private void NextPanel()
        {
            SwapPanel(1);
        }

        private void SwapPanel(int dir)
        {
            //Only if image was used
            if (_currentPage != 0)
            {
                if ((_order[_currentPage] == 1 && dir == 1)||(_order[_currentPage-1] == 1 && dir == -1))
                {
                    _currentImg += dir;
                }
            }
        
            // Because dialogue panel has no button it will ignore this part 
            if (_order[_currentPage] != 2)
            {
                // Remove previous listeners
                _prev.onClick.RemoveAllListeners();
                _next.onClick.RemoveAllListeners();
            }
            _currentText += dir;
            _currentPage += dir;
        
        
        
            if (_currentPage == _sumPages)
            {
                DialogueEnd();
                print("DialogueEndCaled");
            } else {
                Destroy(_thisPage);
                DialogueContinue();
            }
        }
    }
}
