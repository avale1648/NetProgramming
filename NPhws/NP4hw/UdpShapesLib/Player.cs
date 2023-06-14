using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpShapesLib
{
    public enum Shape { Circle, Square, Triangle, Diamond }
    public enum FigureSize { Small, Medium, Large }

    public class Player
    {
        public string Name { get; }
        public Shape Shape { get; }
        public FigureSize Size { get; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Player(string name, Shape shape, byte red, byte green, byte blue, FigureSize size)
        {
            Name = name;
            Shape = shape;
            Red = red;
            Green = green;
            Blue = blue;
            Size = size;
        }
        public Player(BinaryReader reader)
        {
            Name = reader.ReadString();
            Shape = (Shape)reader.ReadByte();
            Red = reader.ReadByte();
            Green = reader.ReadByte();
            Blue = reader.ReadByte();
            Size = (FigureSize)reader.ReadByte();
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
        }
        public void Move(BinaryReader reader)
        {
            X = reader.ReadInt32();
            Y = reader.ReadInt32();
        }
        public void ChangeColour(BinaryReader reader)
        {
            Red = reader.ReadByte();
            Green = reader.ReadByte();
            Blue = reader.ReadByte();
        }
        public void Draw(Graphics g)
        {
            Brush brush = new SolidBrush(Color.FromArgb(Red, Green, Blue));
            g.ResetTransform();
            g.TranslateTransform(X, Y);
            int size = 0;
            if (Size == FigureSize.Small)
                size = 25;
            else if (Size == FigureSize.Medium)
                size = 50;
            else if (Size == FigureSize.Large)
                size = 100;
            if (Shape == Shape.Circle)
                g.FillEllipse(brush, 0, 0, size, size);
            else if (Shape == Shape.Square)
                g.FillRectangle(brush, 0, 0, size, size);
            else if (Shape == Shape.Triangle)
                g.FillPolygon(brush, new[] { new Point(size / 2, 0), new Point(size, size), new Point(0, size) });
            else if (Shape == Shape.Diamond)
                g.FillPolygon(brush, new[] { new Point(size / 2, 0), new Point(size, size / 2), new Point(size / 2, size), new Point(0, size / 2) });
        }
        public byte[] EnterMessage()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Message.Enter);
            writer.Write(Name);
            writer.Write((byte)Shape);
            writer.Write(Red);
            writer.Write(Green);
            writer.Write(Blue);
            writer.Write((byte)Size);
            writer.Write(X);
            writer.Write(Y);
            return stream.ToArray();
        }
        public byte[] ExistMessage()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Message.Exist);
            writer.Write(Name);
            writer.Write((byte)Shape);
            writer.Write(Red);
            writer.Write(Green);
            writer.Write(Blue);
            writer.Write((byte)Size);
            writer.Write(X);
            writer.Write(Y);
            return stream.ToArray();
        }
        public byte[] MoveMessage()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Message.Move);
            writer.Write(Name);
            writer.Write(X);
            writer.Write(Y);
            return stream.ToArray();
        }
        public byte[] LeaveMessage()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Message.Leave);
            writer.Write(Name);
            return stream.ToArray();
        }
        public byte[] ChangeColourMessage()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(Message.ChangeColour);
            writer.Write(Name);
            writer.Write(Red);
            writer.Write(Green);
            writer.Write(Blue);
            return stream.ToArray();
        }
        public bool Contains(Point point) =>
            point.X >= X && point.X < X + 50 &&
            point.Y >= Y && point.Y < Y + 50;
    }
}
