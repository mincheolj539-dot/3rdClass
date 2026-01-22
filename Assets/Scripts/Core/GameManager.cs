using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public float GameTime { get; private set; }
    public int Level { get; private set; }
    public int KillCount { get; private set; }
    
    public PlayerController Player;

    private void Start()
    {
        GameTime = 0f;
        Level = 1;
        KillCount = 0;
        
        // Find Player if not assigned
        if (Player == null)
            Player = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        GameTime += Time.deltaTime;
        
        // Example: Level up every 60 seconds
        int currentLevel = (int)(GameTime / 60) + 1;
        if (currentLevel > Level)
        {
            Level = currentLevel;
            Debug.Log("Level Up! Current Level: " + Level);
        }
    }

    public void AddKillCount(int amount = 1)
    {
        KillCount += amount;
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0; // Pause game
    }
}
