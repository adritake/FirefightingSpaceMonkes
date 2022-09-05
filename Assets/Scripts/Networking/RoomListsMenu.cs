using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private ListedRoom _roomList;
    [SerializeField] private Transform _contentParent;

    private List<ListedRoom> _listedRooms = new List<ListedRoom>();

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                //Removed from the list
                int index = _listedRooms.FindIndex(x => x.RoomInfo.Name == info.Name);
                if(index != -1)
                {
                    Destroy(_listedRooms[index].gameObject);
                    _listedRooms.RemoveAt(index);
                }
            }
            else
            {
                //Added to the list
                ListedRoom listedRoom = Instantiate(_roomList, _contentParent);
                if (listedRoom != null)
                {
                    listedRoom.SetRoomInfo(info);
                    _listedRooms.Add(listedRoom);
                }
            }
        }
    }
}
