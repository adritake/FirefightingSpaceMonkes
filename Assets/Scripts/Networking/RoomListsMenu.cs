using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFSM
{
    public class RoomListsMenu : MonoBehaviourPunCallbacks
    {
        [Header("Room List Prefab")]
        [SerializeField] private ListedRoom _roomList;
        [Header("Content object, where rooms will be listed")]
        [SerializeField] private Transform _contentParent;

        private List<ListedRoom> _listedRooms = new List<ListedRoom>();

        #region Monobehaviour
        private void Awake()
        {
            PhotonNetwork.LeaveLobby();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.JoinLobby();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (_listedRooms.Count > 0)
            {
                foreach (ListedRoom room in _listedRooms)
                {
                    Destroy(room.gameObject);
                }
                _listedRooms.Clear();
            }
        }

        #endregion

        #region Callbacks Overrides
        public override void OnLeftLobby()
        {
            if (_listedRooms.Count > 0)
            {
                foreach (ListedRoom room in _listedRooms)
                {
                    Destroy(room.gameObject);
                }
                _listedRooms.Clear();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                if (info.RemovedFromList)
                {
                    //Removed from the list
                    int index = _listedRooms.FindIndex(x => x.RoomInfo.Name == info.Name);
                    if (index != -1)
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

        #endregion
    }
}
