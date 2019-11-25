using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UDictionary<K, V> : ISerializationCallbackReceiver
{
	//variables
	private Dictionary<K, V> backingDictionary = new Dictionary<K, V>();

	[SerializeField]
	private K[] keys;
	[SerializeField]
	private V[] values;
	private int length;

	public void OnBeforeSerialize()
	{
		if (keys != null && values != null)
		{
			ResetSize();

			length = keys.Length;

			int i = 0;
			foreach (var kvp in backingDictionary)
			{
				if (i < length) { break; }
				keys[i] = kvp.Key;
				values[i] = kvp.Value;
				i++;
			}
		}
	}

	public void OnAfterDeserialize()
	{
		if (keys != null && values != null)
		{
			ResetSize();

			backingDictionary = new Dictionary<K, V>();
			length = keys.Length;

			for (int i = 0; i < length; i++)
			{
				if (keys[i] == null) { continue; }
				
				if (!backingDictionary.ContainsKey(keys[i]))
				{
					backingDictionary.Add(keys[i], values[i]);
				}
				else
				{
					backingDictionary[keys[i]] = values[i];
				}
			}
		}
	}

	private void ResetSize()
	{
		if (keys.Length == values.Length) { return; }

		int newLength = keys.Length != length ? keys.Length : values.Length;
		int diff = length - newLength;
		int numOfElements = diff < 0 ? length : newLength;

		if (newLength != keys.Length) //if keys is different
		{
			K[] oldKeys = keys;
			keys = new K[newLength];

			for (int i = 0; i < numOfElements; i++)
			{
				keys[i] = oldKeys[i];
			}
		}
		else //if values are different
		{
			V[] oldVals = values;
			values = new V[newLength];

			for (int i = 0; i < numOfElements; i++)
			{
				values[i] = oldVals[i];
			}
		}
	}

	public void Add(K key, V value)
	{
		backingDictionary.Add(key, value);
	}

	public bool Remove(K key)
	{
		return backingDictionary.Remove(key);
	}

	public bool ContainsKey(K key)
	{
		return backingDictionary.ContainsKey(key);
	}

	public bool ContainsValue(V value)
	{
		return backingDictionary.ContainsValue(value);
	}

	public IEqualityComparer<K> Comparer
	{
		get
		{
			return backingDictionary.Comparer;
		}
	}


	public V this[K key]
	{
		get
		{
			return backingDictionary[key];
		}
		
		set
		{
			backingDictionary[key] = value;
		}
	}

	public IEnumerable<K> Keys
	{
		get
		{
			return backingDictionary.Keys;
		}
	}

	public IEnumerable<V> Values
	{
		get
		{
			return backingDictionary.Values;
		}
	}

	public int Count
	{
		get
		{
			return backingDictionary.Count;
		}
	}

	public bool Equals(Object obj)
	{
		return backingDictionary.Equals(obj);
	}

	public Dictionary<K, V>.Enumerator GetEnumerator()
	{
		return backingDictionary.GetEnumerator();
	}
}