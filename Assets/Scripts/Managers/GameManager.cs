using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action OnGameOver;
    public event Action OnLevelCompleted;

    private List<Enemy> enemyList = new List<Enemy>();

    private bool isLevelCompleted;
    private bool isGameOver;
    private int levelStarCount;
    private float checkForDelayTimer;

    private void Awake() 
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable() 
    {
        Enemy.OnEnemySpawned += AddEnemyToList;
        Enemy.OnEnemyDead += RemoveEnemyFromList;
    }

    private void Update() 
    {
        if(isGameOver || isLevelCompleted) return;
        
        CalculateStars();

        if(Player.Instance.IsHitByEnemy) 
        {
            GameOver();
        }

        if(Player.Instance.Gun.GetCurrentBullet() > 0 && enemyList.Count == 0)
        {
            checkForDelayTimer += Time.deltaTime;
            if(checkForDelayTimer > 2f)
            {
                LevelCompleted();
            }
        }
        else if(Player.Instance.Gun.GetCurrentBullet() == 0)
        {
            checkForDelayTimer += Time.deltaTime;
            if(checkForDelayTimer > 2f)
            {
                CheckResultAfterDelay();
            }
        }
    }

    private void CheckResultAfterDelay()
    {
        if (enemyList.Count > 0)
        {
            GameOver();
        }
        else if (enemyList.Count == 0)
        {
            LevelCompleted();
        }
    }

    private void LevelCompleted()
    {
        OnLevelCompleted?.Invoke();
        Debug.Log("Level Completed....");
        Debug.Log("Start Count : " + levelStarCount);
        isLevelCompleted = true;
    }

    private void GameOver()
    {
        OnGameOver?.Invoke();
        Debug.Log("Game Over....");
        isGameOver = true;
    }

    private void AddEnemyToList(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    private void RemoveEnemyFromList(Enemy enemy)
    {
        if(!enemyList.Contains(enemy)) return;
        enemyList.Remove(enemy);
    }

    public int GetEnemyCount => enemyList.Count;
    public int GetStarCount => levelStarCount;

    private void CalculateStars()
    {
        float bulletPercent = Player.Instance.Gun.GetBulletPercent();
        if(bulletPercent >= 0.5f)
        {
            levelStarCount = 3;
        }
        else if(bulletPercent < 0.5f && bulletPercent >= 0.3f)
        {
            levelStarCount = 2;
        }
        else if(bulletPercent < 0.3f)
        {
            levelStarCount = 1;
        }
    }
}
