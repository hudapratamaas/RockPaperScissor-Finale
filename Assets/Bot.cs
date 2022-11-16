using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public CardPlayer player;
    public GameManager gameManager;
    public BotStats stats;

    private float timer = 0;
    int lastSelected = 0;
    Card[] cards;
    public bool IsReady = false;

public void SetStats(BotStats newStats, bool RestoreFullHealth = false)
{
    this.stats = newStats;
        var newPlayerStats = new PlayerStats
        {
            MaxHealth = this.stats.MaxHealth,
            RestoreValue = this.stats.RestoreValue,
            DamageValue = this.stats.DamageValue
        };
        player.SetStats(newPlayerStats, RestoreFullHealth);
}

    IEnumerator Start()
    {
        cards = player.GetComponentsInChildren<Card>();

        yield return new WaitUntil(() => player.IsReady);
        SetStats(this.stats);
        this.IsReady = true;
    }
    void Update()
    {
        if (gameManager.State != GameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }
        if (timer < stats.choosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }
        timer = 0;
        ChooseAttack();
    }
    public void ChooseAttack()
    {
        var random = Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;
        player.SetChosenCard(cards[selection]);
        lastSelected = selection;
    }
}