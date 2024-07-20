using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Data;
using UnityEngine.SceneManagement;

public class NetManager : MonoBehaviourPunCallbacks
{
    public static NetManager Instance { get; private set; }

    [SerializeField]
    private Button _loginBtn; // 로그인 버튼
    [SerializeField]
    private Text _statusTxt; // 상태 메시지 텍스트

    private bool _isConnectedToMaster = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Photon 서버에 연결 시도
        PhotonNetwork.ConnectUsingSettings();
        _loginBtn.interactable = false; // 연결되기 전까지 로그인 버튼 비활성화
        _statusTxt.text = "Connecting to Master Server..";
    }

    // 로그인 버튼 클릭 시 호출되는 메서드
    public void Connect(string name)
    {
        // 닉네임 설정
        PhotonNetwork.LocalPlayer.NickName = name;

        // Photon 서버에 연결되었는지 확인
        if (_isConnectedToMaster)
        {
            _statusTxt.text = "Connecting to Room..";
            PhotonNetwork.JoinRandomOrCreateRoom(); // 무작위 방에 접속 또는 새 방 생성
        }
        else
        {
            _statusTxt.text = "Offline : failed to Connect";
        }
    }

    // 마스터 서버에 성공적으로 연결되었을 때 호출되는 콜백
    public override void OnConnectedToMaster()
    {
        _isConnectedToMaster = true;
        _loginBtn.interactable = true; // 로그인 버튼 활성화
        _statusTxt.text = "Online : connected to master server";
    }

    // 서버 연결이 끊겼을 때 호출 콜백
    public override void OnDisconnected(DisconnectCause cause)
    {
        _isConnectedToMaster = false;
        _loginBtn.interactable = false; // 로그인 버튼 비활성화
        _statusTxt.text = "Offline : failed to connect.\nreconnecting...";
        PhotonNetwork.ConnectUsingSettings(); // 재연결 시도
    }

    // 무작위 방 참가에 실패했을 때 호출 콜백
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _statusTxt.text = "No empty room. creating new room..."; // 빈 방이 없을 때 새 방 생성 메시지
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // 최대 4명인 새 방 생성
    }

    // 방에 성공적으로 참가했을 때 호출 콜백
    public override void OnJoinedRoom()
    {
        _statusTxt.text = "Success to join room"; // 방 참가 성공 메시지
        SceneManager.LoadScene("KingdomScene"); // KingdomScene 씬 로드
        // PhotonNetwork.LoadLevel("Main"); // 네트워크 연결된 씬 로드 할때 쓰이는 코드
    }
}
