using Unity.Properties;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class QTE_2 : VisualElement
{

    [SerializeField, DontCreateProperty]
    private string m_Combination = "NOT";

    [SerializeField, DontCreateProperty]
    private int m_NumKeys;

    [UxmlAttribute, CreateProperty]
    public string combination
    {
        get => m_Combination;
        set
        {
            m_Combination = value;
            MarkDirtyRepaint();
        }
    }
    
    
    [UxmlAttribute, CreateProperty]
    public int numKeys
    {
        get => m_NumKeys;
        set
        {
            m_NumKeys = value;
            RefreshLetters();
            MarkDirtyRepaint();
        }
    }

    public QTE_2()
    {
        
        //Registers a callback to generate the visual contents of the units
        generateVisualContent += GenerateVisualContent;
    }
    private void GenerateVisualContent(MeshGenerationContext context)
    {
        float width = layout.width;
        float height = layout.height;
        float offset;

        var painter = context.painter2D;

        //Line
        painter.BeginPath();
        painter.LineTo(new Vector2(width * 0.5f, height * 0.7f));
        painter.lineWidth = 8f;
        painter.LineTo(new Vector2(width * 0.5f, height * 0.3f));
        painter.Stroke();



        //Key Visual
        for (int i = 0; i < numKeys; i++)
        {
            offset = i * (width * 0.2f);
            Box(painter, (width * 0.5f) + offset, height * 0.4f, height * 0.2f);
        }
        
    }

    private void Box(Painter2D painter, float startX, float startY, float size)
    {
        painter.BeginPath();
        painter.lineWidth = 6f;
        painter.fillColor = Color.cornflowerBlue;
        painter.MoveTo(new Vector2(startX, startY));
        painter.LineTo(new Vector2(startX - (size / 2), startY));
        painter.LineTo(new Vector2(startX - (size / 2), startY + size));
        painter.LineTo(new Vector2(startX + (size / 2), startY + size));
        painter.LineTo(new Vector2(startX + (size / 2), startY));
        painter.ClosePath();
        painter.Fill();
        painter.Stroke();
    }

    private void RefreshLetters()
    {
        Clear(); // remove old labels

        float width = layout.width;
        float height = layout.height;
        if (float.IsNaN(width) || float.IsNaN(height) || width <= 0 || height <= 0)
            return;

        int count = Mathf.Min(combination.Length, numKeys);
        float totalWidth = numKeys * (height * 0.25f);

        for (int i = 0; i < count; i++)
        {
            float boxX = (width * 0.5f) - (totalWidth / 2) + i * (height * 0.25f);
            float boxY = height * 0.4f;
            float boxSize = height * 0.2f;

            Label label = new Label(combination[i].ToString());
            label.style.position = Position.Absolute;
            label.style.left = boxX - (boxSize / 2);
            label.style.top = boxY;
            label.style.width = boxSize;
            label.style.height = boxSize;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.fontSize = 40;
            label.style.color = Color.black;
            label.style.unityFontStyleAndWeight = FontStyle.Bold;
            Add(label);
        }
    }
}
