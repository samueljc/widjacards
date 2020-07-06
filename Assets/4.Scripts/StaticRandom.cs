using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrapper around <c>UnityEngine.Random</c>.
/// </summary>
public static class StaticRandom {
  /// <inheritdoc cref="M:UnityEngine.Random.Range(System.Int32,System.Int32)" />
  public static int Range(int min, int max) {
    return Random.Range(min, max);
  }

  /// <inheritdoc cref="M:UnityEngine.Random.Range(System.Single,System.Single)" />
  public static float Range(float min, float max) {
    return Random.Range(min, max);
  }

  /// <summary>
  /// Return a random vector between min [inclusive] and max [inclusive].
  /// </summary>
  /// <param name="min">The minimum vector.</param>
  /// <param name="max">The maximum vector.</param>
  /// <returns>A vector between the minimum and maximum vectors.</returns>
  public static Vector2 Range(in Vector2 min, in Vector2 max) {
    return new Vector2(Range(min.x, max.x), Range(min.y, max.y));
  }

  /// <summary>
  /// Return a random vector between min [inclusive] and max [inclusive].
  /// </summary>
  /// <param name="min">The minimum vector.</param>
  /// <param name="max">The maximum vector.</param>
  /// <returns>A vector between the minimum and maximum vectors.</returns>
  public static Vector3 Range(in Vector3 min, in Vector3 max) {
    return new Vector3(
      StaticRandom.Range(min.x, max.x),
      StaticRandom.Range(min.y, max.y),
      StaticRandom.Range(min.z, max.z)
    );
  }

  /// <summary>
  /// Creates a random quaternion
  /// </summary>
  /// <param name="min"></param>
  /// <param name="max"></param>
  /// <returns></returns>
  public static Quaternion Rotation(float min, float max) {
    return Quaternion.Euler(0f, 0f, Range(min, max));
  }

  /// <summary>
  /// Shuffle a list in place.
  /// </summary>
  /// <param name="list">The list to shuffle.</param>
  /// <typeparam name="T">The type of the list.</typeparam>
  public static void Shuffle<T>(this IList<T> list) {
    // Fisher-Yates shuffle
    for (int i = 0; i < list.Count - 2; ++i) {
      int j = StaticRandom.Range(i, list.Count);
      T temp = list[i];
      list[i] = list[j];
      list[j] = temp;
    }
  }
}
