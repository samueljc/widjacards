using System.Collections;
using System.Collections.Generic;

public class Inventory : IEnumerable<CardModel> {
  public delegate void OnChange();

  private event OnChange change;

  private int capacity;
  private List<CardModel> list;

  public bool Full {
    get { return this.Count >= this.Capacity; }
  }

  public int Count {
    get { return this.list.Count; }
  }

  public int Capacity {
    get { return this.capacity; }
  }

  public Inventory(int capacity) {
    this.capacity = capacity;
    this.list = new List<CardModel>(this.capacity);
  }

  public Inventory(List<CardModel> list) {
    this.list = list;
    this.capacity = list.Capacity;
  }

  public void AddChangeHandler(OnChange handler) {
    change += handler;
  }

  public void RemoveChangeHandler(OnChange handler) {
    change -= handler;
  }

  public bool Contains(CardModel card) {
    return this.list.Contains(card);
  }

  public bool Contains(string cardID) {
    return this.list.Find(c => cardID == c.ID) != null;
  }

  public CardModel Find(string cardID) {
    return this.list.Find(c => cardID == c.ID);
  }

  public void Add(CardModel card) {
    this.list.Add(card);
    this.change?.Invoke();
  }

  public void Shuffle() {
    this.list.Shuffle();
    this.change?.Invoke();
  }

  public void Remove(CardModel model) {
    if (this.list.Remove(model)) {
      this.change?.Invoke();
    }
  }

  public void Remove(string cardID) {
    int index = this.list.FindIndex(c => cardID == c.ID);
    if (index != -1) {
      this.list.RemoveAt(index);
      this.change?.Invoke();
    }
  }

  public CardModel Pop() {
    if (this.list.Count == 0) {
      return null;
    }
    CardModel card = this.list[this.list.Count - 1];
    this.list.RemoveAt(this.list.Count - 1);
    this.change?.Invoke();
    return card;
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the IEnumerable implementation which is in turn needed to
  /// use foreach loops on this class.
  /// </remarks>
  public IEnumerator<CardModel> GetEnumerator() {
    return this.list.GetEnumerator();
  }

  /// <inheritdoc />
  /// <remarks>
  /// Needed for the generic IEnumerable implementation.
  /// </remarks>
  IEnumerator IEnumerable.GetEnumerator() {
    return this.GetEnumerator();
  }
}
