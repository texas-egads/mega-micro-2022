using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler {

    private TMP_Text pTextMeshPro;
    private Canvas canvas;

    private void Start() {
        pTextMeshPro = GetComponent<TMP_Text>();
        
        // search parent objects for the canvas
        GameObject currentObj = gameObject;
        while (canvas == null && currentObj != null) {
            currentObj = currentObj.transform.parent?.gameObject;
            canvas = currentObj?.GetComponent<Canvas>();
        }
        if (canvas == null)
            Debug.LogError("Couldn't find a canvas above this OpenHyperlinks component.");
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (pTextMeshPro == null)
            return;

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(
            pTextMeshPro,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );

        if (linkIndex != -1) {
            TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
            OpenLink(linkInfo.GetLinkID());
        }
    }

    public void OpenLink(string link) {
        Application.OpenURL(link);
    }
}