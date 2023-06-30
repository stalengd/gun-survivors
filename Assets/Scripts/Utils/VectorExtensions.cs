using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public static class VectorExtensions
{
    public static JObject ToJson(this Vector2Int vector)
    {
        return new JObject() { ["x"] = vector.x, ["y"] = vector.y };
    }

    public static Vector2Int Vector2IntValue(this JToken json)
    {
        return new Vector2Int(json["x"].Value<int>(), json["y"].Value<int>());
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithX(this Vector3 vector, float x)
    {
        vector.x = x;
        return vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithY(this Vector3 vector, float y)
    {
        vector.y = y;
        return vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 WithZ(this Vector3 vector, float z)
    {
        vector.z = z;
        return vector;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithX(this Vector2 vector, float x)
    {
        vector.x = x;
        return vector;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithY(this Vector2 vector, float y)
    {
        vector.y = y;
        return vector;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Slerp2d(this Vector2 from, Vector2 to, float v)
    {
        var diff = Vector2.SignedAngle(from, to);
        return Quaternion.Euler(0f, 0f, diff * v) * from;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        var radians = angle * Mathf.Deg2Rad;
        var ca = Mathf.Cos(radians);
        var sa = Mathf.Sin(radians);
        return new Vector2(ca * vector.x - sa * vector.y, sa * vector.x + ca * vector.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetAngle(this Vector2 vector)
    {
        var angle = Mathf.Atan2(vector.y, vector.x);
        var degrees = 180 * angle / Mathf.PI;
        return degrees;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector3 LinearRandom(this Vector3 from, Vector3 to)
    {
        return Vector3.Lerp(from, to, Random.value);
    }

    public static Vector3 BoxRandom(this Vector3 from, Vector3 to)
    {
        var result = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            if (from[i] < to[i])
                result[i] = Random.Range(from[i], to[i]);
            else
                result[i] = Random.Range(to[i], from[i]);
        }
        return result;
    }
}