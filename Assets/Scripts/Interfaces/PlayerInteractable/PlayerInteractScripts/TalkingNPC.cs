using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class TalkingNPC : MonoBehaviour, IPlayerInteract
{

        public void PlayerInteract()
        {
                transform.Find("DialogueSummoner").GetComponent<NpcTextBox>().DialogueStart();
        }
}
