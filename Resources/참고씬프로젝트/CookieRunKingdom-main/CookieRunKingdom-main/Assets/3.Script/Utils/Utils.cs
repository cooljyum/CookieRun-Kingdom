using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static readonly float TouchTime = 0.15f;  // 터치로 인식되는 시간

    public static readonly int JellyTime = 300;

    public static readonly Vector3 Dir = new Vector3(7.72f, 3.86f, 0);
    public static readonly float KnockBackPower = 3f;


    public static readonly float SKillLevelUpInterest = 1.04f;
    public static readonly float LevelUpInterest = 1.045f;
    public static readonly float EvolutionInterest = 1.05f;

    public static readonly Vector2 MapStartPoint = new Vector2(-100, -100);
    public static readonly Vector2 MapEndPoint = new Vector2(100, 100);

    /// <summary>
    /// 모든 자기의 자식들을 Destroy해주는 함수
    /// </summary>
    /// <param name="parent">자식을 다 없애줄 부모 오브젝트</param>
    public static void DestroyAllChild(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (!child.Equals(parent))
                GameObject.Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 정해준 index부터 모든 자식을 Destory해주는 함수
    /// </summary>
    /// <param name="parent">자식을 없애줄 부모 오브젝트</param>
    /// <param name="index">몇번째 자식부터 없애줄지 결정</param>
    public static void DestroyChildren(this Transform parent, int index)
    {
        int currentIndex = 0;

        foreach (Transform child in parent)
        {
            if (!child.Equals(parent))
            {
                if (currentIndex >= index)
                    GameObject.Destroy(child.gameObject);

                currentIndex++;
            }
        }
    }

    /// <summary>
    /// 초를 넣으면 시간 문자열로 반환해주는 메소드
    /// </summary>
    /// <param name="time">초</param>
    /// <returns>초를 변환한 시간 문자열</returns>
    public static string GetTimeText(int time, bool isString = true)
    {
        int hour = time / 3600;
        time %= 3600;
        int min = time / 60;
        time %= 60;
        int sec = time;

        string result = "";

        if(isString)
        {
            if (hour != 0)
                result += hour + "시간 ";
            if (min != 0)
                result += min + "분 ";

            result += sec + "초";
        }
        else
        {
            if (min < 10)
                result += "0" + min;
            else
                result += min.ToString();

            result += ":";

            if (sec < 10)
                result += "0" + sec;
            else
                result += sec.ToString();
        }

        return result;
    }


    /// <summary>
    /// 해당 트랜스폼의 위치를 그리드에 맞춰서 이동시킵니다.
    /// </summary>
    /// <param name="transform">이동시킬 트랜스폼</param>
    public static void SetGridTransform(this Transform transform)
    {
        Vector3Int pos = GridManager.Instance.Grid.WorldToCell(transform.position);
        transform.position = GridManager.Instance.Grid.CellToWorld(pos);
    }

    /// <summary>
    /// 스프라이트를 텍스쳐2D로 변환해주는 메소드
    /// </summary>
    /// <param name="sprite">변환할 스프라이트</param>
    /// <returns>변환된 텍스쳐2D</returns>
    public static Texture2D TextureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    
    /// <summary>
    /// 부모의 활성화 비활성화 상태를 판별하는 메소드
    /// 부모가 없다면 자기자신의 상태를 반환
    /// </summary>
    /// <param name="recursion">true하면 부모를 계속 타고 올라가면서 체크</param>
    /// <returns>활성화가 되어있으면 true, 아니면 false</returns>
    public static bool CheckParentActive(this Transform transform, bool recursion = false)
    {
        if (transform.parent == null)
            return transform.gameObject.activeSelf;

        if(recursion)
        {
            if (!transform.parent.gameObject.activeSelf)
                return false;
            else
                return CheckParentActive(transform.parent, recursion);
        }
        else
        {
            return transform.parent.gameObject.activeSelf;
        }

    }
}
