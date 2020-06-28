using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardView : MonoBehaviour {
  [SerializeField]
  private TextMeshProUGUI title;

  [SerializeField]
  private TextMeshProUGUI description;

  [SerializeField]
  private TextMeshProUGUI power;

  [SerializeField]
  private Image typeIcon;

  [SerializeField]
  private Image graphic;

  [SerializeField]
  private Sprite heroIcon;

  [SerializeField]
  private Sprite neutralIcon;

  [SerializeField]
  private Sprite villainIcon;

  private CardModel model;

  public CardModel Model {
    get { return this.model; }

    set {
      this.model = value;
      this.title.text = this.model.Title;
      this.description.text = this.model.Description;
      this.power.text = this.model.Power.ToString();
      this.graphic.sprite = this.model.Graphic;
      // set the card type icon
      switch (this.model.Type) {
        case CardType.CHARACTER:
          this.power.gameObject.SetActive(true);
          break;
        case CardType.ENCHANTMENT:
          this.power.gameObject.SetActive(false);
          break;
        case CardType.SPELL:
          this.power.gameObject.SetActive(false);
          break;
      }
      // set the affiliation icon
      switch (this.model.Affiliation) {
        case Affiliation.HERO:
          this.typeIcon.sprite = heroIcon;
          break;
        case Affiliation.VILLAIN:
          this.typeIcon.sprite = villainIcon;
          break;
        case Affiliation.NONE:
          this.typeIcon.sprite = neutralIcon;
          break;
      }
      // set the other detail icons
    }
  }
}
