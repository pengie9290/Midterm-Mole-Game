using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance = null;
    
    public List<GameObject> Worms = new List<GameObject>();
    public List<GameObject> Enemies =  new List<GameObject>();

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

    public void WinGame()
    {
        PlayerControl.Instance.GameState = PlayerControl.GameStates.Victory;
        ScoreController.Instance.GameInProgress = false;
    }

}

//Keep track of worm count
//Send signal to score, player, and NPCs that the game has ended