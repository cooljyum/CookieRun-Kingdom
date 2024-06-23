using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectsController : MonoBehaviour
{
    public Shader PostShader; // ����Ʈ ���μ��� ���̴�
    private Material _postEffectMaterial; // ���̴��� ����ϱ� ���� ����

    private void Awake()
    {
        // ���̴��� ������� ������ �����մϴ�.
        if (PostShader != null)
        {
            _postEffectMaterial = new Material(PostShader);
        }
        else
        {
            Debug.LogError("PostShader�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // ī�޶� ȭ�鿡 �������� �� ȣ��˴ϴ�.
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_postEffectMaterial == null)
        {
            // ����Ʈ ���μ��� ������ ���ٸ� ���� �̹����� �״�� �������մϴ�.
            Graphics.Blit(src, dest);
            return;
        }

        int width = src.width; // ���� �ؽ�ó�� �ʺ�
        int height = src.height; // ���� �ؽ�ó�� ����

        // �ӽ� ���� �ؽ�ó�� �����մϴ�.
        RenderTexture startRenderTexture = RenderTexture.GetTemporary(width, height);

        // src �ؽ�ó�� startRenderTexture�� ��(����)�մϴ�.
        Graphics.Blit(src, startRenderTexture, _postEffectMaterial, 0);

        // startRenderTexture�� dest �ؽ�ó�� ��(����)�մϴ�.
        Graphics.Blit(startRenderTexture, dest);

        // �ӽ� ���� �ؽ�ó�� �����մϴ�.
        RenderTexture.ReleaseTemporary(startRenderTexture);
    }
}