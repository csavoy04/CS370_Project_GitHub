using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;

[UxmlElement]
public partial class Current_Units : VisualElement
{
    /*
    [SerializeField, DontCreateProperty]
    private float m_Health;

    [UxmlAttribute, CreateProperty]
    public float health
    {
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
        painter.lineWidth = 8f;


        float fillAmount = width * (health / 100f);

        painter.LineTo(new Vector2(0, 0));
        painter.LineTo(new Vector2(0 + fillAmount, 0));
        painter.LineTo(new Vector2(0 + fillAmount, height));
        painter.LineTo(new Vector2(0, height));
        painter.ClosePath();
        painter.fillColor = Color.green;
        painter.Fill(FillRule.NonZero);
        painter.Stroke();
    }*/
}
