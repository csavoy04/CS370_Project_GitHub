using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QTE_1: VisualElement
{
    public new class UxmlFactory : UxmlFactory<Current_Units, UxmlTraits> { }

    [SerializeField, DontCreateProperty]
    private float m_Line;

    [UxmlAttribute, CreateProperty]
    public float line
    {
        get => m_Line;
        set
        {
            m_Line = Mathf.Clamp(value, 0.01f, 360f);
            MarkDirtyRepaint();
        }
    }

    public QTE_1()
    {
        //Registers a callback to generate the visual contents of the units
        generateVisualContent += GenerateVisualContent;
    }
    private void GenerateVisualContent(MeshGenerationContext context)
    {
        float width = contentRect.width;
        float height = contentRect.height;

        var painter = context.painter2D;

        //Draw Circle
        painter.BeginPath();
        painter.lineWidth = 10f;
        painter.Arc(new Vector2(width * 0.5f, height), width * 0.20f, 0f, 360f);
        painter.ClosePath();
        painter.Stroke();



        //Line
        painter.BeginPath();
        painter.lineWidth = 8f;
        painter.LineTo(new Vector2(width * 0.5f, height));
        painter.Arc(new Vector2(width * 0.5f, height), width * 0.20f, 0f, 0f - line);
        painter.ClosePath();
        painter.Stroke();
    }
}
