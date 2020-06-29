using UnityEngine;

#region structs and enums


[System.Serializable]
public struct Response
{
  
    [Header("Only if above is true")]
    public string button1Text;
    public ResponseType button1Action;
    public string button2Text;
    public ResponseType button2Action;

    [Header("If dialogue type")]
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    
}
public enum ResponseType
{
    advance,
    dialogue,
    shop,
    close,
    quest
}
[System.Serializable]
public struct DataSet
{
    public DataType dataType;
    public int dataIndex;
    [Space(5)]
    public float value;
}
public enum DataType
{
    useDataIndex,
    buyPriceMultiplier,
}



#endregion


[CreateAssetMenu(menuName = "Dialogue")]
[System.Serializable]
public class Dialogue : ScriptableObject
{
    public DialogueText[] dialogue = new DialogueText[1];

}

[System.Serializable]
public class DialogueText
{
    public string name;
    [TextArea(3, 5)]
    public string dialogue;
    [Space(5)]
    public bool response;
    public Response responses;
    [Space(5)]
    public bool setData;
    public DataSet dataSet;

}

