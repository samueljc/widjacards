using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controller for managing the details panel.
/// </summary>
public class DetailsController : MonoBehaviour {
  /// <summary>
  /// A reference to the details panel.
  /// </summary>
  [SerializeField]
  private GameObject detailsPanel;

  /// <summary>
  /// A reference to the layout group.
  /// </summary>
  [SerializeField]
  private RectTransform detailsLayoutGroup;

  /// <summary>
  /// A reference to an image for populating the graphic.
  /// </summary>
  [SerializeField]
  private Image graphic;

  /// <summary>
  /// A reference to the text field for populating the title.
  /// </summary>
  [SerializeField]
  private TextMeshProUGUI title;

  /// <summary>
  /// A reference to the text field for populating the description.
  /// </summary>
  [SerializeField]
  private TextMeshProUGUI description;

  /// <summary>
  /// A reference to the text field for populating the power.
  /// </summary>
  [SerializeField]
  private TextMeshProUGUI power;

  /// <summary>
  /// A reference to the text field for populating the rarity.
  /// </summary>
  [SerializeField]
  private TextMeshProUGUI rarity;

  private LibraryCardModel model;

  /// <summary>
  /// The model for the card the details panels is describing.
  /// </summary>
  public LibraryCardModel Model {
    get { return this.model; }
    set {
      this.model = value;
      if (this.model == null) {
        this.Hide();
      } else {
        this.detailsPanel.SetActive(true);
        this.graphic.sprite = this.model.Graphic;
        this.graphic.SetNativeSize();
        this.title.text = this.model.Title;
        this.description.text = this.model.Description;
        this.power.text = this.model.Power.ToString();
        this.rarity.text = (this.model.Probability * 100f).ToString() + "%";

        // FIXME: This is awful. Because I have nested ContentSizeFitters with
        // the layout groups it's requiring multiple passes to get the layout
        // correct. To handle this I'm just forcing the layout to be rebuilt
        // relative to the level of nesting which works, but getting a better
        // layout group hierarchy seems like the way to go.
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.detailsLayoutGroup);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.detailsLayoutGroup);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.detailsLayoutGroup);
      }
    }
  }

  /// <summary>
  /// Hide the details panel.
  /// </summary>
  public void Hide() {
    this.detailsPanel.SetActive(false);
  }
}