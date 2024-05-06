using System.Linq;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zorro.UI;

namespace DiaryWarning.UIStuff;

public class EscapeMenuManagerAPIThingy
{
    internal static GameObject? EscapeMenu => Resources.FindObjectsOfTypeAll<Transform>()
        .FirstOrDefault(obj => obj.name == "EscapeMenu")?.gameObject;
    
    internal static GameObject? UIMenu
        => EscapeMenu?.transform
            .Find("MainPage")?.gameObject;

    internal static GameObject? UIButtonList
        => UIMenu?.transform.Find("LIST")?.gameObject;

    private static readonly GameObject templateButton =
        DiaryWarningMod.mainAB.LoadAsset<GameObject>("Assets/TemplateButton.prefab");

    internal static GameObject? AddButton(string name, string text, UnityAction action, int? index = null)
    {
        var uiButtonList = UIButtonList;
        if (uiButtonList == null)
        {
            DiaryWarningMod.Logger.LogError("Failed to find the EscapeMenu control list!");
            return null;
        }

        var button = Object.Instantiate(templateButton, uiButtonList.transform);
        button.name = name;
        
        if (index.HasValue) button.transform.SetSiblingIndex(index.Value);

        var uiButton = button.GetComponentInChildren<Button>();
        if (uiButton == null)
        {
            DiaryWarningMod.Logger.LogError("Failed to find the Button component!");
            Object.Destroy(button);
            return null;
        }

        var uiText = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if (uiText == null)
        {
            DiaryWarningMod.Logger.LogError("Failed to find the TextMeshProUGUI component!");
            Object.Destroy(button);
            return null;
        }

        uiButton.onClick.AddListener(action);
        uiText.SetCharArray(text.ToCharArray());
        uiText.SetAllDirty();
        return button;
    }
    
    internal static GameObject? AddPageWithComp<T>(string name, GameObject page) where T : EscapeMenuPage
    {
        var escapeMenu = EscapeMenu;
        if (escapeMenu == null)
        {
            DiaryWarningMod.Logger.LogError("Failed to find the EscapeMenu!");
            return null;
        }
        
        var pageObj = Object.Instantiate(page, escapeMenu.transform);
        pageObj.name = name;
        
        var comp = pageObj.AddComponent<T>();
        if (comp != null) return pageObj;
        
        DiaryWarningMod.Logger.LogError($"Failed to add the component {typeof(T).Name} to the page!");
        Object.Destroy(pageObj);
        return null;

    }
}