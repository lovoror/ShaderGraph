using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScratchUI : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{

    public RenderTexture renderTexture;

    public Texture brushTexture;

    public Texture blankTexture;

    public RectTransform rectTransform;

    public Canvas canvas;

    private bool m_isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        DrawBlank();
    }

    private void DrawBlank()
    {
        // 激活rt
        RenderTexture.active = renderTexture;
        
        // 保存当前状态
        GL.PushMatrix();
        // 设置矩阵
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);
        
        // 绘制贴图
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        Graphics.DrawTexture(rect, blankTexture);
        
        GL.PopMatrix();
        RenderTexture.active = null;
        
    }
    
    private void Draw(int x, int y)
    {
        // 激活rt
        RenderTexture.active = renderTexture;
        // 保存当前状态
        GL.PushMatrix();
        // 设置矩阵
        GL.LoadPixelMatrix(0, renderTexture.width, renderTexture.height, 0);


        // 绘制笔刷图案
        x -= (int)(brushTexture.width * 0.5f);
        y -= (int)(brushTexture.height * 0.5f);
        Rect rect = new Rect(x, y, brushTexture.width, brushTexture.height);
        Graphics.DrawTexture(rect, brushTexture);

        // 弹出改变
        GL.PopMatrix();

        RenderTexture.active = null;
    }
    /// <summary>
    /// 按下
    /// </summary>
    public void OnPointerDown(PointerEventData data)
    {
        m_isMove = true;
    }

    /// <summary>
    /// 抬起
    /// </summary>
    public void OnPointerUp(PointerEventData data)
    {
        m_isMove = false;
    }

    private void Update()
    {
        if (m_isMove)
        {
            OnMouseMove(Input.mousePosition);
        }
    }

    /// <summary>
    /// 刮卡
    /// </summary>
    /// <param name="position">刮卡的屏幕坐标</param>
    private void OnMouseMove(Vector2 position)
    {
        // 获取刮的位置的ui局部坐标
        var uiLocalPos = ScreenPosToUiLocalPos(position, rectTransform, canvas.worldCamera);
        // 将局部坐标转化为uv坐标
        var sizeDelta = rectTransform.sizeDelta;
        var uvX = (sizeDelta.x / 2f + uiLocalPos.x) / sizeDelta.x;
        var uvY = (sizeDelta.y / 2f + uiLocalPos.y) / sizeDelta.y;
        // 将uv坐标转化为Graphics坐标
        var x = (int)(uvX * renderTexture.width);
        // 注意，uv坐标系和Graphics坐标系的y轴方向相反
        var y = (int)(renderTexture.height - uvY * renderTexture.height);

        Draw(x, y);
    }

    /// <summary>
    /// 将屏幕坐标抓话为目标RectTransform的局部坐标
    /// </summary>
    /// <param name="screenPos">屏幕坐标</param>
    /// <param name="transform">目标RectTransform</param>
    /// <param name="cam">摄像机</param>
    /// <returns>ui局部坐标</returns>
    private Vector2 ScreenPosToUiLocalPos(Vector3 screenPos, RectTransform transform, Camera cam)
    {
        Vector2 uiLocalPos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform, screenPos, cam, out uiLocalPos))
        {
            return uiLocalPos;
        }
        return Vector2.zero;
    }
}
