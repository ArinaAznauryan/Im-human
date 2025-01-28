using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap; 
    private GameObject objective;
    public questHint questHintRef;
    
    private int currentPlayerLevel;

    private void Awake() { 
        objective =  Resources.Load<GameObject>("Prefabs/Objective");

        questHintRef = FindObjectOfType<questHint>();

        questMap = CreateQuestMap(); 

        //Debug.Log("QUEST MAP: " + questMap);

        // foreach (KeyValuePair<string, Quest> bitch in questMap) { 
        //     Debug.Log("BITCH OHFUCK: " + bitch);
        // }

        // Quest quest = GetQuestById("CollectGlassesQuest"); 
        // Debug.Log("FUCK YOUSELF RIGHT NOW: " + quest);
        // Debug.Log("Woof: " + quest.info.descrption); 
        // Debug.Log("Woof: " + quest.info.levelRequirement); 
        // Debug.Log("Woof: " + quest.state); 
        // Debug.Log("Woof: " + quest.CurrentStepExists());
    }

    private void Start() { 
        foreach (Quest quest in questMap.Values) { 
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void OnEnable() { 
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable() { 
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }

    private void ChangeQuestState(string id, QuestState state) { 
        Quest quest = GetQuestById(id); 
        quest.state = state; 
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private void PlayerLevelChange(int level) {currentPlayerLevel = level;}

    private bool CheckRequirementsMet(Quest quest) { 
        bool meetsRequirements = true; 

        if (currentPlayerLevel < quest.info.levelRequirement) meetsRequirements = false; 
        
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites) { 
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED) 
                meetsRequirements = false; 
        } 
        return meetsRequirements; 
    } 

    private void Update() { 
        foreach (Quest quest in questMap.Values) { 
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest)) 
                ChangeQuestState(quest.info.id, QuestState.CAN_START); 
        }
    }

    private void StartQuest(string id) { 
        Quest quest = GetQuestById(id); 
        quest.InstatiateCurrentQuestStep(this.transform); 
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        CreateObjective(quest);
        
    }

    private void CreateObjective(Quest quest) {
        GameObject objectiveClone = Instantiate(objective, Vector2.zero, Quaternion.identity) as GameObject;
        objectiveClone.SetActive(true);
        objectiveClone.transform.parent = GameObject.Find("questUI").transform;
        objectiveClone.transform.localPosition = Vector2.zero;
        objectiveClone.transform.Find("ObjectivePanel/ObjectiveDescription").gameObject.GetComponent<TextMeshProUGUI>().text = quest.info.description;
    }

    private void AdvanceQuest(string id) {
        Quest quest = GetQuestById(id); 
        quest.MoveToNextStep(); 
        if (quest.CurrentStepExists()) quest.InstatiateCurrentQuestStep(this.transform); 
        else ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
    }

    private void FinishQuest(string id) { 
        Quest quest = GetQuestById(id); 
        ClaimRewards(quest); 
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest) { 
        //_________OPEN THE DOORS___________//
        //GameEventsManager.instance.goldEvents.GoldGained(quest.info.goldReward); 
        //GameEventsManager.instance.playerEvents.ExperienceGained(quest.info.experienceReward);
    }

    private Dictionary<string, Quest> CreateQuestMap() { 
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests"); 
        //Debug.Log("ALL QUESTS: " + allQuests);

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>(); 
        
        foreach (QuestInfoSO questInfo in allQuests) { 
            if (idToQuestMap.ContainsKey(questInfo.id)) { 
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id); 
            } 
            //Debug.Log("BITCH OH FUCK: " + questInfo + questInfo.id);
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id) { 
        Quest quest = questMap[id]; 
        //Debug.Log("QUEST ID: " + questMap[id] + "WOOF: " + quest);
        if (quest == null) { 
            Debug.LogError(quest + " ID  not found in the Quest Map: " + id); 
        }
        //Debug.Log("FCKN QUEST: " + quest.info);
        return quest; 
    } 
}
