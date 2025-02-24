using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ReloadUI : MonoBehaviour
{
    public Image reloadCircle; // Assign in Inspector
    private PlayerShooting playerShooting;

    void Start()
    {
        playerShooting = FindFirstObjectByType<PlayerShooting>(); // Get PlayerShooting script
        reloadCircle.fillAmount = 0; // Start empty
    }

    public void StartReloadUI(float reloadTime)
    {
        StartCoroutine(UpdateReloadCircle(reloadTime));
    }

    IEnumerator UpdateReloadCircle(float reloadTime)
    {
        float timer = 0f;
        while (timer < reloadTime)
        {
            timer += Time.deltaTime;
            reloadCircle.fillAmount = timer / reloadTime; // Fill over time
            yield return null;
        }
        reloadCircle.fillAmount = 0; // Reset after reload
    }
}
