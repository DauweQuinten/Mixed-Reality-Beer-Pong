using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckSpaceAvailability : MonoBehaviour
{
    private List<MRUKRoom> _rooms = new List<MRUKRoom>();
    public UnityEvent OnNoRoomAvailable = new UnityEvent();

    private void Start()
    {
        StartCoroutine(StartCheckAfterSeconds(1f));
    }


    /// <summary>
    /// Check if there is any room available.
    /// If not, invoke OnNoRoomAvailable event.
    /// </summary>
    public void CheckAvailableRooms()
    {
        

        _rooms = MRUK.Instance?.GetRooms();     
        if ( _rooms != null)
        {
            if (_rooms.Count == 0)
            {
                OnNoRoomAvailable.Invoke();
                Debug.Log("no rooms found");
            }
            else
            {
                Debug.Log($"count: {_rooms.Count}");
                Debug.Log(_rooms[0]);
            }
        }
        else
        {
            Debug.Log("No instance of MRUK found");
        }
    }

    IEnumerator StartCheckAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        CheckAvailableRooms();
    }
}
