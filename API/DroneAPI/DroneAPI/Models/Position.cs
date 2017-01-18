using System;
using System.Collections.Generic;
using System.Linq;

/**
 * @author: Harmen Hilvers
 * Model containing position information
 * */

namespace DroneAPI.Models
{
    public struct Position : IEquatable<Position>
    {
        public double X;
        public double Y;

        public Position(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Position other)
        {
            return (X.Equals(other.X) && Y.Equals(other.Y));
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }
    }
}