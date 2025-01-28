using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")] 
    [SerializeField] 
    private QuestInfoSO questInfoForPoint; 

    [Header("Config")] 
    [SerializeField] private bool startPoint = true; 
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false; 
    private string questId; 
    private QuestState currentQuestState;
    private QuestIcon questIcon;

    private void Awake() { 
        questId = questInfoForPoint.id; 
        questIcon = GetComponentInChildren<QuestIcon>();
    } 
    
    private void OnEnable() { 
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        //GameEventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
    }

    private void OnDisable() {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        //GameEventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
    }

    private void SubmitPressed() { 
        if (!playerIsNear) return; 

        if (currentQuestState.Equals(QuestState.CAN_START) & startPoint) {
           // Debug.Log("IT WOOOOKS, IT WOOOOOOOOOOOOKS");
            GameEventsManager.instance.questEvents.StartQuest(questId); 
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH) & finishPoint) GameEventsManager.instance.questEvents.FinishQuest(questId);
    }

    private void QuestStateChange(Quest quest) { 
        if (quest.info.id.Equals(questId)) { 
            currentQuestState = quest.state; 
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
            //Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    private void Update() { 
        Transform player = GameObject.FindWithTag("Player").transform;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < 30) playerIsNear = true;
        else playerIsNear = false;

        var tapped = GameEventsManager.instance.inputEvents.tapped;
        var animalTask = gameObject.GetComponent<animalTaskManager>();

        //DISABLED => START_TASK => IN_PROGRESS => CAN_FINISH => END_TASK => FINISHED
        switch (animalTask.taskState) {
            case AnimalTaskState.START_TASK:
            animalTask.taskState = AnimalTaskState.IN_PROGRESS;
            SubmitPressed();
            break;

            case  AnimalTaskState.END_TASK:
            animalTask.taskState = AnimalTaskState.FINISHED;
            SubmitPressed();
            break;
        }

        if (currentQuestState.Equals(QuestState.CAN_FINISH)) animalTask.taskState = AnimalTaskState.CAN_FINISH;
    }
    // private void OnTriggerEnter2D(Collider2D otherCollider) { 
    //     if (otherCollider.CompareTag("Player")) playerIsNear = true; 
    // } 

    // private void OnTriggerExit2D(Collider2D otherCollider) { 
    //     if (otherCollider.CompareTag("Player")) playerIsNear = false; 
    // } 
}
