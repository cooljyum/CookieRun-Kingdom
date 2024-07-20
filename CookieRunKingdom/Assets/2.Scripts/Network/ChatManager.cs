using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField _msgInput; // 메시지 입력 필드
    [SerializeField]
    private Text _chatLog; // 채팅 로그를 표시할 텍스트 UI
    [SerializeField]
    private Text _chattingList; // 접속 중인 플레이어 목록을 표시할 텍스트 UI
    [SerializeField]
    private Button _sendBtn; // 메시지 전송 버튼
    [SerializeField]
    private ScrollRect _scrollRect; // 채팅 로그의 스크롤을 제어하는 ScrollRect

    private string _chatters; // 접속 중인 플레이어 목록 문자열

    void Start()
    {
        // Photon의 메시지 큐가 작동하도록 설정
        PhotonNetwork.IsMessageQueueRunning = true;

        // 버튼 클릭 이벤트에 연결
        _sendBtn.onClick.AddListener(SendButtonOnClicked);
    }

    // 메시지 전송 시 호출
    public void SendButtonOnClicked()
    {
        // 메시지 입력 필드가 비어 있는지 확인
        if (string.IsNullOrEmpty(_msgInput.text))
        {
            Debug.Log("메시지가 비어있습니다.");
            return;
        }

        // 메시지 형식을 지정하여 문자열로 만듦
        string msg = string.Format("[{0}]: {1}", PhotonNetwork.LocalPlayer.NickName, _msgInput.text);
        // 다른 클라이언트들에게 메시지 전송
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        // 자신 메시지 수신 처리
        ReceiveMsg(msg);

        // 입력 필드 초기화
        _msgInput.text = "";
    }

    void Update()
    {
        // 플레이어 목록 업데이트
        chatterUpdate();

        // 엔터 키가 눌리고 입력 필드가 포커스되지 않은 경우 메시지 전송
        if (Input.GetKeyDown(KeyCode.Return) && !_msgInput.isFocused)
        {
            SendButtonOnClicked();
        }
    }

    // 접속 중인 플레이어 목록을 업데이트
    void chatterUpdate()
    {
        _chatters = "*접속 플레이어 리스트*\n";

        // 현재 접속 중인 모든 플레이어 닉네임을 리스트에 추가
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            _chatters += p.NickName + "\n";
        }
        
        _chattingList.text = _chatters;
    }

    // RPC 메서드: 메시지를 수신하여 채팅 로그에 추가하는 메서드
    // 네트워크 상의 다른 클라이언트에서 호출하는 메서드
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        // 메시지 추가
        _chatLog.text += "\n" + msg;

        // 스크롤을 가장 아래로 내림
        if (_scrollRect != null)
        {
            _scrollRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
