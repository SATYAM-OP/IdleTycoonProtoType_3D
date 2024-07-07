using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public const string SAVE_KEY = "Player progress";
    public static Progress playerProgress;

    private void Awake()
    {
        LoadProgress();
    }

    public void LoadProgress()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, null);

        if (string.IsNullOrEmpty(json))
        {
            playerProgress = new Progress();
        }
        else
        {
            playerProgress = JsonUtility.FromJson<Progress>(json);
        }
    }

    public static void SaveProgress()
    {
        string progress = JsonUtility.ToJson(playerProgress);
        PlayerPrefs.SetString(SAVE_KEY, progress);
    }

    private void OnDestroy()
    {
        SaveProgress();
    }

}

[System.Serializable]
public class Progress
{
    public List<RoomStates> rooms;
    public Progress()
    {
        rooms = new List<RoomStates>();

        // Initialize rooms
        RoomStates firstRoom = new RoomStates
        {
            isUnlocked = true,
            isOccupied = false,
            isDirty = false,
            roomLevel = 1,
            roomIndex = 0
        };
        rooms.Add(firstRoom);

        for (int i = 1; i < 10; i++) // Assume 10 rooms for example
        {
            RoomStates room = new RoomStates
            {
                isUnlocked = false,
                isOccupied = false,
                isDirty = false,
                roomLevel = 1,
                roomIndex = i
            };
            rooms.Add(room);
        }
    }

}
