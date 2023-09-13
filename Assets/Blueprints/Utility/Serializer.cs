using UnityEngine;

/// <summary>
/// Classes that inherit this abstraction must mark themselves with the [System.Serializable] attribute
/// for serialization to work as expected
/// </summary>
public static class Serializer
{
    public static byte[] Serialize<T>(this T item)
        => System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(item));

    public static T Deserialize<T>(this byte[] data)
        => JsonUtility.FromJson<T>(System.Text.Encoding.UTF8.GetString(data)) ?? default;
}