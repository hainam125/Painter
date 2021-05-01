using UnityEngine;

public class Paintable : MonoBehaviour {
    private const int TEXTURE_SIZE = 1024;

    public RenderTexture PaintTexture;
    public RenderTexture MaskTexture;
    private Renderer rend;

    public Renderer Renderer { get; private set; }

    private void Start() {
        PaintTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        PaintTexture.filterMode = FilterMode.Bilinear;
        MaskTexture = new RenderTexture(TEXTURE_SIZE, TEXTURE_SIZE, 0);
        MaskTexture.filterMode = FilterMode.Bilinear;

        Renderer = GetComponent<Renderer>();
        Renderer.material.SetTexture("_PaintTexture", PaintTexture);
        Renderer.material.SetTexture("_MaskTexture", MaskTexture);
    }

    private void OnDisable() {
        PaintTexture.Release();
        MaskTexture.Release();
    }
}
