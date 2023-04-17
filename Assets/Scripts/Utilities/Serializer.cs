using UnityEngine;

/// <summary>
/// Classes that inherit this abstraction must mark themselves with the [System.Serializable] attribute
/// for serialization to work as expected
/// </summary>
public static class Serializer
{
    public static byte[] Serialize<T>(this T item)
    {
        return System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(item));
    }

    public static T Deserialize<T>(this byte[] data, int index, int count)
    {
        var item = JsonUtility.FromJson<T>(System.Text.Encoding.UTF8.GetString(data, index, count));
        
        if (item != null)
        {
            return item;
        }
        
        Debug.LogError($"supplied data is null");
        return default;
    }
}