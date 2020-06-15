using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float[] data = new float[1];
    public DialogueStruct[] NPCDialogue = new DialogueStruct[1];




    #region dialogue conditions
    public enum ConditionType
    {
        Data,
        Quest
    }

    [System.Serializable]
    public class Condition 
    {
        public ConditionType conditionType;

        [Header("NPC Data")]
        public int dataElement;
        public Comparison symbol;
        public float comparison;

        [Header("Quest")]
        public int questIndex;
        public QuestState questState;

        #region check condition
        public bool Check(NPC npcData)
        {
            switch(conditionType)
            {
                case ConditionType.Data:
                    #region check npc data
                    float data = npcData.data[dataElement];
                    switch (symbol)
                    {
                        case Comparison.Equals:
                            {
                                if (data == comparison)
                                {
                                    return true;
                                }
                                break;
                            }
                        case Comparison.Greater:
                            {
                                if (data > comparison)
                                {
                                    return true;
                                }
                                break;
                            }
                        case Comparison.Less:
                            {
                                if (data < comparison)
                                {
                                    return true;
                                }
                                break;
                            }
                    }
                    #endregion
                    break;

                case ConditionType.Quest:
                    #region check quest data
                    npcData.UpdateQuestStates();
                    if (npcData.quests.Length <= questIndex)
                    {
                        return false;
                    }
                    Quest quest = npcData.quests[questIndex];
                    if (quest.state == questState)
                    {
                        return true;
                    }
                    break;
                    #endregion

            }
            

        return false;
        }
        #endregion
    }
    #endregion

    #region dialogue

    DialogueControl dialogueControl;

    [System.Serializable]
    public struct DialogueStruct
    {
        public bool hasCondition;
        public Condition condition;
        public Dialogue dialogue;
    }

    private void Start()
    {
        dialogueControl = GameObject.FindGameObjectWithTag("Player").GetComponent<DialogueControl>();
    }


    public void Talk()
    {
        for (int i = 0; i <  NPCDialogue.Length; i++)
        {
            if (NPCDialogue[i].hasCondition)
            {
                if (NPCDialogue[i].condition.Check(this))
                {
                    dialogueControl.StartDialogue(NPCDialogue[i].dialogue, this);
                    return;
                }
                
            }
            else
            {
                dialogueControl.StartDialogue(NPCDialogue[i].dialogue, this);
                return;
            }
        }
       
    }
    #endregion

    #region quests
    public Quest[] quests;

    public Quest GetQuest()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (quests[i].state != QuestState.Claimed)
            {
                Quest quest = quests[i];
                return quest;
            }
        }
        return null;
    }

    public void UpdateQuestStates()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            quests[i].UpdateState();
        }
    }
    #endregion
}

public enum Comparison
{
    Equals,
    Greater,
    Less
}

