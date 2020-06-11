using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour
{
    #region variables
    [Header("UI References")]
    public GameObject dialogueBox;
    public Text dialogueText;
    public Text nameText;
    public Button advanceButton;
    public Button response1;
    public Button response2;

    [Header("Dialogue Control")]
    public Dialogue currentDialogue;
    public NPC currentNPC;
    // 1 / (textspeed * 100) is the amount of seconds between each character
    public float textSpeed = 10;
    public bool choice;
    public static bool inShop;
    bool showDialogue;
    bool canContinue;
    int index;

    PlayerControl player;
    #endregion

    #region start
    private void Start()
    {
        player = GetComponent<PlayerControl>();
        CloseDialogueBox();
    }
    #endregion

    #region start dialogue
    public void StartDialogue(Dialogue newDialogue, NPC newNPC)
    {
        //open the dialogue box
        if (!showDialogue)
        {
            OpenDialogueBox();
        }

        //set which npc is showing dialogue
        currentNPC = newNPC;

        //set the dialogue being shown and the index
        currentDialogue = newDialogue;
        index = 0;
        //write out the dialogue
        StartCoroutine(DisplayText(currentDialogue.dialogue[index].name, currentDialogue.dialogue[index].dialogue));

        
    }
    #endregion

    #region advance dialogue
    public void AdvanceDialogue()
    {
        //cancel if text is still writing
        if (!canContinue)
        {
            return;
        }

        //increase index and finish if at end
        index += 1;
        if (index == currentDialogue.dialogue.Length)
        {
            CloseDialogueBox();
            return;
        }
        //otherwise write the next part of the dialogue
        else
        {
            StartCoroutine(DisplayText(currentDialogue.dialogue[index].name, currentDialogue.dialogue[index].dialogue));
        }
    }
    #endregion

    #region display text
    public IEnumerator DisplayText(string speakerName, string text)
    {
        //prevent continuing while dialogue is being written
        canContinue = false;
        choice = false;
        advanceButton.gameObject.SetActive(false);
        //hide responses
        response1.gameObject.SetActive(false);
        response2.gameObject.SetActive(false);
        //reset the text element
        dialogueText.text = "";
        //set the name of the speaker, if it's the player use their name
        if (speakerName == "Player")
        {
            nameText.text = player.playerName;
        }
        else
        {
            nameText.text = speakerName;
        }

        //create an array of characters of the text that needs displaying
        char[] charArray = text.ToCharArray();

        //add the characters from the array one at a time to the screen
        for (int i = 0; i < charArray.Length; i++)
        {
            dialogueText.text += charArray[i];
            yield return new WaitForSecondsRealtime(1 / (textSpeed * 100));
        }

        #region set data
        if (currentDialogue.dialogue[index].setData)
        {
            switch (currentDialogue.dialogue[index].dataSet.dataType)
            {
                case DataType.useDataIndex:
                {
                    //set current npcs data of index defined in dialogue to value defined in dialogue
                    currentNPC.data[currentDialogue.dialogue[index].dataSet.dataIndex] = currentDialogue.dialogue[index].dataSet.value;
                    break;
                
                }
                case DataType.buyPriceMultiplier:
                {
                    //set buy price multiplier of npc shop, if it exists
                    Storage NPCShop = currentNPC.GetComponent<Storage>();
                    if (NPCShop != null)
                    {
                        NPCShop.buyMultiplier = currentDialogue.dialogue[index].dataSet.value;
                    }
                    break;
                }
               

            }
        }
    
            
        #endregion

        #region set up responses
        //if multiple options set responses
        if (currentDialogue.dialogue[index].response)
        {
            //start a choice, show buttons
            choice = true;
            response1.gameObject.SetActive(true);
            response2.gameObject.SetActive(true);

            //set button text to choice options
            response1.GetComponentInChildren<Text>().text = currentDialogue.dialogue[index].responses.button1Text;
            response2.GetComponentInChildren<Text>().text = currentDialogue.dialogue[index].responses.button2Text;

            //remove click events from buttons
            response1.onClick.RemoveAllListeners();
            response2.onClick.RemoveAllListeners();

            //add events to buttons
            switch(currentDialogue.dialogue[index].responses.button1Action)
            {
                case ResponseType.advance:
                    {
                        response1.onClick.AddListener(() => AdvanceDialogue());
                        break;
                    }
                case ResponseType.dialogue:
                    {
                        if (currentDialogue.dialogue[index].responses.dialogue1 != null)
                        {
                            response1.onClick.AddListener(() => StartDialogue(currentDialogue.dialogue[index].responses.dialogue1, currentNPC));
                        }
                        else
                        {
                            response1.onClick.AddListener(() => AdvanceDialogue());
                        }
                        break;
                    }
                case ResponseType.shop:
                    {
                        response1.onClick.AddListener(() => OpenShop());
                        break;
                    }
                case ResponseType.close:
                    {
                        response1.onClick.AddListener(() => CloseDialogueBox());
                        break;
                    }
            }
            switch (currentDialogue.dialogue[index].responses.button2Action)
            {
                case ResponseType.advance:
                    {
                        response2.onClick.AddListener(() => AdvanceDialogue());
                        break;
                    }
                case ResponseType.dialogue:
                    {
                        if (currentDialogue.dialogue[index].responses.dialogue2 != null)
                        {
                            response2.onClick.AddListener(() => StartDialogue(currentDialogue.dialogue[index].responses.dialogue2, currentNPC));
                        }
                        else
                        {
                            response2.onClick.AddListener(() => AdvanceDialogue());
                        }
                        break;
                    }
                case ResponseType.close:
                    {
                        response2.onClick.AddListener(() => CloseDialogueBox());
                        break;
                    }
                case ResponseType.shop:
                    {
                        response2.onClick.AddListener(() => OpenShop());
                        break;
                    }
            }
           
        } 
        else
        {
            advanceButton.gameObject.SetActive(true);
        }
        #endregion

        //text done, allow the player to continue
        canContinue = true;
        
        yield return null;
    }
    #endregion

    #region open/close dialogue
    void OpenDialogueBox()
    {
        dialogueBox.SetActive(true);
        showDialogue = true;
        player.currentMenu = "Dialogue";

        advanceButton.gameObject.SetActive(false);
        advanceButton.onClick.RemoveAllListeners();
        advanceButton.onClick.AddListener(() => AdvanceDialogue());

        PauseHandler.Pause();
    }

    void CloseDialogueBox()
    {

        dialogueBox.SetActive(false);
        showDialogue = false;
        if (!inShop)
        {
            currentDialogue = null;
            player.currentMenu = null;

            PauseHandler.Resume();
        }
    }
    #endregion

    #region shop

    public void OpenShop()
    {
        Storage NPCShop = currentNPC.GetComponent<Storage>();
        if (NPCShop != null)
        {
            inShop = true;
            CloseDialogueBox();
            NPCShop.Open();
        }
        else
        {
            AdvanceDialogue();
        }
            
    }

    public void CloseShop()
    {
        player.GetComponent<Inventory>().CloseInventory();
        OpenDialogueBox();
        inShop = false;
        AdvanceDialogue();
    }

    #endregion
}
