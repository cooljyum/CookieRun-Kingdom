using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    [SerializeField]
    private InputField _idInput; // �̸��� �Ǵ� ����� �̸� �Է� �ʵ�
    [SerializeField]
    private InputField _pwInput; // ��й�ȣ �Է� �ʵ�
    [SerializeField]
    private InputField _nameInput; // ����� �̸� �Է� �ʵ� (ȸ������ �� ���)
    [SerializeField]
    private Text _statusTxt; // ���� �޽��� �ؽ�Ʈ

    // �α���
    public void Login()
    {
        // PlayFab Title ID�� �������� ���� ��� ����
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "1504A";

        // �̸��ϰ� ��й�ȣ�� ����Ͽ� �α��� ��û ����
        var request = new LoginWithEmailAddressRequest { Email = _idInput.text, Password = _pwInput.text };
        // PlayFab API�� ȣ���Ͽ� �̸��Ϸ� �α��� �õ�
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    // �α��� ���� �� ȣ�� �ݹ�
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("�α��� ����");
        _statusTxt.text = "�α��� ����!";

        // �α��� ���� �� ���� ���� ��������
        GetAccountInfo();
    }

    // �α��� ���� �� ȣ�� �ݹ�
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("�α��� ����"); 
        Debug.LogWarning(error.GenerateErrorReport()); // ���� �� ���� ���

        _statusTxt.text = "�α��� ���ФФ�";
    }

    // ȸ������
    public void Register()
    {
        //�ʵ� ��ĭ ó��
        if (string.IsNullOrEmpty(_idInput.text))
        {
            _statusTxt.text = "���̵� �Է����ּ���!";
            return;
        } 
        else if (string.IsNullOrEmpty(_pwInput.text)) 
        { 
            _statusTxt.text = "��й�ȣ�� �Է����ּ���!";
            return;
        }
        else if (string.IsNullOrEmpty(_nameInput.text)) 
        { 
            _statusTxt.text = "�г����� �Է����ּ���!";
            return;
        }

        // �̸���, ��й�ȣ, ����� �̸��� ����Ͽ� ȸ������ ��û ����
        var request = new RegisterPlayFabUserRequest { Email = _idInput.text, Password = _pwInput.text, Username = _nameInput.text };
        // PlayFab API�� ȣ���Ͽ� ����� ��� �õ�
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }

    // ȸ������ ���� �� ȣ�� �ݹ�
    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("���� ����");
        _statusTxt.text = "���� ����!";
    }

    // ȸ������ ���� �� ȣ�� �ݹ�
    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("���� ����");
        Debug.LogWarning(error.GenerateErrorReport()); // ���� �� ���� ���
        _statusTxt.text = "���� ���ФФ�";
    }

    // ���� �α����� ������� ���� ���� ��������
    private void GetAccountInfo()
    {
        // �̸����� ����Ͽ� ���� ���� ��û ����
        var request = new GetAccountInfoRequest { Email = _idInput.text };
        // PlayFab API�� ȣ���Ͽ� ���� ���� ��������
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    // ���� ���� ���������� �������� �� ȣ�� �ݹ�
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        // ���� �������� ����� �̸��� ������
        string username = result.AccountInfo.Username;
        if (string.IsNullOrEmpty(username))
        {
            // ����� �̸� ���� ��� �̸����� ����� �̸����� ���
            username = result.AccountInfo.PrivateInfo.Email;
        }
        // NetManager ���� �г��� �����ָ鼭 Connect
        NetManager.Instance.Connect(username);
    }

    // ���� ���� �������� ���� �� ȣ�� �ݹ�
    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogWarning("���� ���� �������� ����");
        Debug.LogWarning(error.GenerateErrorReport());
    }
}
