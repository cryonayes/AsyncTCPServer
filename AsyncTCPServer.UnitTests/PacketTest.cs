using System.Text;
using AsyncTCPServer.Common;
using Xunit;

namespace AsyncTCPServer.UnitTests;

public class PacketTest
{
    
    [Fact]
    public void TestPacketString()
    {
        var packet = new Packet();
        packet.Write("TEST");
        packet.SetBytes();

        Assert.Equal("TEST",packet.ReadString());
    }
    
    [Fact]
    public void TestPacketBool()
    {
        var packet = new Packet();
        packet.Write(true);
        packet.SetBytes();

        Assert.True(packet.ReadBool());
    }
    
    [Fact]
    public void TestPacketShort()
    {
        var packet = new Packet();
        packet.Write(short.MaxValue);
        packet.SetBytes();

        Assert.Equal(short.MaxValue, packet.ReadShort());
    }
    
    [Fact]
    public void TestPacketInt()
    {
        var packet = new Packet();
        packet.Write(int.MaxValue);
        packet.SetBytes();

        Assert.Equal(int.MaxValue, packet.ReadInt());
    }
    
    [Fact]
    public void TestPacketFloat()
    {
        var packet = new Packet();
        packet.Write(float.MaxValue);
        packet.SetBytes();

        Assert.Equal(float.MaxValue, packet.ReadFloat());
    }
    
    [Fact]
    public void TestPacketLong()
    {
        var packet = new Packet();
        packet.Write(long.MaxValue);
        packet.SetBytes();

        Assert.Equal(long.MaxValue, packet.ReadLong());
    }
    
    [Fact]
    public void TestPacketByte()
    {
        var packet = new Packet();
        packet.Write(new byte[]{0xff});
        packet.SetBytes();

        Assert.Equal(0xff, packet.ReadByte());
    }
    
    [Fact]
    public void TestPacketBytes()
    {
        var data = Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet");
        var packet = new Packet();
        packet.Write(data);
        packet.SetBytes();

        Assert.Equal(data, packet.ReadBytes(data.Length));
    }

    [Fact]
    public void TestPacketConstructorInt()
    {
        var packet = new Packet(int.MaxValue);
        packet.SetBytes();
        
        Assert.Equal(int.MaxValue, packet.ReadInt());
    }
    
    [Fact]
    public void TestPacketConstructorBytes()
    {
        var data = Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet");
        var packet = new Packet(data);
        packet.SetBytes();
        
        Assert.Equal(data, packet.ReadBytes(data.Length));
    }

    [Fact]
    public void TestPacketReset()
    {
        var data = Encoding.ASCII.GetBytes("Lorem ipsum dolor sit amet");
        var packet = new Packet(data);
        packet.SetBytes();
        Assert.Equal(data, packet.ReadBytes(data.Length));
        
        packet.Reset();
        Assert.Throws<Exception>(() => { packet.ReadInt(); });
        
        packet.Write(int.MaxValue);
        packet.SetBytes();
        Assert.Equal(int.MaxValue, packet.ReadInt());
    }
}