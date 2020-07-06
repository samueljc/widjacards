using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A <c>ScriptableObject</c> representing a generic variable.
/// </summary>
/// <typeparam name="T">The underlying variable type.</typeparam>
/// <remarks>
/// <para>
/// A default value can be provided in the inspector that will be used when
/// deserializing instances of this object but the internal value will not
/// remember changes during runtime.
/// </para>
/// <para>
/// This class includes a change event handler to signify when the underlying
/// value has changed. The initial deserialization does not trigger a change
/// event.
/// </para>
/// </remarks>
public class Variable<T> : ScriptableObject, ISerializationCallbackReceiver {
  /// <summary>
  /// Delegate for handling changes to the underlying value.
  /// </summary>
  /// <param name="oldValue">The value before the change.</param>
  /// <param name="newValue">The value after the change.</param>
  public delegate void ChangeHandler(T oldValue, T newValue);

  /// <summary>
  /// Event handlers for change events.
  /// </summary>
  private event ChangeHandler change;

  /// <summary>
  /// The default value that will be given to any new instance.
  /// </summary>
  [SerializeField]
  private T defaultValue;

  /// <summary>
  /// The internal value of this object.
  /// </summary>
  /// <remarks>
  /// Will not be serialized.
  /// </remarks>
  [NonSerialized]
  private T internalValue;

  /// <summary>
  /// The underlying value of this object.
  /// </summary>
  /// <remarks>
  /// Changing the value will raise a change event. If the incoming value is
  /// the same as the existing value no event will be raised.
  /// </remarks>
  public T Value {
    get { return this.internalValue; }
    set {
      if (EqualityComparer<T>.Default.Equals(this.internalValue, value)) {
        return;
      }
      T oldValue = this.internalValue;
      this.internalValue = value;
      this.change?.Invoke(oldValue, this.internalValue);
    }
  }

  /// <summary>
  /// Add a handler to listen to variable changes.
  /// </summary>
  /// <param name="handler">The handler to add.</param>
  public void AddChangeHandler(ChangeHandler handler) {
    this.change += handler;
  }

  /// <summary>
  /// Remove a handler from listening to changes.
  /// </summary>
  /// <param name="handler">The handler to remove.</param>
  public void RemoveChangeHandler(ChangeHandler handler) {
    this.change -= handler;
  }

  /// <summary>
  /// Sets the internal value to the default value.
  /// </summary>
  public void OnAfterDeserialize() {
    // TODO: should this trigger a change? I don't think it should...
    this.internalValue = this.defaultValue;
  }

  /// <summary>
  /// Does not serialize any properties of the object.
  /// </summary>
  public void OnBeforeSerialize() {}
}
