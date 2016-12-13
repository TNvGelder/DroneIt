using System;
using System.Collections.Generic;
using System.Linq;

namespace DroneAPI.Models
{
    public struct Position : IEquatable<Position>
    {
        public double X;
        public double Y;


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