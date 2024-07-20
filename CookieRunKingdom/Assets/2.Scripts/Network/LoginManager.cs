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
    private InputField _idInput; // 이메일 또는 사용자 이름 입력 필드
    [SerializeField]
    private InputField _pwInput; // 비밀번호 입력 필드
    [SerializeField]
    private InputField _nameInput; // 사용자 이름 입력 필드 (회원가입 시 사용)
    [SerializeField]
    private Text _statusTxt; // 상태 메시지 텍스트

    // 로그인
    public void Login()
    {
        // PlayFab Title ID가 설정되지 않은 경우 설정
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) PlayFabSettings.TitleId = "1504A";

        // 이메일과 비밀번호를 사용하여 로그인 요청 생성
        var request = new LoginWithEmailAddressRequest { Email = _idInput.text, Password = _pwInput.text };
        // PlayFab API를 호출하여 이메일로 로그인 시도
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }

    // 로그인 성공 시 호출 콜백
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("로그인 성공");
        _statusTxt.text = "로그인 성공!";

        // 로그인 성공 후 계정 정보 가져오기
        GetAccountInfo();
    }

    // 로그인 실패 시 호출 콜백
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("로그인 실패"); 
        Debug.LogWarning(error.GenerateErrorReport()); // 오류 상세 정보 출력

        _statusTxt.text = "로그인 실패ㅠㅠ";
    }

    // 회원가입
    public void Register()
    {
        //필드 빈칸 처리
        if (string.IsNullOrEmpty(_idInput.text))
        {
            _statusTxt.text = "아이디를 입력해주세요!";
            return;
        } 
        else if (string.IsNullOrEmpty(_pwInput.text)) 
        { 
            _statusTxt.text = "비밀번호를 입력해주세요!";
            return;
        }
        else if (string.IsNullOrEmpty(_nameInput.text)) 
        { 
            _statusTxt.text = "닉네임을 입력해주세요!";
            return;
        }

        // 이메일, 비밀번호, 사용자 이름을 사용하여 회원가입 요청 생성
        var request = new RegisterPlayFabUserRequest { Email = _idInput.text, Password = _pwInput.text, Username = _nameInput.text };
        // PlayFab API를 호출하여 사용자 등록 시도
        PlayFabClientAPI.RegisterPlayFabUser(request, RegisterSuccess, RegisterFailure);
    }

    // 회원가입 성공 시 호출 콜백
    private void RegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("가입 성공");
        _statusTxt.text = "가입 성공!";
    }

    // 회원가입 실패 시 호출 콜백
    private void RegisterFailure(PlayFabError error)
    {
        Debug.LogWarning("가입 실패");
        Debug.LogWarning(error.GenerateErrorReport()); // 오류 상세 정보 출력
        _statusTxt.text = "가입 실패ㅠㅠ";
    }

    // 현재 로그인한 사용자의 계정 정보 가져오기
    private void GetAccountInfo()
    {
        // 이메일을 사용하여 계정 정보 요청 생성
        var request = new GetAccountInfoRequest { Email = _idInput.text };
        // PlayFab API를 호출하여 계정 정보 가져오기
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    // 계정 정보 성공적으로 가져왔을 때 호출 콜백
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        // 계정 정보에서 사용자 이름을 가져옴
        string username = result.AccountInfo.Username;
        if (string.IsNullOrEmpty(username))
        {
            // 사용자 이름 없는 경우 이메일을 사용자 이름으로 사용
            username = result.AccountInfo.PrivateInfo.Email;
        }
        // NetManager 유저 닉네임 전해주면서 Connect
        NetManager.Instance.Connect(username);
    }

    // 계정 정보 가져오기 실패 시 호출 콜백
    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogWarning("계정 정보 가져오기 실패");
        Debug.LogWarning(error.GenerateErrorReport());
    }
}
