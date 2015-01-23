using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent
{
  public class CircularArray<T>
  {
    private int size, count;
    private int head, tail;
    private T[] array;

    public CircularArray(int size)
    {
      this.head = this.tail = 0;
      this.size = size;
      this.array = new T[size];
    }

    public void Add(T item)
    {
      if (this.size > 0)
      {
        this.array[this.tail] = item;
        if (this.head == this.tail && this.count != 0)
        {
          this.head = (this.tail + 1) % this.size;
        }
        this.tail = (this.tail + 1) % this.size;

        this.count++;
        this.count = this.count > this.size ? this.size : this.count;
      }
    }

    public T get(int i)
    {
      return this.array[(head + i) % this.size];
    }

    public T[] ToArray()
    {
      T[] orderedArray = new T[count];
      int index = 0;
      for (int i = head; i < this.count; i++)
      {
        orderedArray[index] = this.array[i];
        index++;
      }
      if (this.count == this.size)
      {
        for (int i = 0; i < this.tail; i++)
        {
          orderedArray[index] = this.array[i];
          index++;
        }
      }
      return orderedArray;
    }

    public List<T> ToList()
    {
      List<T> orderedList = new List<T>();
      for (int i = head; i < this.count; i++)
      {
        orderedList.Add(this.array[i]);
      }
      if (this.count == this.size)
      {
        for (int i = 0; i < this.tail; i++)
        {
          orderedList.Add(this.array[i]);
        }
      }
      return orderedList;
    }

    public override String ToString()
    {
      String str = "{";
      T[] orderedArray = this.ToArray();
      foreach (T item in orderedArray)
      {
        str += item.ToString();
      }
      str += "}";
      return str;
    }
  }
}
