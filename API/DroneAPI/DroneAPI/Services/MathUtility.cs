using DroneAPI.Models;
using System;

namespace DroneAPI.Services
{
    public static class MathUtility
    {
        // Calculates distance between two positions
        public static double CalculateDistance(Position a, Position b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2.0) + Math.Pow(a.Y - b.Y, 2.0));
        }

        // Calculate next direction for the drone to fly
        public static int CalculateAngle(Position origin, Position destination)
        {
            double n = 270 - (Math.Atan2(origin.Y - destination.Y, origin.X - destination.X)) * 180 / Math.PI;
            return Convert.ToInt16(n) % 360;
        }
    }
}