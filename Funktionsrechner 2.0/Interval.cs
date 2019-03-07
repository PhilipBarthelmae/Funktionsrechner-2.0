using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    class Interval
    {
        public double lowerBound;
        public double upperBound;

        public Interval(double x1, double x2) //Konstruktor
        {
            lowerBound = x1;
            upperBound = x2;
        }

        /// <summary>
        /// Gibt Mitte des Intervalls zurück
        /// </summary>
        /// <returns></returns>
        public double getIntervallAverage()
        {
            return (lowerBound + upperBound) / 2;
        }

        /// <summary>
        /// Tauscht Grenzen
        /// </summary>
        public void swapBounds()
        {
            double hilf;
            hilf = lowerBound;
            lowerBound = upperBound;
            upperBound = hilf;
        }
    }
}
