using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{
    public Button resourceButton;
    public Image resourceImage;
    public Text resourceDescription;
    public Text resourceUpgradeCost;
    public Text resourceUnlockCost;

    private ResourceConfig _config;

    private int _level = 1;

    public bool isUnlocked { get; private set; }

    private void Start()
    {
        resourceButton.onClick.AddListener(() =>
        {
            if (isUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }
        });
    }

    public void SetConfig(ResourceConfig config)
    {
        _config = config;

        resourceDescription.text = $"{_config.name} Lv.{_level}\n+{GetOutput().ToString("0")}";
        resourceUnlockCost.text = $"Unlock Cost\n{GetUpgradeCost()}";
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";

        SetUnlocked(_config.unlockCost == 0);
    }

    public double GetOutput()
    {
        return _config.output * _level;
    }

    public double GetUpgradeCost()
    {
        return _config.upgradeCost * _level;
    }

    public double GetUnlockCost()
    {
        return _config.unlockCost;
    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();
        if(GameManager.instance.TotalGold < upgradeCost)
        {
            return;
        }

        GameManager.instance.AddGold(-upgradeCost);
        _level++;

        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{_config.name} Lv.{_level}\n + {GetOutput().ToString("0")}";
    }

    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if(GameManager.instance.TotalGold < unlockCost)
        {
            return;
        }

        SetUnlocked(true);
        GameManager.instance.ShowNextResource();

        AchievementController.instance.UnlockAchievement(AchievementType.UnlockResource, _config.name);
    }

    public void SetUnlocked (bool unlocked)
    {
        isUnlocked = unlocked;
        resourceImage.color = isUnlocked ? Color.white : Color.grey;
        resourceUnlockCost.gameObject.SetActive(!unlocked);
        resourceUpgradeCost.gameObject.SetActive(unlocked);
    }
}
