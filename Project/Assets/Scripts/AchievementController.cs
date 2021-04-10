using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    private static AchievementController _instance = null;
    public static AchievementController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AchievementController>();
            }

            return _instance;
        }
    }

    [SerializeField] private Transform _popUpTransform;
    [SerializeField] private Text _popUpText;
    [SerializeField] private float _popUpShowDuration = 3f;
    [SerializeField] private List<AchievementData> _achievementList;

    private float _popUpShowDurationCounter;

    private void Update()
    {
        if (_popUpShowDurationCounter > 0)
        {
            _popUpShowDurationCounter -= Time.unscaledDeltaTime;

            _popUpTransform.localScale = Vector3.LerpUnclamped(_popUpTransform.localScale, Vector3.one, 0.5f);
        }
        else
        {
            _popUpTransform.localScale = Vector2.LerpUnclamped(_popUpTransform.localScale, Vector3.right, 0.5f);
        }
    }

    public void UnlockAchievement(AchievementType type, string value)
    {
        AchievementData achievement = _achievementList.Find(a => a.type == type && a.value == value);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            ShowAchievementPopUp(achievement);
        }
    }

    private void ShowAchievementPopUp(AchievementData achievement)
    {
        _popUpText.text = achievement.title;
        _popUpShowDurationCounter = _popUpShowDuration;
        _popUpTransform.localScale = Vector2.right;
    }
}

[System.Serializable]
public class AchievementData
{
    public string title;
    public AchievementType type;
    public string value;
    public bool isUnlocked;
}

public enum AchievementType
{
    UnlockResource
}
