public class LibraryCardModel : CardModel {
  private float probability;

  public float Probability {
    get { return this.probability; }
  }

  public LibraryCardModel(CardDetails details, float probability) : base(details) {
    this.probability = probability;
  }
}
