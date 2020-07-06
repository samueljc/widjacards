public class CardModel {
  private CardDetails details;

  private string id;

  private int power;

  public string ID {
    get { return this.id; }
  }

  public string Title {
    get { return this.details.Title; }
  }

  public string Description {
    get { return this.details.Description; }
  }

  public UnityEngine.Sprite Graphic {
    get { return this.details.Graphic; }
  }

  public Affiliation Affiliation {
    get { return this.details.Affiliation; }
  }

  public CardType Type {
    get { return this.details.type; }
  }

  public int Power {
    get { return this.power; }
  }

  public CardModel(in CardDetails details) {
    this.details = details;
    this.id = System.Guid.NewGuid().ToString();
    this.power = this.details.Power;
  }

  /*
  public override bool Equals(object obj) {
    CardModel other = obj as CardModel;
    if (other == null) {
      return false;
    }
    return this.id == other.id;
  }

  public override int GetHashCode() {
    return this.id.GetHashCode();
  }
  */
}
