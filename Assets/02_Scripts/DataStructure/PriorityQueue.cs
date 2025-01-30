using System.Collections;
using System.Collections.Generic;
using System;

public class PriorityQueue<T> where T : IComparable<T>
{
	private List <T> _data;

	public PriorityQueue()
	{
		if(_data == null)
			_data = new List <T>();
	}

	public void Enqueue(T item)
	{
		_data.Add(item);
		BubbleUp();
	}

	public void Reposition(T item)
    {
		int index = _data.IndexOf(item);
		while (index > 0)
		{
			int pi = (index - 1) / 2;
			if (_data[index].CompareTo(_data[pi]) >= 0)
			{
				break;
			}
			Swap(index, pi);
			index = pi;
		}
	}

	private void BubbleUp()
    {
		int ci = Count - 1;
		while(ci > 0)
        {
			int pi = (ci - 1) / 2;
            if (_data[ci].CompareTo(_data[pi]) >= 0)
            {
				break;
            }
			Swap(ci, pi);
			ci = pi;
        }
    }

	private void Swap(int index1, int index2)
	{
		var tmp = _data[index1];
		_data[index1] = _data[index2];
		_data[index2] = tmp;
	}

	public T Dequeue()
	{
		T head = _data[0];

		MoveLastItemToTheTop();
		SinkDown();

		return head;
	}

	private void MoveLastItemToTheTop()
	{
		int li = Count - 1;
		_data[0] = _data[li];
		_data.RemoveAt(li);
	}

	private void SinkDown()
	{
		var li = Count - 1;
		var pi = 0;

		while (true)
		{
			var ci1 = pi * 2 + 1;
			if (ci1 > li)
			{
				break;
			}
			var ci2 = ci1 + 1;
			if (ci2 <= li && _data[ci2].CompareTo(_data[ci1]) < 0)
			{
				ci1 = ci2;
			}
			if (_data[pi].CompareTo(_data[ci1]) < 0)
			{
				break;
			}
			Swap(pi, ci1);
			pi = ci1;
		}
	}

	public bool Contains(T nodeToCheck)
    {
		return _data.Contains(nodeToCheck);
    }

	public int Count
	{
		get
        {
			return _data.Count;
		}
	}

	public T Head
    {
		get
        {
			return _data[0];
        }
    }
}
