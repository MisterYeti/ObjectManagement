using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    private Color _color;
    private int _shapeId = int.MinValue;
    private MeshRenderer _meshRenderer;
    static int colorPropertyId = Shader.PropertyToID("_Color");
    static MaterialPropertyBlock sharedPropertyBlock;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public int ShapeId
    {
        get { return _shapeId; }
        set
        {
            if (_shapeId == int.MinValue && value != int.MinValue)
                _shapeId = value;
            else
                Debug.LogError("Not allowed to changed shapeId");
        }
    }

    public int MaterialId { get; private set; }

    public void SetMaterial (Material material, int materialId)
    {
        _meshRenderer.material = material;
        MaterialId = materialId;
    }

    public void SetColor (Color color)
    {
        this._color = color;
        if (sharedPropertyBlock == null)
        {
            sharedPropertyBlock = new MaterialPropertyBlock();
        }
        sharedPropertyBlock.SetColor(colorPropertyId, color);
        _meshRenderer.SetPropertyBlock(sharedPropertyBlock);
    }


    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.Write(_color);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        SetColor(reader.Version > 0 ? reader.ReaderColor() : Color.white);
    }

}
