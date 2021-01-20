using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public class FPSCounter
    {
        double currentFrametimes;
        double weight;
        int numerator;
        Stopwatch T = new Stopwatch();
        public double framerate
        {
            get
            {
                return (numerator / currentFrametimes);
            }
        }

        public FPSCounter()
        {
        }

        public void Update(double timeSinceLastFrame)
        {
            currentFrametimes = currentFrametimes / weight;
            currentFrametimes += timeSinceLastFrame;
        }
    }
}
