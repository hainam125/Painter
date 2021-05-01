using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : MonoBehaviour {
    public static PaintManager Instance { get; private set; }

    [SerializeField] private Shader texturePaint;

    private Material paintMaterial;
    private Material maskMaterial;

    private CommandBuffer command;

    private void Awake() {
        Instance = this;
        paintMaterial = new Material(texturePaint);
        maskMaterial = new Material(texturePaint);
        command = new CommandBuffer();
        command.name = "CommandBuffer - " + gameObject.name;
    }

    public void Paint(Paintable paintable, Vector3 pos, float radius, float hardness, float strength, Color paintColor) {
        var paintTex = paintable.PaintTexture;
        var maskTex = paintable.MaskTexture;
        var renderer = paintable.Renderer;

        SetupPaint(paintMaterial, paintTex, pos, radius, hardness, strength, paintColor);
        command.SetRenderTarget(paintTex);
        command.DrawRenderer(renderer, paintMaterial, 0);

        SetupPaint(maskMaterial, maskTex, pos, radius, hardness, strength, Color.white);
        command.SetRenderTarget(maskTex);
        command.DrawRenderer(renderer, maskMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    private void SetupPaint(Material mat, RenderTexture tex, Vector3 pos, float radius, float hardness, float strength, Color paintColor) {
        mat.SetVector("_PainterPosition", pos);
        mat.SetFloat("_Hardness", hardness);
        mat.SetFloat("_Strength", strength);
        mat.SetFloat("_Radius", radius);
        mat.SetTexture("_MainTex", tex);
        mat.SetColor("_PainterColor", paintColor);
    }
}
