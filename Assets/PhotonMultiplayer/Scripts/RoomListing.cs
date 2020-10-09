using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject roomListingPrefab;
    [SerializeField]
    private Transform content;

    private List<GameObject> roomListings = new List<GameObject>();

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                int index = roomList.FindIndex(ByName(info.Name));
                if (index != -1)
                {
                    Destroy(content.GetChild(index).gameObject);
                    roomListings.RemoveAt(index);
                }
            }
            GameObject roomListing = Instantiate(roomListingPrefab, content);
            Debug.Log("instantiate room listing");
            if (roomListing != null)
            {
                if (info.IsOpen && info.IsVisible)
                {
                    Debug.Log(info.Name);
                    RoomButton roomButton = roomListing.GetComponent<RoomButton>();
                    roomButton.SetRoom(info.Name, info.MaxPlayers, info.PlayerCount);
                    roomListings.Add(roomListing);
                }
            }
        }
    }
    static System.Predicate<RoomInfo> ByName(string name)
    {
        return delegate (RoomInfo room)
        {
            return room.Name == name;
        };
    }
}
