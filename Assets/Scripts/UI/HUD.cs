using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private Transform bulletImageContainer;
    [SerializeField] private GameObject singleShotBulletImgPrefab;
    [SerializeField] private GameObject multipleShotBulletPrefab;
    [SerializeField] private GameObject inLevelPanel;
    [SerializeField] private GameObject levelCompletedPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Transform starSpriteContainer;
    [SerializeField] private Sprite startSprite;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button[] restartLevelButtons;

    private void Start() 
    {
        GameManager.Instance.OnGameOver += DisplayGameoverPanel;
        GameManager.Instance.OnLevelCompleted += DisplayLevelCompletedPanel;
        
        nextLevelButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        foreach(Button restartBtn in restartLevelButtons)
        {
            restartBtn.onClick.AddListener(() => {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });
        }
        
        if(SceneManager.GetActiveScene().buildIndex + 1 >= SceneManager.sceneCountInBuildSettings)
        {
            nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().fontSize = 24;
            nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Restart\nGame";
            
            nextLevelButton.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });
        }

        Player.Instance.Gun.OnBulletReduce += UpdateBulletCountUI;
        UpdateBulletCountUI(Player.Instance.Gun.GetCurrentBullet());
        UpdateLevelText();
    }

    private void UpdateBulletCountUI(int bulletAmount)
    {
        GameObject bulletImgPrefab = null;
        if(Player.Instance.Gun.GetCurrentWeaponType() == WeaponType.SingleShotGun)
        {
            bulletImgPrefab = singleShotBulletImgPrefab;
        } else if(Player.Instance.Gun.GetCurrentWeaponType() == WeaponType.MultipleShootGun)
        {
            bulletImgPrefab = multipleShotBulletPrefab;
        }

        foreach(Transform bulletImg in bulletImageContainer)
        {
            Destroy(bulletImg.gameObject);
        }

        int bulletCount = bulletAmount;
        for (int i = 0; i < bulletCount; i++)
        {
            Instantiate(bulletImgPrefab, bulletImageContainer);
        }
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }

    private void DisplayGameoverPanel()
    {
        DisablePanel(inLevelPanel);
        EnablePanel(gameOverPanel);
    }

    private void DisplayLevelCompletedPanel()
    {
        DisablePanel(inLevelPanel);
        EnablePanel(levelCompletedPanel);
        for (int i = 0; i < GameManager.Instance.GetStarCount; i++)
        {
            if(starSpriteContainer.GetChild(i).TryGetComponent<Image>(out Image image))
            {
                image.sprite = startSprite;
            }     
        }        
    }

    private void EnablePanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    private void DisablePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
