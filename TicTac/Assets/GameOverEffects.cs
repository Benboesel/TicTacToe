using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverEffects : Singleton<GameOverEffects>
{
    [SerializeField] private GameOverEffect playerWinPrefab;
    [SerializeField] private GameOverEffect tiePrefab;
    [SerializeField] private GameOverEffect aiWinPrefab;

    private Color staticSkyTop;
    private Color staticSkyHorizon;
    private Color staticSkyBottom;
    private float skyFadeTime = 1f;
    [SerializeField] private List<MeshRenderer> boardMeshRenders;

    private Dictionary<MeshRenderer, Color> staticRendererColors;

    public void Start()
    {
        staticSkyTop = RenderSettings.skybox.GetColor("_Color1");
        staticSkyHorizon = RenderSettings.skybox.GetColor("_Color2");
        staticSkyBottom = RenderSettings.skybox.GetColor("_Color3");

        TicTacToe.Instance.OnPlayerWin += OnPlayerWin;
        TicTacToe.Instance.OnAIWin += OnAIWin;
        TicTacToe.Instance.OnTie += OnTie;
        staticRendererColors = new Dictionary<MeshRenderer, Color>();
        foreach(MeshRenderer meshRenderer in boardMeshRenders)
        {
            staticRendererColors.Add(meshRenderer, meshRenderer.material.color);
        }
    }

    public void OnTie()
    {
        GameOver(tiePrefab);
    }

    public void OnPlayerWin()
    {
        GameOver(playerWinPrefab);
    }

    public void OnAIWin()
    {
        GameOver(aiWinPrefab);
    }

    public void EffectsOver()
    {
        StartCoroutine(FadeSkybox(staticSkyTop, staticSkyHorizon, staticSkyBottom));
        foreach (KeyValuePair<MeshRenderer, Color> boardRenderer in staticRendererColors)
        {
            StartCoroutine(FadeBoardColor(boardRenderer.Value, boardRenderer.Key));
        }
        TicTacToe.Instance.NewGame();
    }

    private void GameOver(GameOverEffect gameOverPrefab)
    {
        GameOverEffect effect = Instantiate(gameOverPrefab, this.transform) as GameOverEffect;
        effect.transform.localPosition = Vector3.zero;
        effect.transform.localRotation = Quaternion.identity;
        StartCoroutine(FadeSkybox(effect.SkyTopColor, effect.SkyHorizonColor, effect.SkyBottomColor));
        foreach(MeshRenderer meshRenderer in boardMeshRenders)
        {
            StartCoroutine(FadeBoardColor(effect.BoardColor, meshRenderer));
        }
        foreach(Cell cell in TicTacToe.Instance.GetFilledCells())
        {
            StartCoroutine(FadeBoardColor(effect.BoardColor, cell.FilledGamePiece.MeshRenderer));
        }
        //Color Pieces
    }

    private IEnumerator FadeBoardColor(Color color, MeshRenderer meshRenderer)
    {
        float animationStartTime = Time.time;
        Color initialColor = meshRenderer.material.color;
        while (true)
        {
            if(meshRenderer == null)
            {
                break;
            }
            float percentageDone = (Time.time - animationStartTime) / skyFadeTime;
            if (percentageDone > 1)
            {
                percentageDone = 1;
            }
            meshRenderer.material.color = Color.Lerp(initialColor, color, percentageDone);
            if (percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeSkybox(Color topColor, Color horizonColor, Color bottomColor)
    {
        Color initialSkyTop = RenderSettings.skybox.GetColor("_Color1");
        Color initialSkyHorizon = RenderSettings.skybox.GetColor("_Color2");
        Color initialSkyBottom = RenderSettings.skybox.GetColor("_Color3");
        float animationStartTime = Time.time;

        while (true)
        {
            float percentageDone = (Time.time - animationStartTime) / skyFadeTime;
            if (percentageDone > 1)
            {
                percentageDone = 1;
            }
            RenderSettings.skybox.SetColor("_Color1", Color.Lerp(initialSkyTop, topColor, percentageDone));
            RenderSettings.skybox.SetColor("_Color2", Color.Lerp(initialSkyHorizon, horizonColor, percentageDone));
            RenderSettings.skybox.SetColor("_Color3", Color.Lerp(initialSkyBottom, bottomColor, percentageDone));
            if (percentageDone == 1)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void OnApplicationQuit()
    {
        SetSkyBoxColor(staticSkyTop, staticSkyHorizon, staticSkyBottom);
    }

    private void SetSkyBoxColor(Color topColor, Color horizonColor, Color bottomColor)
    {
        RenderSettings.skybox.SetColor("_Color1", topColor);
        RenderSettings.skybox.SetColor("_Color2", horizonColor);
        RenderSettings.skybox.SetColor("_Color3", bottomColor);
    }

}
