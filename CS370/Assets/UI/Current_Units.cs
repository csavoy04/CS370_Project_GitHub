using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Current_Units : VisualElement
{
    //This is a number that is a percentage of the health
    [SerializeField, DontCreateProperty]
    float m_Health;

    //Value between 0 and 100
    [UxmlAttribute, CreateProperty]
    public float health
    {
        //The progress property is exposed in C#
        get => m_Health;
        set
        {
            m_Health = Mathf.Clamp(value, 0.01f, 100f);
            MarkDirtyRepaint();
        }
    }

    public Current_Units()
    {
        //Registers a callback to generate the visual contents of the units
        generateVisualContent += GenerateVisualContent;
    }

    private void GenerateVisualContent(MeshGenerationContext context)
    {
        float width = contentRect.width;
        float height = contentRect.height;
        
        var painter = context.painter2D;

        painter.BeginPath();
        painter.lineWidth = 8f;
        painter.LineTo(new Vector2(0, 0));
        painter.LineTo(new Vector2(width, 0));
        painter.LineTo(new Vector2(width, height));
        painter.LineTo(new Vector2(0, height));
        painter.ClosePath();
        painter.fillColor = Color.white;
        painter.Fill(FillRule.NonZero);
        painter.Stroke();

        //Bar Fill
        painter.BeginPath();
        painter.lineWidth = 10f;

        float fillAmount = ((100f - health) / 100f);

        painter.LineTo(new Vector2(0, 0));
        painter.LineTo(new Vector2(width - fillAmount, 0));
        painter.LineTo(new Vector2(width - fillAmount, height));
        painter.LineTo(new Vector2(0, height));
        painter.ClosePath();
        painter.fillColor = Color.green;
        painter.Fill(FillRule.NonZero);
        painter.Stroke();
    }
}
