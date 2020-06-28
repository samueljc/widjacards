public class CardModel {
  private CardDetails details;

  private int power;

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
    this.power = this.details.Power;
  }
}
