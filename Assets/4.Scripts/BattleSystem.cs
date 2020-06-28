using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour {
  public Coin[] coinFactory;
  public GameObject critBubble;
  public GameObject missBubble;
  public GameObject hitBubble;

  public int coinCount = 3;

  // coinSpacing is the number of units between coins
  private const float coinSpacing = 1f;
  private Coin[] coins;
  private int flippedCoins = 0;

  public void Start() {
    CreateCoins();
    HideAttackBubbles();
    BeginAttack();
  }

  private void BeginAttack() {
    this.flippedCoins = 0;
    foreach (Coin coin in this.coins) {
      coin.flippable = true;
    }
  }

  private void OnFlipped(Coin coin, bool heads) {
    coin.flippable = false;
    ++this.flippedCoins;
    // After all the coins are flipped we want to show a modal denoting 
    // the power of the attack and perform the attack
    if (this.flippedCoins == this.coins.Length) {
      Invoke("PerformAttack", 0.2f);
    }
  }

  private void PerformAttack() {
    ShowAttackBubble(this.GetAttackStatus());
  }

  private void ShowAttackBubble(AttackStatus status) {
    switch (status) {
      case AttackStatus.HIT:
        hitBubble.SetActive(true);
        break;
      case AttackStatus.MISS:
        missBubble.SetActive(true);
        break;
      case AttackStatus.CRIT:
        critBubble.SetActive(true);
        break;
    }
    Invoke("HideAttackBubbles", 1.2f);
  }

  private void HideAttackBubbles() {
    if (hitBubble.activeSelf) {
      hitBubble.SetActive(false);
    }
    if (missBubble.activeSelf) {
      missBubble.SetActive(false);
    }
    if (critBubble.activeSelf) {
      critBubble.SetActive(false);
    }
    BeginAttack();
  }

  private AttackStatus GetAttackStatus() {
    int headCount = 0;
    foreach (Coin coin in this.coins) {
      if (coin.heads) {
        ++headCount;
      } else {
        --headCount;
      }
    }
    if (headCount == this.coins.Length) {
      // All heads is a crit
      return AttackStatus.CRIT;
    } else if (headCount < 0) {
      // Fewer heads than tails is a miss
      return AttackStatus.MISS;
    } else {
      return AttackStatus.HIT;
    }
  }

  private void CreateCoins() {
    coins = new Coin[coinCount];
    float leftOffset = ((coinCount - 1) / 2f) * -coinSpacing;
    float flex = 0.2f;
    for (int i = 0; i < coinCount; ++i) {
      Coin coin = Instantiate(
        coinFactory[Random.Range(0, coinFactory.Length)],
        new Vector3(
          leftOffset + i * coinSpacing + Random.Range(-flex, flex),
          Random.Range(-flex, flex),
          0
        ),
        Quaternion.Euler(0, 0, Random.Range(0, 360))
      );
      coin.heads = Random.Range(0, 2) == 0;
      coin.OnFlipped += OnFlipped;
      coins[i] = coin;
    }
  }
}
