using UnityEngine;

public class StaticList<T> : ScriptableObject, ISerializationCallbackReceiver {
  [SerializeField]
  private T[] list;

  public T[] List {
    get { return this.list; }
  }

  public void OnAfterDeserialize() {}

  public void OnBeforeSerialize() {}
}
