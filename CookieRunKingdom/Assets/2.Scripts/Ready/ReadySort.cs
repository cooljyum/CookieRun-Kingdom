using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySort : MonoBehaviour
{
    //ĳ���͸� ��ġ�� ��ġ���� �����ϴ� ����Ʈ
    private List<Transform> _detailPositions = new List<Transform>();
    public List<Transform> DetailPositions
    {  
        get { return _detailPositions; } 
    } 

    //���� ��ġ�� ĳ���͵��� �����ϴ� ����Ʈ
    private List<GameObject> _characters = new List<GameObject>();
    public List<GameObject> Characters
    {
        get { return _characters; }
    }

    private void Awake()
    {
        //�ش� �����ǿ� �ش��ϴ� ���� ��ġ Ʈ���� ���� ����Ʈ�� �߰�
        for (int i = 0; i < 3; i++)
        {
            _detailPositions.Add(transform.GetChild(i));
        }
    }
    
    //ĳ���͸� �߰��ϴ� �Լ�
    public void Add(GameObject character)
    {
        //����Ʈ�� ������� �� 0��(�߾�)��ġ�� ��ġ
        if (_characters.Count == 0)
        {
            character.transform.position = _detailPositions[0].position;
            _characters.Add(character);            
            return;
        }

        //����Ʈ�� �ϳ��� ĳ���Ͱ� ���� �� ���� ĳ���͸� 1��(�ϴ�)���� �ű��,
        //���ο� ĳ���͸� 2��(���)�� �߰�
        if(_characters.Count == 1)
        {
            _characters[0].transform.position = _detailPositions[1].position;
            character.transform.position = _detailPositions[2].position;

            _characters.Add(character);            
            return;
        }
    }

    //ĳ���͸� �����ϴ� �Լ�
    public void Remove(GameObject character)
    {
        //����Ʈ�� �� �� �̻��� ĳ���Ͱ� ���� ��,
        //���� ��� ĳ���� ���� �� ���� ĳ���ʹ� 0��(�߾�)���� �̵�
        if (_characters.Count > 1)
        {
            _characters.Remove(character);
            _characters[0].transform.position = _detailPositions[0].position;
        }
        _characters.Remove(character);      
    }

    //Ư�� ĳ���Ͱ� ����Ʈ�� ���ԵǾ� �ִ��� Ȯ���ϴ� �Լ�
    public bool Contains(GameObject character)
    {
        return _characters.Contains(character);
    }
    
    //����Ʈ�� ���� �Լ�
    public void Clear()
    {
        _characters.Clear();
    }

    //����Ʈ�� ũ�⸦ ��ȯ�ϴ� �Լ�
    public int GetSize() {  return _characters.Count; }
}
