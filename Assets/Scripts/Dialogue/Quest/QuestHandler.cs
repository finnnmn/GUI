using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    public static List<Quest> questList = new List<Quest>();
    public Quest selectedQuest;
    protected List<Button> questButtons = new List<Button>();

    PlayerControl player;

    [Header("Quest Log")]
    public GameObject questLog;
    public Button buttonPrefab;

    public RectTransform content;

    [Space(5)]
    public Text questName;
    public Text questDescription;
    public Text questProgress;

    [Header("Accept Quest")]
    public GameObject acceptQuestPanel;
    public Text acceptQuestName;
    public Text acceptQuestDescription;
    public Text acceptQuestGoal;
    public Text acceptQuestGoldReward;
    public Text acceptQuestExpReward;

    [Header("Quest Reward")]
    public GameObject rewardPanel;
    public Text rewardGold;
    public Text rewardExperience;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        CloseQuestLog();
    }

    #region Accept Quest

    public void AcceptQuest()
    {
        //check that quest can be accepted
        if (selectedQuest == null)
        {
            return;
        }
        if (selectedQuest.state != QuestState.Available)
        {
            return;
        }
        //accept quest
        selectedQuest.state = QuestState.Active;
        questList.Add(selectedQuest);
        CloseAcceptPanel();

        
    }
    public void OpenAcceptPanel(Quest quest)
    {
        //setup quest
        selectedQuest = quest;
        acceptQuestPanel.SetActive(true);
        acceptQuestName.text = quest.name;
        acceptQuestDescription.text = quest.description;
        acceptQuestGoal.text = "Gather: " + quest.goal.itemName + " x" + quest.goal.requiredAmount;
        acceptQuestGoldReward.text = "Gold: " + quest.reward.gold;
        acceptQuestExpReward.text = "Exp: " + quest.reward.experience;

        //pause game
        player.currentMenu = "Quest";
        PauseHandler.paused = true;
        PauseHandler.Pause();
    }

    public void CloseAcceptPanel()
    {
        selectedQuest = null;
        acceptQuestPanel.SetActive(false);

        player.currentMenu = null;
        PauseHandler.paused = false;
        PauseHandler.Resume();
    }
    #endregion

    #region Quest Reward

    public void CompleteQuest()
    {
        if (selectedQuest == null)
        {
            return;
        }
        if (selectedQuest.state != QuestState.Complete || (!questList.Contains(selectedQuest)))
        {
            return;
        }
        selectedQuest.state = QuestState.Claimed;
        selectedQuest.Complete();
        questList.Remove(selectedQuest);
        CloseRewardPanel();
    }

    public void OpenRewardPanel(Quest quest)
    {
        selectedQuest = quest;
        rewardPanel.SetActive(true);
        rewardGold.text = "Gold: " + quest.reward.gold;
        rewardExperience.text = "Exp: " + quest.reward.experience;

        //pause game
        player.currentMenu = "Quest";
        PauseHandler.paused = true;
        PauseHandler.Pause();
    }

    public void CloseRewardPanel()
    {
        selectedQuest = null;
        rewardPanel.SetActive(false);

        player.currentMenu = null;
        PauseHandler.paused = false;
        PauseHandler.Resume();
    }
    #endregion

    #region Quest Log
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
        quest.UpdateState();
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
    #endregion
}
