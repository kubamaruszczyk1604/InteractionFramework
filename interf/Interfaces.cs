using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractionFramework
{
    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Magnitude { get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); } }
    }

    public struct CommandData
    {
        public string Command { get; set; }
    }
    public delegate void OnCommand(CommandData command);
    public interface ICommandProvider
    {
       OnCommand onCommand { get; set; }
    }


    public delegate void OnDirectionChanged(Vector3 deltaDir);
    public interface IDirectionProvider
    {
        Vector3 GetDirection();
        OnDirectionChanged onDirectionChanged { get; set; }
    }

    public delegate void OnPositionChanged(Vector3 deltaPos, Vector3 posNow);
    public interface IPositionProvider
    {
        Vector3 GetPosition();
        OnDirectionChanged onPositionChanged { get; set; }
    }

    public interface I2DPosProvider
    {
        Vector3 GetPosition();
        OnPositionChanged onPositionChanged { get; set; }
    }

   
}
