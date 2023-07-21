using System.Text;

namespace AsyncTCPServer.Common;

public class Packet
{
    private readonly List<byte> _buffer;
    private byte[]? _readableBuffer;
    private int _readPos;
    
    public Packet()
    {
        _buffer = new List<byte>();
        _readableBuffer = null;
    }

    public Packet(int packetId)
    {
        _buffer = new List<byte>();
        _readableBuffer = null;
        
        Write(packetId);
    }
    
    public Packet(byte[] data)
    {
        _buffer = new List<byte>(data.Count());
        _readableBuffer = null;
        
        Write(data);
    }
    
    public void SetBytes(byte[]? data = null)
    {
        if (data != null) Write(data);
        _readableBuffer = _buffer.ToArray();
    }

    public void WriteLength()
    {
        _buffer.InsertRange(0, BitConverter.GetBytes(_buffer.Count));
    }

    public byte[] ToArray()
    {
        _readableBuffer = _buffer.ToArray();
        return _readableBuffer;
    }

    public int GetLength()
    {
        return _buffer.Count;
    }

    public int GetUnreadLength()
    {
        return _buffer.Count - _readPos;
    }

    public void Reset()
    {
        _buffer.Clear();
        _readPos = 0;
        _readableBuffer = null;
    }

    #region BOOL
    
    public void Write(bool value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public bool ReadBool(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = BitConverter.ToBoolean(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += 1;
        return value;
    }

    #endregion

    #region SHORT
    
    public void Write(short value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public short ReadShort(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = BitConverter.ToInt16(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += 2;
        return value;
    }
    
    #endregion
    
    #region INT
    
    public void Write(int value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public int ReadInt(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = BitConverter.ToInt32(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += 4;
        return value;
    }
    
    #endregion
    
    #region FLOAT
    
    public void Write(float value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public float ReadFloat(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = BitConverter.ToSingle(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += 4;
        return value;
    }

    #endregion

    #region LONG
    
    public void Write(long value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public long ReadLong(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = BitConverter.ToInt64(_readableBuffer, _readPos);
        if (moveReadPos) _readPos += 8;
        return value;
    }

    #endregion
    
    #region Byte

    public void Write(byte data)
    {
        _buffer.Add(data);
    }

    public byte ReadByte(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var value = _readableBuffer[_readPos];
        if (moveReadPos) _readPos += 1;
        return value;
    }
    
    #endregion

    #region BYTEARR

    public void Write(IEnumerable<byte> data)
    {
        _buffer.AddRange(data);
    }

    public byte[] ReadBytes(int lenght, bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos) throw new Exception("Cannot read data from buffer");
        var value = _buffer.GetRange(_readPos, lenght).ToArray();
        if (moveReadPos) _readPos += lenght;
        return value;
    }
    
    #endregion

    #region STRING
    
    public void Write(string value)
    {
        Write(value.Length);
        _buffer.AddRange(Encoding.ASCII.GetBytes(value));
    }
    
    public string ReadString(bool moveReadPos = true)
    {
        if (_buffer.Count <= _readPos || _readableBuffer == null)
            throw new Exception("Cannot read data from buffer");
        var length = ReadInt();
        var value = Encoding.ASCII.GetString(_readableBuffer, _readPos, length);
        if (moveReadPos && value.Length > 0) _readPos += length;
        return value;
    }

    #endregion
}