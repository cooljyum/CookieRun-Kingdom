using System.Collections.Generic;

public class PriorityQueue<T>
{
	private List<Pair<T, int>> _heap = new List<Pair<T, int>>();

	public bool IsEmpty => _heap.Count == 0;

	public void Enqueue(Pair<T, int> node)
	{
		_heap.Add(node);

		// 지금 추가한 노드의 인덱스
		int now = _heap.Count - 1;

		while (now > 0)
		{
			int next = (now - 1) / 2;

			// 부모노드가 우선순위가 더 크면 종료
			if (_heap[now].Second < _heap[next].Second)
				break;

			// 아니라면 올라가면서 교체
			Pair<T, int> temp = _heap[now];
			_heap[now] = _heap[next];
			_heap[next] = temp;
			now = next;
		}
	}

	public Pair<T, int> Dequeue()
	{
		Pair<T, int> result = _heap[0];
		int lastIdx = _heap.Count - 1;

		_heap[0] = _heap[lastIdx];
		_heap.RemoveAt(lastIdx);
		lastIdx--;

		int now = 0;
		while (true)
		{
			int left = 2 * now + 1;
			int right = 2 * now + 2;

			int next = now;

			if (left <= lastIdx && _heap[next].Second < _heap[left].Second)
				next = left;
			if (right <= lastIdx && _heap[next].Second < _heap[right].Second)
				next = right;

			if (now == next)
				break;

			Pair<T, int> temp = _heap[now];
			_heap[now] = _heap[next];
			_heap[next] = temp;
			now = next;
		}

		return result;
	}

	public bool Contains(T node)
    {
		for(int i = 0; i < _heap.Count; i++)
			if (_heap[i].First.Equals(node))
				return true;
		return false;
    }
}
