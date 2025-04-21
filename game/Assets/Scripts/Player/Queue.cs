using System;

public class Queue<T>
{
    private T[] _array;
    private int _head;
    private int _tail;
    private int _size;
    private int _capacity;

    private const int DefaultCapacity = 4;

    public Queue() : this(DefaultCapacity)
    {
    }

    public Queue(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity cannot be negative.");
        }

        _array = new T[capacity];
        _head = 0;
        _tail = 0;
        _size = 0;
        _capacity = capacity;
    }

    public int Count
    {
        get { return _size; }
    }

    public void Enqueue(T item)
    {
        if (_size == _capacity)
        {
            Grow();
        }

        _array[_tail] = item;
        _tail = (_tail + 1) % _capacity;
        _size++;
    }

    public T Dequeue()
    {
        if (_size == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        T item = _array[_head];
        _array[_head] = default(T); // Optional: Clear the reference
        _head = (_head + 1) % _capacity;
        _size--;
        return item;
    }

    public T Peek()
    {
        if (_size == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
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
            Array.Clear(_array, _head, _capacity - _head);
            Array.Clear(_array, 0, _tail);
        }

        _head = 0;
        _tail = 0;
        _size = 0;
    }

    private void Grow()
    {
        int newCapacity = _capacity * 2;
        if (newCapacity < DefaultCapacity)
        {
            newCapacity = DefaultCapacity;
        }

        T[] newArray = new T[newCapacity];

        if (_head < _tail)
        {
            Array.Copy(_array, _head, newArray, 0, _size);
        }
        else
        {
            Array.Copy(_array, _head, newArray, 0, _capacity - _head);
            Array.Copy(_array, 0, newArray, _capacity - _head, _tail);
        }

        _array = newArray;
        _head = 0;
        _tail = _size;
        _capacity = newCapacity;
    }
}
