using System;
using System.Collections.Generic;

namespace Quelea
{
  public class CircularArray<T>
  {
    protected readonly int size;
    protected int count;
    protected int head, tail;
    protected readonly T[] array;

    public CircularArray(int size)
    {
      head = tail = 0;
      this.size = size;
      array = new T[size];
    }

    public int Count
    {
      get { return count; }
    }

    public void Add(T item)
    {
      if (size > 0)
      {
        array[tail] = item;
        if (head == tail && count != 0)
        {
          head = (tail + 1) % size;
        }
        tail = (tail + 1) % size;

        count++;
        count = count > size ? size : count;
      }
    }

    public T Head
    {
      get { return array[head]; }
      set { array[head] = value; }
    }

    public T Get(int i)
    {
      return array[(head + i) % size];
    }

    public T[] ToArray()
    {
      T[] orderedArray = new T[count];
      int index = 0;
      for (int i = head; i < count; i++)
      {
        orderedArray[index] = array[i];
        index++;
      }
      if (count == size)
      {
        for (int i = 0; i < tail; i++)
        {
          orderedArray[index] = array[i];
          index++;
        }
      }
      return orderedArray;
    }

    // Custom defined mod because built in % does not loop negatives.
    protected int mod(int x, int m)
    {
      int r = x % m;
      return r < 0 ? r + m : r;
    }

    public List<T> ToList()
    {
      List<T> orderedList = new List<T>();
      if (count < size)
      {
        for (int i = tail - 1; orderedList.Count < count; i--)
        {
          orderedList.Add(array[i]);
        }
      }
      else
      {
        for (int i = mod(tail - 1, size); orderedList.Count < size; i = mod(i - 1, size))
        {
          orderedList.Add(array[i]);
        }
      }
      return orderedList;
    }

    public override String ToString()
    {
      String str = "{";
      T[] orderedArray = ToArray();
      foreach (T item in orderedArray)
      {
        str += item.ToString();
      }
      str += "}";
      return str;
    }
  }
}
