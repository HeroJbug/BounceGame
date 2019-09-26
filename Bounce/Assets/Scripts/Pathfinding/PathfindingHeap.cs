using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathfindingHeap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public PathfindingHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T newItem)
    {
        newItem.HeapIdx = currentItemCount;
        items[currentItemCount] = newItem;
        SortUp(newItem);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T first = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIdx = 0;
        SortDown(items[0]);
        return first;
    }

    public int HeapCount
    {
        get
        {
            return currentItemCount;
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIdx], item);
    }

    void SortDown(T newItem)
    {
        while(true)
        {
            int childIdxLeft = newItem.HeapIdx * 2 + 1;
            int childIdxRight = newItem.HeapIdx * 2 + 2;
            int swapIdx = 0;

            if(childIdxLeft < currentItemCount)
            {
                swapIdx = childIdxLeft;

                if(childIdxRight < currentItemCount)
                {
                    if(items[childIdxLeft].CompareTo(items[childIdxRight]) < 0)
                    {
                        swapIdx = childIdxRight;
                    }
                }

                if(newItem.CompareTo(items[swapIdx]) < 0)
                {
                    Swap(newItem, items[swapIdx]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T newItem)
    {
        int parentIdx = (newItem.HeapIdx - 1)/2;
        while(true)
        {
            T parentItem = items[parentIdx];
            if(newItem.CompareTo(parentItem) > 0)
            {
                Swap(newItem, parentItem);
            }
            else
            {
                break;
            }
            parentIdx = (newItem.HeapIdx - 1) / 2;
        }
    }

    void Swap(T a, T b)
    {
        items[a.HeapIdx] = b;
        items[b.HeapIdx] = a;
        int temp = a.HeapIdx;
        a.HeapIdx = b.HeapIdx;
        b.HeapIdx = temp;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIdx
    {
        get;
        set;
    }
}
