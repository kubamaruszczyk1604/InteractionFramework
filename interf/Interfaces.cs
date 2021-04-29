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

    public delegate void OnNewText(string text);
    public delegate void OnNewLine();
    public delegate void OnJumpCursor(int right, int down);
    public interface ITextProvider
    {
        OnNewText onNewText { get; set; }
        OnNewLine onNewLine { get; set; }
        OnJumpCursor onMoveCursor { get; set; }
    }

    public enum Finger { Thumb = 0, Index = 1, Middle = 2, Ring = 3, Pinky = 4 }
    public enum Hand { Unknown = 0, Left = 1, Right = 2}
    public interface IHandTracker
    {
        Vector3 GetFingertipPosition(Finger finger,Hand hand);
        Vector3 GetFingerDirection(Finger finger, Hand hand);

    }
}
