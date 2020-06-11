using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float[] data = new float[1];
    public DialogueStruct[] NPCDialogue = new DialogueStruct[1];
    
    [System.Serializable]
    public struct DialogueStruct
    {
        public bool hasCondition;
        public Condition condition;
        public Dialogue dialogue;
    }

    [System.Serializable]
    public class Condition 
    {
        public int dataElement;
        public Comparison symbol;
        public float comparison;

        #region check condition
        public bool Check(NPC npcData)
        {
            float data = npcData.data[dataElement];
            switch(symbol)
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

        return false;
        }
        #endregion
    }


    DialogueControl dialogueControl;

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
}

public enum Comparison
{
    Equals,
    Greater,
    Less
}

