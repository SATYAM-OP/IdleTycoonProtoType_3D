using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotel : MonoBehaviour
{
    [SerializeField] List<RoomView> rooms;

    public bool CanAttendCustomer()
    {
        return GetAvailableRoom() != null;
    }

    public RoomView GetAvailableRoom()
    {
        foreach (var item in rooms)
        {
            if (item.roomState.isUnlocked && !item.roomState.isOccupied && !item.roomState.isDirty)
            {
                return item;
            }
        }
        return null;
    }
}
