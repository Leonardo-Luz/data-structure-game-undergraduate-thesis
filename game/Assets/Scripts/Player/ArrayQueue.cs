using System;

[System.Serializable]
public class ArrayQueue<T>
{
  private T[] _array;
  private int _head;
  private int _tail;
  private int _size;

  private const int DefaultCapacity = 4;

  public ArrayQueue() : this(DefaultCapacity)
  {
  }

  public ArrayQueue(int capacity)
  {
    if (capacity < 0)
    {
      throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
    }

    _array = new T[capacity];
    _head = 0;
    _tail = 0;
    _size = 0;
  }

  public int Count
  {
    get
    {
      return _size;
    }
  }

  public void Enqueue(T item)
  {
    if (_size == _array.Length)
    {
      throw new InvalidOperationException("ArrayQueue is full.");
    }

    _array[_tail] = item;
    _tail = (_tail + 1) % _array.Length;
    _size++;
  }

  public T Dequeue()
  {
    if (_size == 0)
    {
      throw new InvalidOperationException("ArrayQueue is empty.");
    }

    T item = _array[_head];
    _array[_head] = default(T);
    _head = (_head + 1) % _array.Length;
    _size--;
    return item;
  }

  public T Peek()
  {
    if (_size == 0)
    {
      throw new InvalidOperationException("ArrayQueue is empty.");
    }

    return _array[_head];
  }

  public void Clear()
  {
    if (_head < _tail)
    {
      Array.Clear(_array, _head, _size);
    }
    else
    {
      Array.Clear(_array, _head, _array.Length - _head);
      Array.Clear(_array, 0, _tail);
    }

    _head = 0;
    _tail = 0;
    _size = 0;
  }
}
