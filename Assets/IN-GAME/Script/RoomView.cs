using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;


[Serializable]
public class RoomStates
{
    public bool isUnlocked = false;
    public bool isOccupied = false;
    public bool isDirty = false;
    public int roomLevel = 1;
    public int roomIndex;
}

public class RoomView : MonoBehaviour
{
    [SerializeField] MoneyStack moneyStack;
    [SerializeField] UnlockRoom unlockRoom;
    [SerializeField] UpgradeRoom UpgradeRoom;

    public RoomStates roomState;
    public GameObject DoorWall;
    public GameObject backWall;
    public GameObject room;
    public GameObject darkRoom;
    public CleaningSpot[] cleanSpots;
    public Transform sleepAnchor;
    public MeshRenderer bedRenderer;

    public ParticleSystem sleepParticle;
    [SerializeField] TextMeshProUGUI levelNumber;

    private void Start()
    {
        roomState = SaveSystem.playerProgress.rooms[roomState.roomIndex];
        LoadRoomState();
    }

    public void AssignedCustomer(Customer customer)
    {
        print("CUSTOMER ASSIGNED");
        customer.MoveTo(sleepAnchor.position);
        customer.currentState=CustomerStates.BEING_CUSTOMER;
        customer.OnTargetReached += HandelReachedTarget;
    }

    private void HandelReachedTarget(Customer customer)
    {
        switch (customer.currentState)
        {
            case CustomerStates.WAITING_CUSTOMER:
                print("CUSTOMER WAITING");
                break;
            case CustomerStates.BEING_CUSTOMER:
                print("SLEEPING");
                StartCoroutine(SleepCustomer(customer));
                break;
            case CustomerStates.SLEEP:
                print("SO RAHA HAI");
                break;
            case CustomerStates.GOING_HOME:
                customer.OnTargetReached -= HandelReachedTarget;
                customer.currentState = CustomerStates.WAITING_CUSTOMER;
                customer.gameObject.SetActive(false);
                break;
            default:
                print("DOING NOTHING");
                break;
        }
    }

    private void LoadRoomState()
    {
        //    roomState = SaveSystem.playerProgress.rooms[roomIndex];
        if (!roomState.isUnlocked)
        {
            backWall?.SetActive(false);
            room?.SetActive(false);
            DoorWall?.SetActive(true);
            moneyStack.gameObject.SetActive(false);
            if (unlockRoom!=null)
            {
              unlockRoom.RoomUnlocked += roomUnlocked;
            }
        }
        else
        {
            SubscribeCleaningSpots();
            moneyStack.gameObject.SetActive(true);
            backWall?.SetActive(true);
            room?.SetActive(true);
            DoorWall?.SetActive(false);
            SetRoomNumner();

            if (unlockRoom != null)
            {
                unlockRoom.gameObject.SetActive(false);
            }
            if (roomState.roomLevel <= 1)
            {
                UpgradeRoom.RoomUpgraded += RoomUpgraded;
            }
            else
            {
                UpgradeRoom.gameObject.SetActive(false);
            }

        }

        if (roomState.isOccupied)
        {
            roomState.isOccupied = false;
        }

        CheckRoomDirty();

    }

    private void roomUnlocked()
    {
        roomState.isUnlocked = true;
        unlockRoom.RoomUnlocked -= roomUnlocked;
        LoadRoomState();
        SaveSystem.playerProgress.rooms[roomState.roomIndex]=roomState;
        SaveSystem.SaveProgress();
    }

    private void RoomUpgraded()
    {
        roomState.roomLevel += 1;
        SetRoomNumner();
        SaveSystem.playerProgress.rooms[roomState.roomIndex] = roomState;
        SaveSystem.SaveProgress();
    }
    private void SetRoomNumner()
    {
        levelNumber.text="LEVEL "+roomState.roomLevel;
        if (roomState.roomLevel>1)
        {
            bedRenderer.material.color = Color.blue;
        }
    }

    #region CleaningSpot

    public void SubscribeCleaningSpots()
    {
        foreach (var item in cleanSpots)
        {
            item.SpotCleaned += CheckCleaning;
        }
    }

    public void UnSubscribeCleaningSpots()
    {
        foreach (var item in cleanSpots)
        {
            item.SpotCleaned -= CheckCleaning;
        }
    }

    #endregion

    public void CheckRoomDirty()
    {
            foreach (var item in cleanSpots)
            {
                item.parent.SetActive(roomState.isDirty);
                item.isDirty = roomState.isDirty;
            }
    }

    IEnumerator SleepCustomer(Customer customer)
    {

        const float SLEEP_TIME = 5f;

        customer.currentState=CustomerStates.SLEEP;
        darkRoom?.SetActive(true);
        customer.Agent.enabled = false;
        customer.transform.DOJump(sleepAnchor.position, 1f, 1, 1f);

        if(sleepParticle!=null) 
         sleepParticle.Play();

        yield return new WaitForSeconds(SLEEP_TIME);

        print("WAKE UP SID");

        if (sleepParticle != null)
            sleepParticle?.Stop();

        customer.Agent.enabled = true;
        darkRoom?.SetActive(false);
        customer.currentState = CustomerStates.GOING_HOME;
        customer.MoveTo(CustomerPool.instance.customerSpawnPoint.position);
        roomState.isOccupied = false;
        roomState.isDirty=true;
        CheckRoomDirty();
        for (int i = 0; i < roomState.roomLevel; i++)
        {
            yield return new WaitForSeconds(0.25f);
            moneyStack.SpawnMoney();
        }
    }


    private void CheckCleaning()
    {
        foreach (var item in cleanSpots)
        {
            if (!item.isDirty)
            {
                return;
            }
        }
        roomState.isDirty=false;
    }


    private void OnDestroy()
    {
        UnSubscribeCleaningSpots();
    }

}
