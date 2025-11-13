using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QTE_3 : VisualElement
{

    [SerializeField, DontCreateProperty]
    private float m_FillPercentage;

    [UxmlAttribute, CreateProperty]
    public float fillPercentage
    {
        get => m_FillPercentage;
        set
        {
            m_FillPercentage = Mathf.Clamp(value, 0.01f, 1f);
            MarkDirtyRepaint();
        }
    }
    public QTE_3()
    {
        //Registers a callback to generate the visual contents of the units
        generateVisualContent += GenerateVisualContent;
    }
    private void GenerateVisualContent(MeshGenerationContext context)
    {
        float width = contentRect.width;
        float height = contentRect.height;
        float startX = width * 0.25f;
        float endX = width * 0.75f;
        float fillWidth = (endX - startX) * fillPercentage;

        var painter = context.painter2D;

        //Container
        painter.BeginPath();
        painter.lineWidth = 10f;
        painter.MoveTo(new Vector2(startX, height * 0.45f));
        painter.LineTo(new Vector2(startX, height * 0.55f));
        painter.LineTo(new Vector2(endX, height * 0.55f));
        painter.LineTo(new Vector2(endX, height * 0.45f));
        painter.ClosePath();
        painter.Stroke();



        //Fill
        painter.BeginPath();
        painter.lineWidth = 10f;
        painter.fillColor = Color.green;
        painter.MoveTo(new Vector2(startX, height * 0.45f));
        painter.LineTo(new Vector2(startX, height * 0.55f));
        painter.LineTo(new Vector2(startX + fillWidth, height * 0.55f));
        painter.LineTo(new Vector2(startX + fillWidth, height * 0.45f));
        painter.ClosePath();
        painter.Fill();
        painter.Stroke();

    }
}
