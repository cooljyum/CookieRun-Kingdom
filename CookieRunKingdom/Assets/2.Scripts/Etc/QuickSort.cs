using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : MonoBehaviour
{
    private List<int> _data;

    public QuickSort(List<int> data)
    {
        this._data = data;
    }

    public List<int> Sort() 
    {
        QuickSorting(0, _data.Count - 1);
        return _data;
    }

    private void QuickSorting(int low, int high) //Devide�Լ� �ݺ� ����
    {
        if (low < high)
        {
            int pivot = Devide(low, high);
            QuickSorting(low, pivot - 1);
            QuickSorting(pivot + 1, high);
        }
    }

    private int Devide(int low, int high) //pivot�������� ū ���� ���� �� �и�
    {
        int pivot = _data[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (_data[j] < pivot)
            {
                i++;
                Swap(i, j);
            }
        }
        Swap(i + 1, high);
        return i + 1;
    }

    private void Swap(int i, int j) // �и� �� ���� ��ȯ
    {
        int temp = _data[i];
        _data[i] = _data[j];
        _data[j] = temp;
    }
}
