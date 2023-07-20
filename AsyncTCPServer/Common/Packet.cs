using System.Text;

namespace NetworkingLib.Common;

public class Packet
{
    private readonly List<byte> _buffer;
    private readonly byte[] _readableBuffer;
    private readonly int _bufferSize;
    private int _readPos;
    
    public Packet(int bufferSize)
    {
        _bufferSize = bufferSize;
        _buffer = new List<byte>(bufferSize);
        _readableBuffer = new byte[bufferSize];
    }

    public void SetBytes(byte[] data)
    {
        if (data.Count() > _bufferSize)
            throw new Exception("Data size is bigger than buffer size");
            
        Write(data);
        Array.Copy(_buffer.ToArray(), _readableBuffer, _bufferSize);
    }

    public void WriteLength()
    {
        _buffer.InsertRange(0, BitConverter.GetBytes(_buffer.Count));
    }

    public byte[] ToArray()
    {
        Array.Copy(_buffer.ToArray(), _readableBuffer, _bufferSize);
        return _readableBuffer;
    }

    public int GetLength()
    {
        return _buffer.Count;
    }

    public int GetUnreadLength()
    {
        return _bufferSize - _readPos;
    }

    public void Reset()
    {
        _buffer.Clear();
        _readPos = 0;
        Array.Fill(_readableBuffer, (byte)0);
    }

    #region BOOL
    
    public void Write(bool value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public bool ReadBool(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = BitConverter.ToBoolean(_buffer.ToArray(), _readPos);
            if (moveReadPos) _readPos += 1;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }

    #endregion

    #region SHORT
    
    public void Write(short value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public short ReadShort(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = BitConverter.ToInt16(_buffer.ToArray(), _readPos);
            if (moveReadPos) _readPos += 2;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }
    
    #endregion
    
    #region INT
    
    public void Write(int value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public int ReadInt(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = BitConverter.ToInt32(_buffer.ToArray(), _readPos);
            if (moveReadPos) _readPos += 4;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }
    
    #endregion
    
    #region FLOAT
    
    public void Write(float value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public float ReadFloat(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = BitConverter.ToSingle(_buffer.ToArray(), _readPos);
            if (moveReadPos) _readPos += 4;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }

    #endregion

    #region LONG
    
    public void Write(long value)
    {
        _buffer.AddRange(BitConverter.GetBytes(value));
    }
    
    public long ReadLong(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = BitConverter.ToInt64(_buffer.ToArray(), _readPos);
            if (moveReadPos) _readPos += 8;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }

    #endregion
    
    #region Byte

    public void Write(byte data)
    {
        _buffer.Add(data);
    }

    public byte ReadByte(bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = _buffer[_readPos];
            if (moveReadPos) _readPos += 1;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }
    
    #endregion

    #region BYTEARR

    public void Write(IEnumerable<byte> data)
    {
        _buffer.AddRange(data);
    }

    public byte[] ReadBytes(int lenght, bool moveReadPos = true)
    {
        if (_buffer.Count > _readPos)
        {
            var value = _buffer.GetRange(_readPos, lenght).ToArray();
            if (moveReadPos) _readPos += lenght;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
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
        if (_buffer.Count > _readPos)
        {
            var length = ReadInt();
            var value = Encoding.ASCII.GetString(_readableBuffer, _readPos, length);
            if (moveReadPos && value.Length > 0) _readPos += length;
            return value;
        }
        throw new Exception("Cannot read data from buffer");
    }

    #endregion
}