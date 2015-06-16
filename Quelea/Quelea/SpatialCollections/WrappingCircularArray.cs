using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;

namespace Quelea
{
  public class WrappingCircularArray<T> : CircularArray<T>
  {
    private readonly bool[] wrappedArray;

    public WrappingCircularArray(int size)
      : base(size)
    {
      wrappedArray = new bool[size];
    }

    public void Add(T item, bool wrapped)
    {
      if (size > 0)
      {
        array[tail] = item;
        wrappedArray[tail] = wrapped;
        if (head == tail && count != 0)
        {
          head = (tail + 1) % size;
        }
        tail = (tail + 1) % size;
        
        count++;
        count = count > size ? size : count;
      }
    }

    public DataTree<T> ToTree()
    {
      DataTree<T> tree = new DataTree<T>();
      int nextPathIndex = 0;
      if (count < size)
      {
        for (int i = tail - 1; tree.DataCount < count; i--)
        {
          tree.Add(array[i], new GH_Path(nextPathIndex));
          if (wrappedArray[i])
          {
            nextPathIndex++;
          }
        }
      }
      else
      {
        for (int i = mod(tail - 1, size); tree.DataCount < size; i = mod(i - 1, size))
        {
          tree.Add(array[i], new GH_Path(nextPathIndex));
          if (wrappedArray[i])
          {
            nextPathIndex++;
          }
        }
      }
      return tree;
    }
  }
}
