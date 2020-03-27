using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance = null;
    
    public List<GameObject> Worms = new List<GameObject>();
    public List<GameObject> Enemies =  new List<GameObject>();

    public AudioSource FoodEaten;
    public AudioSource EnemyDies;
    public AudioSource BossDies;

    public int FoodRemaining
    {
        get
        {
            return Worms.Count;
        }
    }

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void RegisterFood (GameObject food)
    {
        if (!Worms.Contains(food)) Worms.Add(food);
    }

    public void RegisterEnemy (GameObject enemy)
    {
        if (!Enemies.Contains(enemy)) Enemies.Add(enemy);
    }

    public void KillFood (GameObject food)
    {
        if (Worms.Contains(food)) Worms.Remove(food);
        if (FoodRemaining <= 0) WinGame();
    }

    public void KillEnemy (GameObject enemy)
    {
        if (Enemies.Contains(enemy)) Enemies.Remove(enemy);
    }

    public void PlayDeathSound (bool isFood, bool isBoss)
    {
        if (isFood && FoodEaten != null) FoodEaten.Play();
        if (isBoss && BossDies != null) BossDies.Play();
        if (!isFood && !isBoss && EnemyDies != null) EnemyDies.Play();
    }

    public void WinGame()
    {
        PlayerControl.Instance.GameState = PlayerControl.GameStates.Victory;
        ScoreController.Instance.GameInProgress = false;
        EndGame();
    }

    public void EndGame()
    {
        foreach (var worm in Worms)
            worm.SendMessage("EndGame");
        foreach (var enemy in Enemies)
            enemy.SendMessage("EndGame");
    }
}