using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineTrackingTest.Models
{
    /// <summary>
    /// @author: Twan van Gelder
    /// Enum for showing the positioning relative to a line.
    /// </summary>
    public enum PositioningState
    {
        Correct,
        Left,
        Right,
        Lost,
        Init
    }
}
