using UnityEngine;

public class PerformanceUI : MonoBehaviour
{
    private float _deltaTime = 0.0f;

    private void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float fps = 1.0f / _deltaTime;
        string text = string.Format("{0:0.} FPS", fps);
        
        int enemyCount = EnemyManager.Instance != null ? EnemyManager.Instance.ActiveEnemyCount : 0;
        string enemyText = "Enemy: " + enemyCount;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 50;
        style.normal.textColor = Color.yellow;

        GUI.Label(new Rect(50, 50, 400, 100), text, style);
        GUI.Label(new Rect(50, 150, 400, 100), enemyText, style);
    }
}
