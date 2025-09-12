using System;

[System.Serializable]
public class ArrayStack<T>
{
  private T[] _array;
  private int _size;
  private int _top;

  private const int DefaultCapacity = 4;

  public ArrayStack() : this(DefaultCapacity)
  {
  }

  public ArrayStack(int capacity)
  {
    if (capacity < 0)
    {
      throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
    }

    _array = new T[capacity];
    _top = 0;
    _size = 0;
  }

  public int Count
  {
    get { return _size; }
  }

  public void Push(T item)
  {
    if (_size == _array.Length)
    {
      throw new InvalidOperationException("Stack is full.");
    }

    _array[_top] = item;
    _top = (_top + 1) % _array.Length;
    _size++;
  }

  public T Pop()
  {
    if (_size == 0)
    {
      throw new InvalidOperationException("Stack is empty.");
    }

    _top = (_top - 1 + _array.Length) % _array.Length;
    T item = _array[_top];
    _array[_top] = default(T);
    _size--;
    return item;
  }

  public T Peek()
  {
    if (_size == 0)
    {
      throw new InvalidOperationException("Stack is empty.");
    }

    int lastItemIndex = (_top - 1 + _array.Length) % _array.Length;
    return _array[lastItemIndex];
  }

  public void Clear()
  {
    if (_size > 0)
    {
      int start = (_top - _size + _array.Length) % _array.Length;
      if (start < _top)
      {
        Array.Clear(_array, start, _size);
      }
      else
      {
        Array.Clear(_array, start, _array.Length - start);
        Array.Clear(_array, 0, _top);
      }
    }

    _top = 0;
    _size = 0;
  }
}
