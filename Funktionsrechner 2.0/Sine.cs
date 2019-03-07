using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    class Sine : Function
    {
        //Sinuskurve: a sin(b(x-c))+d
        public double a, b, c, d;           //Parameter

        public Sine(double [] parameters)//Konstruktor
        {
            this.parameters = parameters;
            a = parameters[0];
            b = parameters[1];
            c = parameters[2];
            d = parameters[3];
        }

        /// <summary>
        /// Gibt Funktionsty zurück
        /// </summary>
        /// <returns></returns>
        public override string getFunctionType() { return "sine"; }

        /// <summary>
        /// Berechnet Y Wert
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double calculateYValue(double x)
        {
            if (checkIfBigLetter(name))
            {
                return parameters[0] * Math.Sin(parameters[1] * (x + parameters[2])) + (parameters[3] * x);
            }
            else
            {
                return parameters[0] * Math.Sin(parameters[1] * (x + parameters[2])) + parameters[3];
            }
        }

        /// <summary>
        /// Gibt Funktion als string zurück
        /// </summary>
        /// <returns></returns>
        public override string printFunction()
        {
            //Sinuskurve: a sin(b(x-c))+d
            a = Math.Round(a, 2);
            b = Math.Round(b, 2);
            c = Math.Round(c, 2);
            d = Math.Round(d, 2);
            string function = Convert.ToString(name) + "(x)= ";
            if (a != 1)
            {
                function += Convert.ToString(a) + " sin(";
            }
            else
            {
                function += "sin(";
            }
            if (c == 0)
            {
                if (b == 1)
                {
                    function += "x)";
                }
                else
                {
                    function += Convert.ToString(b) + "x)";
                }
            }
            else
            {
                function += Convert.ToString(b) + "(x";
            }
            if (c > 0)
            {
                function += "-" + Convert.ToString(c) + "))";
            }
            else if (c < 0)
            {
                double cNew = Math.Abs(c);
                function += "+" + Convert.ToString(cNew) + "))";
            }
            if (d != 0)
            {
                if (d > 0)
                {
                    function += "+" + Convert.ToString(d);
                }
                else
                {
                    function += Convert.ToString(d);
                }
                if (checkIfBigLetter(name))
                {
                    function += "x";
                }
            }
            return function;
        }

        /// <summary>
        /// Prüft ob ableiten möglich
        /// </summary>
        /// <returns></returns>
        public override bool checkDerivationPossible()
        { 
            return true;
        }

        /// <summary>
        /// Prüft ob Aufleiten möglich
        /// </summary>
        /// <returns></returns>
        public override bool checkIntegrationPossible()
        {
            if (checkIfBigLetter(name)) { return false; }
            else return true;
        }

        /// <summary>
        /// Leitet Funktion ab
        /// </summary>
        /// <returns></returns>
        public override Function createDerivative()
        {
            double[] newParameters = new double[4];
            newParameters[0] = parameters[0] * parameters[1];
            newParameters[1] = parameters[1];
            newParameters[2] = parameters[2];
            newParameters[3] = 0;
            Function derivative = new Cosine(newParameters);
            if (checkIfBigLetter(name) == false) //von F(x) zu f(x) wird kein "'" hinzugefügt
            {
                derivative.primeCount = this.primeCount + 1;
            }
            derivative.name = createDerivativeName(primeCount, name);
            return derivative;
        }

        /// <summary>
        /// Leitet Funktion auf
        /// </summary>
        /// <returns></returns>
        public override Function createIntegral()
        {
            double[] newParameters = new double[4];
            newParameters[0] = (parameters[0] / parameters[1]) * -1;
            newParameters[1] = parameters[1];
            newParameters[2] = parameters[2];
            newParameters[3] = parameters[3];
            Function integral = new Cosine(newParameters);
            if (this.primeCount > 0)
            {
                integral.primeCount = this.primeCount - 1;
            }
            integral.name = createIntegralName(integral.primeCount, primeCount, this.name);
            return integral;
        }

        /// <summary>
        /// Berechnet Nullstellen
        /// </summary>
        /// <returns></returns>
        public override double[] calculateZeros()
        {
            int limit = 200;
            double period = 2 * Math.PI / Math.Abs(b);
            double shift = period/4;
            double[] newParameters = new double[4];
            newParameters[0] = a;
            newParameters[1] = b;
            newParameters[2] = c + shift; //verschiebung um p/4 nach links
            newParameters[3] = d;
            Function shifted = new Cosine(parameters); //sinus = cosinus nur verschoben um p/4
            zeros = shifted.calculateZeros();
            for (int i = 0; i < zeros.Length; i++) //zurückverschieben um p/4 nach rechts
            {
                zeros[i] += shift;
                zeros[i] = hopping(zeros[i], period, ref limit);
                zeros[i] = Math.Round(zeros[i], roundDigits);
            }
            if (limit == 0) { zeros = new double[0]; return zeros; }
            return zeros;
        }

    }
}
