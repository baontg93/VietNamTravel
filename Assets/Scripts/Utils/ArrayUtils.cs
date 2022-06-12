using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ArrayUtils
{
    public static Array CreateAndFillArray(Type type, int length)
    {
        var result = Array.CreateInstance(type, length);
        for (int ixItem = 0; ixItem < result.Length; ixItem++)
            result.SetValue(Activator.CreateInstance(type), ixItem);
        return result;
    }

    public static T[] CreateAndFillArray<T>(int length)
    {
        var type = typeof(T);
        var result = new T[length];
        for (int ixItem = 0; ixItem < result.Length; ixItem++)
            result[ixItem] = (T)Activator.CreateInstance(type);
        return result;
    }

    public static void Shuffle<T>(this T[] array)
    {
        for (int index = 0; index < array.Length; index++)
        {
            T temp = array[index];
            int randomIndex = UnityEngine.Random.Range(index, array.Length);
            array[index] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int index = 0; index < list.Count; index++)
        {
            T temp = list[index];
            int randomIndex = UnityEngine.Random.Range(index, list.Count);
            list[index] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static void AddNonExistItem<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
        }
    }

    public static int IndexOf<T>(this T[] arr, T value)
    {
        for (int index = 0; index < arr.Length; index++)
        {
            if (arr[index].Equals(value)) return index;
        }
        return -1;
    }

    public static int MaxIndexOf<T>(this T[] arr, T value) where T : IComparable
    {
        if (arr[0].CompareTo(value) > 0) return 0;

        for (int index = 0; index < arr.Length; index++)
        {
            if (arr[index].CompareTo(value) > 0) return index - 1;
        }
        return arr.Length - 1;
    }

    /// <summary>
    /// Convert elements in Array to String, using for Debug
    /// </summary>
    /// <typeparam name="T">Type of Array</typeparam>
    /// <param name="arr">Array for convert</param>
    /// <returns>String</returns>
    public static string ToDebugString<T>(this T[] arr)
    {
        return "{ " + string.Join(", ", (from item in arr select item.ToString()).ToArray()) + " }";
    }

    /// <summary>
    /// Convert elements in Array to String, using for Debug
    /// </summary>
    /// <typeparam name="T">Type of Array</typeparam>
    /// <param name="arr">Array for convert</param>
    /// <returns>String</returns>
    public static string ToDebugString<T>(this List<T> arr)
    {
        return "{ " + string.Join(", ", (from item in arr select item.ToString()).ToArray()) + " }";
    }
    /// <summary>
    /// Convert elements in Jagged Array to String, using for Debug
    /// </summary>
    /// <typeparam name="T">Type of Array</typeparam>
    /// <param name="arr">Array for convert</param>
    /// <returns>String</returns>
    public static string ToDebugString<T>(this T[][] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            return "Null Array";
        }
        else
        {
            string tempString = "";
            tempString += "\n";
            for (int index = 0; index < arr.Length; index++)
            {
                tempString += "\t[" + index + "]: " + string.Join(", ", (from item in arr[index] select item.ToString()).ToArray()) + "\n";
            }
            return tempString;
        }
    }

    /// <summary>
    /// Convert elements in Jagged Array to String, using for Debug
    /// </summary>
    /// <typeparam name="T">Type of Array</typeparam>
    /// <param name="arr">Array for convert</param>
    /// <returns>String</returns>
    public static string ToCorrectForm<T>(this T[][] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            return "Null Array";
        }
        else
        {
            string tempString = "new int[" + arr.Length + "][] {\n";
            for (int index = 0; index < arr.Length; index++)
            {
                tempString += "new int[" + arr[index].Length + "] {" + string.Join(", ", (from item in arr[index] select item.ToString()).ToArray()) + "},\n";
            }
            tempString += "};";
            return tempString;
        }
    }

    public static T GetRandomElement<T>(this T[] array)
    {
        return array[UnityEngine.Random.Range(0, array.Length)];
    }

    public static T GetDifferentRandomElement<T>(this T[] array, T element)
    {
        int currentIndex = Array.IndexOf(array, element);
        int index = UnityEngine.Random.Range(currentIndex + 1, currentIndex + array.Length) % array.Length;

        return array[index];
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T GetDifferentRandomElement<T>(this List<T> list, T element)
    {
        int currentIndex = Array.IndexOf(list.ToArray(), element);
        int index = UnityEngine.Random.Range(currentIndex + 1, currentIndex + list.Count) % list.Count;

        return list[index];
    }

    public static int Factorial(this int n, int m)
    {
        if (m == 0 || n == 1)
        {
            return 1;
        }

        int temp = n;
        for (int index = 0; index < m - 1; index++)
        {
            temp *= n;
        }
        return temp;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        return list == null || (list != null && list.Count == 0);
    }

    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        return array == null || (array != null && array.Length == 0);
    }
}