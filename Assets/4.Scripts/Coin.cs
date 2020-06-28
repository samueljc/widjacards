using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin: MonoBehaviour {
  public delegate void Flipped(Coin clip, bool heads);
  public event Flipped OnFlipped;

  public bool heads = true;
  public bool flippable = true;

  private Animator coinAnimator;
  private bool flipping = false;
  private int flipCount = 0;

  public void Awake() {
    this.coinAnimator = GetComponent<Animator>();
  }

  public void Start() {
    // nothing right now
  }

  public void Update() {
    if (this.coinAnimator.GetBool("flipping") != this.flipping) {
      this.coinAnimator.SetBool("flipping", this.flipping);
    }
    if (this.coinAnimator.GetBool("heads") != this.heads) {
      this.coinAnimator.SetBool("heads", this.heads);
    }
  }

  public void OnMouseDown() {
    if (this.flippable) {
      Flip(Random.Range(8, 12));
    }
  }

  public void Flip(int flipCount) {
    if (!this.flipping && flipCount > 0) {
      StartCoroutine("DoFlips", flipCount);
    }
  }

  private IEnumerator DoFlips(int flipCount) {
    this.flipCount = flipCount;
    this.flipping = true;
    while (this.flipCount > 0) {
      yield return new WaitForEndOfFrame();
    }
    OnFlipped?.Invoke(this, this.heads);
    this.flipping = false;
  }

  public void SetHeads() {
    this.heads = true;
  }

  public void SetTails() {
    this.heads = false;
  }

  public void DecrementFlips() {
    --this.flipCount;
  }
}
