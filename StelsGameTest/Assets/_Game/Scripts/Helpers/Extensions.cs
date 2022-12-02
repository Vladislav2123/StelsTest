using UnityEngine;

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

    public static void SetMaterial(this MeshRenderer meshRenderer, int index, Material material)
    {
        Material[] materials = meshRenderer.materials;

        materials[index] = material;
        meshRenderer.materials = materials;
    }

    public static void SetMaterial(this MeshRenderer meshRenderer, Material material)
    {
        Material[] materials = meshRenderer.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = material;
        }

        meshRenderer.materials = materials;
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
}