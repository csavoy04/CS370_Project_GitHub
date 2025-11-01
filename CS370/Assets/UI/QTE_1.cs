using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QTE_1: VisualElement
{

    [SerializeField, DontCreateProperty]
    private float m_Line;
    private float m_MaxRotation;
    private float m_Timing;

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

    [UxmlAttribute, CreateProperty]
    public float maxRotation
    {
        get => m_MaxRotation;
        set
        {
            m_MaxRotation = Mathf.Clamp(value, 0.01f, 360f);
            MarkDirtyRepaint();
        }
    }

    [UxmlAttribute, CreateProperty]
    public float timing
    {
        get => m_Timing;
        set
        {
            m_Timing = Mathf.Clamp(value, 0.01f, 360f);
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

        float amount = 360f * ((maxRotation - line) / maxRotation);
        float timeWindowEnd = 360f * ((maxRotation - timing ) / maxRotation);
        float timeWindowStart = 360f * (((maxRotation - 0.5f) - timing) / maxRotation);

        //Line
        painter.BeginPath();
        painter.lineWidth = 8f; 
        painter.LineTo(new Vector2(width * 0.5f, height));
        painter.Arc(new Vector2(width * 0.5f, height), width * 0.20f, 0f, amount);
        painter.ClosePath();
        painter.Stroke();

        //When to press the QTE
        painter.BeginPath();
        painter.strokeColor = Color.red;
        painter.lineWidth = 8f;
        painter.LineTo(new Vector2(width * 0.5f, height));
        painter.Arc(new Vector2(width * 0.5f, height), width * 0.20f, timeWindowStart, timeWindowEnd);
        painter.ClosePath();
        painter.Stroke();
    }
}
