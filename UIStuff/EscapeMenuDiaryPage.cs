using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zorro.Core;

namespace DiaryWarning.UIStuff;

public class EscapeMenuDiaryPage : EscapeMenuPage
{
    public Button? backButton;
    public GameObject? content;

    private void Awake()
    {
        backButton = transform.Find("BackButton")?.GetComponent<Button>();
        content = transform.Find("Content")?.FindChildRecursive("Content")?.gameObject;
        
        if (backButton == null)
        {
            DiaryWarningMod.Logger.LogError("Failed to find the BackButton!");
            return;
        }
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnBackButtonClicked()
    {
        DiaryWarningMod.Logger.LogWarning($"{backButton}, {content}.");
        pageHandler.TransistionToPage<EscapeMenuMainPage>();
    }

    public void RefreshListings()
    {
        if(content == null)
        {
            // content = transform.Find("Content")?.FindChildRecursive("Content")?.gameObject;
            // if (content == null)
            // {
                DiaryWarningMod.Logger.LogError("Failed to find the Content GameObject!");
                return;
            // }
        }
        
        foreach (Transform child in content.transform) Destroy(child.gameObject);
        foreach (var unlock in DiaryWarningMod.UnlockedDiaryEntries)
        {
            var entryGameObj = Instantiate(DiaryWarningMod.mainAB.LoadAsset<GameObject>("Assets/WrittenDiaryEntry.prefab"), content.transform);
            var title = entryGameObj.transform.Find("Title")?.GetComponent<TMPro.TextMeshProUGUI>();
            var description = entryGameObj.transform.Find("Description")?.GetComponent<TMPro.TextMeshProUGUI>();
            if (title == null || description == null)
            {
                DiaryWarningMod.Logger.LogError("Failed to find the Title or Description Text!");
                return;
            }
            title.text = unlock.GetTitle();
            description.text = unlock.GetDescription();
        }
    }
}
