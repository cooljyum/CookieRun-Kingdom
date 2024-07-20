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
    private Button _loginBtn; // �α��� ��ư
    [SerializeField]
    private Text _statusTxt; // ���� �޽��� �ؽ�Ʈ

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
        // Photon ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        _loginBtn.interactable = false; // ����Ǳ� ������ �α��� ��ư ��Ȱ��ȭ
        _statusTxt.text = "Connecting to Master Server..";
    }

    // �α��� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void Connect(string name)
    {
        // �г��� ����
        PhotonNetwork.LocalPlayer.NickName = name;

        // Photon ������ ����Ǿ����� Ȯ��
        if (_isConnectedToMaster)
        {
            _statusTxt.text = "Connecting to Room..";
            PhotonNetwork.JoinRandomOrCreateRoom(); // ������ �濡 ���� �Ǵ� �� �� ����
        }
        else
        {
            _statusTxt.text = "Offline : failed to Connect";
        }
    }

    // ������ ������ ���������� ����Ǿ��� �� ȣ��Ǵ� �ݹ�
    public override void OnConnectedToMaster()
    {
        _isConnectedToMaster = true;
        _loginBtn.interactable = true; // �α��� ��ư Ȱ��ȭ
        _statusTxt.text = "Online : connected to master server";
    }

    // ���� ������ ������ �� ȣ�� �ݹ�
    public override void OnDisconnected(DisconnectCause cause)
    {
        _isConnectedToMaster = false;
        _loginBtn.interactable = false; // �α��� ��ư ��Ȱ��ȭ
        _statusTxt.text = "Offline : failed to connect.\nreconnecting...";
        PhotonNetwork.ConnectUsingSettings(); // �翬�� �õ�
    }

    // ������ �� ������ �������� �� ȣ�� �ݹ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        _statusTxt.text = "No empty room. creating new room..."; // �� ���� ���� �� �� �� ���� �޽���
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // �ִ� 4���� �� �� ����
    }

    // �濡 ���������� �������� �� ȣ�� �ݹ�
    public override void OnJoinedRoom()
    {
        _statusTxt.text = "Success to join room"; // �� ���� ���� �޽���
        SceneManager.LoadScene("KingdomScene"); // KingdomScene �� �ε�
        // PhotonNetwork.LoadLevel("Main"); // ��Ʈ��ũ ����� �� �ε� �Ҷ� ���̴� �ڵ�
    }
}
