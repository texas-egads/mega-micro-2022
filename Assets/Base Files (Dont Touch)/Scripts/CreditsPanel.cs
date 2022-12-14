using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private Image screenshotImage;

    public void SetMinigame(MinigameDefinition minigame) {
        SetContent(minigame.title, minigame.creditsText, minigame.creditsScreenshot);
    }

    public void SetContent(string header, string body, Sprite screenshot) {
        headerText.text = header;
        bodyText.text = body;

        if (screenshot != null) {
            screenshotImage.sprite = screenshot;
        }
    }


#if UNITY_EDITOR
    private void Start() {
        TestLoadMinigame();
    }

    [ContextMenu("Load Minigame")]
    private void LoadMinigamePressed() {
        UnityEditor.Undo.RecordObjects(new Object[] { headerText, bodyText, screenshotImage }, "Load Minigame");
        TestLoadMinigame();
        UnityEditor.EditorUtility.SetDirty(headerText);
        UnityEditor.EditorUtility.SetDirty(bodyText);
        UnityEditor.EditorUtility.SetDirty(screenshotImage);
    }

    private bool TestLoadMinigame() {
        // we automatically search for the jammer's minigame asset and use that
        string[] guids = UnityEditor.AssetDatabase.FindAssets($"t:{typeof(MinigameDefinition)}");
        if (guids.Length > 0) {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            MinigameDefinition def = UnityEditor.AssetDatabase.LoadAssetAtPath<MinigameDefinition>(path);
            if (def != null) {
                SetMinigame(def);
                return true;
            }
        }

        Debug.LogError("Failed to load a minigame asset to display test credits. Did you create one?");
        return false;
    }
#endif

}
