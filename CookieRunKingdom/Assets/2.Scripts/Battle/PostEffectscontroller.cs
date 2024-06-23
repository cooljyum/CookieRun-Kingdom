using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectsController : MonoBehaviour
{
    public Shader PostShader; // 포스트 프로세싱 셰이더
    private Material _postEffectMaterial; // 셰이더를 사용하기 위한 재질

    private void Awake()
    {
        // 셰이더를 기반으로 재질을 생성합니다.
        if (PostShader != null)
        {
            _postEffectMaterial = new Material(PostShader);
        }
        else
        {
            Debug.LogError("PostShader가 할당되지 않았습니다.");
        }
    }

    // 카메라가 화면에 렌더링할 때 호출됩니다.
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_postEffectMaterial == null)
        {
            // 포스트 프로세싱 재질이 없다면 원본 이미지를 그대로 렌더링합니다.
            Graphics.Blit(src, dest);
            return;
        }

        int width = src.width; // 원본 텍스처의 너비
        int height = src.height; // 원본 텍스처의 높이

        // 임시 렌더 텍스처를 생성합니다.
        RenderTexture startRenderTexture = RenderTexture.GetTemporary(width, height);

        // src 텍스처를 startRenderTexture로 블릿(복사)합니다.
        Graphics.Blit(src, startRenderTexture, _postEffectMaterial, 0);

        // startRenderTexture를 dest 텍스처로 블릿(복사)합니다.
        Graphics.Blit(startRenderTexture, dest);

        // 임시 렌더 텍스처를 해제합니다.
        RenderTexture.ReleaseTemporary(startRenderTexture);
    }
}