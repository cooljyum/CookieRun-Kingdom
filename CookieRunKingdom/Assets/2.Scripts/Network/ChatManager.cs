using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField _msgInput; // �޽��� �Է� �ʵ�
    [SerializeField]
    private Text _chatLog; // ä�� �α׸� ǥ���� �ؽ�Ʈ UI
    [SerializeField]
    private Text _chattingList; // ���� ���� �÷��̾� ����� ǥ���� �ؽ�Ʈ UI
    [SerializeField]
    private Button _sendBtn; // �޽��� ���� ��ư
    [SerializeField]
    private ScrollRect _scrollRect; // ä�� �α��� ��ũ���� �����ϴ� ScrollRect

    private string _chatters; // ���� ���� �÷��̾� ��� ���ڿ�

    void Start()
    {
        // Photon�� �޽��� ť�� �۵��ϵ��� ����
        PhotonNetwork.IsMessageQueueRunning = true;

        // ��ư Ŭ�� �̺�Ʈ�� ����
        _sendBtn.onClick.AddListener(SendButtonOnClicked);
    }

    // �޽��� ���� �� ȣ��
    public void SendButtonOnClicked()
    {
        // �޽��� �Է� �ʵ尡 ��� �ִ��� Ȯ��
        if (string.IsNullOrEmpty(_msgInput.text))
        {
            Debug.Log("�޽����� ����ֽ��ϴ�.");
            return;
        }

        // �޽��� ������ �����Ͽ� ���ڿ��� ����
        string msg = string.Format("[{0}]: {1}", PhotonNetwork.LocalPlayer.NickName, _msgInput.text);
        // �ٸ� Ŭ���̾�Ʈ�鿡�� �޽��� ����
        photonView.RPC("ReceiveMsg", RpcTarget.OthersBuffered, msg);
        // �ڽ� �޽��� ���� ó��
        ReceiveMsg(msg);

        // �Է� �ʵ� �ʱ�ȭ
        _msgInput.text = "";
    }

    void Update()
    {
        // �÷��̾� ��� ������Ʈ
        chatterUpdate();

        // ���� Ű�� ������ �Է� �ʵ尡 ��Ŀ������ ���� ��� �޽��� ����
        if (Input.GetKeyDown(KeyCode.Return) && !_msgInput.isFocused)
        {
            SendButtonOnClicked();
        }
    }

    // ���� ���� �÷��̾� ����� ������Ʈ
    void chatterUpdate()
    {
        _chatters = "*���� �÷��̾� ����Ʈ*\n";

        // ���� ���� ���� ��� �÷��̾� �г����� ����Ʈ�� �߰�
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            _chatters += p.NickName + "\n";
        }
        
        _chattingList.text = _chatters;
    }

    // RPC �޼���: �޽����� �����Ͽ� ä�� �α׿� �߰��ϴ� �޼���
    // ��Ʈ��ũ ���� �ٸ� Ŭ���̾�Ʈ���� ȣ���ϴ� �޼���
    [PunRPC]
    public void ReceiveMsg(string msg)
    {
        // �޽��� �߰�
        _chatLog.text += "\n" + msg;

        // ��ũ���� ���� �Ʒ��� ����
        if (_scrollRect != null)
        {
            _scrollRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
