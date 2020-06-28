using UnityEngine;
using UnityEngine.EventSystems;

public class LibraryCardController : MonoBehaviour, IPointerClickHandler {
  public delegate void ClickHandler(LibraryCardModel model);

  private event ClickHandler click;

  public LibraryCardModel Model;

  public void AddClickHandler(ClickHandler handler) {
    this.click += handler;
  }

  public void RemoveClickHandler(ClickHandler handler) {
    this.click -= handler;
  }

  public void OnPointerClick(PointerEventData eventData) {
    this.click?.Invoke(this.Model);
  }
}
