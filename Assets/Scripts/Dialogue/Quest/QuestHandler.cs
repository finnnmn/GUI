using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    public static List<Quest> questList = new List<Quest>();
    protected List<Button> questButtons = new List<Button>();

    PlayerControl player;

    [Header("UI References")]
    public GameObject questLog;
    public Button buttonPrefab;

    public RectTransform content;

    [Space(5)]
    public Text questName;
    public Text questDescription;
    public Text questProgress;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        CloseQuestLog();
    }

    public void OpenQuestLog()
    {
        //set quest panel to active
        questLog.SetActive(true);
        DeselectQuest();

        PauseHandler.Pause();
        PauseHandler.paused = true;
        player.currentMenu = "QuestLog";

        //destroy current buttons
        foreach (Button button in questButtons)
        {
            GameObject.Destroy(button.gameObject);
        }
        questButtons.Clear();

        //return if no quests
        if (questList.Count == 0)
        {
            return;
        }

        //update state of each quest
        foreach(Quest quest in questList)
        {
            quest.UpdateState();
        }

        //set up a button for selecting each quest
        for (int i = 0; i < questList.Count; i++)
        {
            Button newButton = Instantiate(buttonPrefab);
            questButtons.Add(newButton);
            newButton.transform.SetParent(content.transform);
            newButton.GetComponentInChildren<Text>().text = questList[i].name;
            newButton.onClick.RemoveAllListeners();
            int questIndex = i;
            newButton.onClick.AddListener(() => SelectQuest(questList[questIndex]));
        }

        //set size for list of buttons
        content.sizeDelta = new Vector2(0, 5 + 30 * questButtons.Count);

    }

    public void CloseQuestLog()
    {
        questLog.SetActive(false);
        DeselectQuest();

        PauseHandler.Resume();
        PauseHandler.paused = false;
        player.currentMenu = null;
    }

    public void SelectQuest(Quest quest)
    {
        questName.text = quest.name;
        questDescription.text = quest.description;
        questProgress.text = quest.goal.itemName + ": " + quest.goal.amount + "/" + quest.goal.requiredAmount;
    }

    void DeselectQuest()
    {
        questName.text = "";
        questDescription.text = "";
        questProgress.text = "";
    }
}
