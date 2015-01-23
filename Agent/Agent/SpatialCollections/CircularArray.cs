using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agent.SpatialCollections
{
  class CircularArray<T>
  {
    private int size, count;
    private int head, tail;
    private T[] array;

    public CircularArray(int size)
    {
      this.head = this.tail = 0;
      this.size = size;
      this.tail = 0;
      this.array = new T[size];
    }

    private void Add(T item)
    {
      this.array[this.tail] = item;
      this.tail++;
      if (this.tail >= this.size)
      {
        this.tail = 0;
      }
    }

    private T[] ToArray()
    {
      T[] orderedArray = new T[count];
      int index = 0;
      for (int i = head; i < this.count; i++)
      {
        orderedArray[index] = this.array[i];
        index++;
      }
      for (int i = 0; i < this.tail; i++)
      {
        orderedArray[index] = this.array[i];
        index++;
      }
      return orderedArray;
    }
  }
}
