using UnityEngine;

public static class UsefulMethods {

    public static Vector2 RotateVector2(Vector2 original, float degrees) {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(original.x * Mathf.Cos(radians) - original.y * Mathf.Sin(radians), original.x * Mathf.Sin(radians) + original.y * Mathf.Cos(radians));
    }

    public static Transform FindTransform(Transform parent, string name) {
        foreach (Transform transform in parent.GetComponentsInChildren<Transform>()) {
            if (transform.name == name) return transform;
        }
        return null;
    }

    public static float AngleBetween(Vector3 a, Vector3 b) {
        return Mathf.Acos(Vector3.Dot(a, b) / (a.magnitude * b.magnitude)) * Mathf.Rad2Deg;
    }

    public static bool LayerMaskContains(LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }
}
