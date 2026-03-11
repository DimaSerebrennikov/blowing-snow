using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class RenderTextureSimpleDrawer : MonoBehaviour {
    [SerializeField] RenderTexture renderTexture;
    public void Draw() {
        if (renderTexture == null) {
            Debug.LogWarning("RenderTexture not assigned");
            return;
        }
        Texture2D tex = new(renderTexture.width, renderTexture.height);
        for (int y = 0; y < tex.height; y++) {
            for (int x = 0; x < tex.width; x++) {
                tex.SetPixel(x, y, Color.red);
            }
        }
        tex.Apply();
        Graphics.Blit(tex, renderTexture);
        DestroyImmediate(tex);
    }
}
