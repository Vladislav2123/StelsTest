using UnityEngine;
using System.Collections.Generic;

public static class Extensions
{
    public static Vector3 GetRight(this Vector3 vector)
    {
        return Vector3.Cross(vector, Vector3.up.normalized);
    }

    public static float Angle(this Vector3 vector)
    {
        Vector3 vectorXZ = new Vector3(vector.x, 0, vector.z);
        return Vector3.Angle(vector, vectorXZ);
    }

    public static Vector3 GetVectorFromAngle(this float angle)
    {
        float angleRad = angle * (Mathf.PI / 180);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public static float GetAngleFromVectorFloat(this Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public static void SetMaterial(this Renderer renderer, int index, Material material)
    {
        Material[] materials = renderer.materials;

        materials[index] = material;
        renderer.materials = materials;
    }

    public static void SetMaterial(this Renderer renderer, Material material)
    {
        Material[] materials = renderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = material;
        }

        renderer.materials = materials;
    }

    public static float RoundTo(this float roundingValue, float roundValue)
    {
        roundingValue = (float)Mathf.Round(roundingValue / roundValue) * roundValue;
        return roundingValue;
    }

    public static float RoundToFloor(this float roundingValue, float roundValue)
    {
        roundingValue = (float)Mathf.Floor(roundingValue / roundValue) * roundValue;
        return roundingValue;
    }

    public static float RoundToCeil(this float roundingValue, float roundValue)
    {
        roundingValue = (float)Mathf.Ceil(roundingValue / roundValue) * roundValue;
        return roundingValue;
    }

    public static int PercentOf(this float current, float max)
    {
        return (int)((current / max) * 100);
    }

    public static bool Contains(this LayerMask layermask, int layer)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static T Random<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}