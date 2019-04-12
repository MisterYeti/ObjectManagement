using System.IO;
using UnityEngine;

public class GameDataReader
{
    private BinaryReader _reader;
    public int Version { get; }

    public GameDataReader (BinaryReader reader, int version)
    {
        this._reader = reader;
        this.Version = version;
    }

    public float ReadFloat()
    {
        return _reader.ReadSingle();
    }

    public int ReadInt()
    {
        return _reader.ReadInt32();
    }

    public Quaternion ReadQuaternion()
    {
        Quaternion value;
        value.x = _reader.ReadSingle();
        value.y = _reader.ReadSingle();
        value.z = _reader.ReadSingle();
        value.w = _reader.ReadSingle();
        return value;
    }

    public Vector3 ReaderVector3()
    {
        Vector3 value;
        value.x = _reader.ReadSingle();
        value.y = _reader.ReadSingle();
        value.z = _reader.ReadSingle();
        return value;
    }

    public Color ReaderColor()
    {
        Color value;
        value.r = _reader.ReadSingle();
        value.g = _reader.ReadSingle();
        value.b = _reader.ReadSingle();
        value.a = _reader.ReadSingle();
        return value;
    }
}
