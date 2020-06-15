using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string name;
    [TextArea(3, 5)]
    public string description;
    public QuestState state;
    public QuestGoal goal;
    public QuestReward reward;

    public void UpdateState()
    {
        if (state == QuestState.Available || state == QuestState.Claimed)
        {
            return;
        }

        if (goal.CheckGoal())
        {
            state = QuestState.Complete;
        }
        else
        {
            state = QuestState.Active;
        }
    }

    public void Complete()
    {
        goal.GiveItems();
        reward.ClaimReward();
    }
}

public enum QuestState
{
    Available,
    Active,
    Complete,
    Claimed
}
