using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    class Cosine:Function
    {
        //Kosinuskurve: a cos(b(x-c))+d
        double a, b, c, d;          //Parameter

        public Cosine(double[] parameters)//Konstruktor
        {
            this.parameters = parameters;
            a = parameters[0];
            b = parameters[1];
            c = parameters[2];
            d = parameters[3];
        }

        /// <summary>
        /// Gibt Funktionstyp zurück
        /// </summary>
        /// <returns></returns>
        public override string getFunctionType() { return "cosine"; }

        /// <summary>
        /// Berechnet Y Wert
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double calculateYValue(double x)
        {
            if (checkIfBigLetter(name))
            {
                return parameters[0] * Math.Cos(parameters[1] * (x + parameters[2])) + (parameters[3] * x);
            }
            else
            {
                return parameters[0] * Math.Cos(parameters[1] * (x + parameters[2])) + parameters[3];
            }
        }

        /// <summary>
        /// Gibt Funktion als string zurück
        /// </summary>
        /// <returns></returns>
        public override string printFunction()
        {
            //Kosinuskurve: a cos(b(x-c))+d
            double a = Math.Round(this.a, 2);
            double b = Math.Round(this.b, 2);
            double c = Math.Round(this.c, 2);
            double d = Math.Round(this.d, 2);
            string function = Convert.ToString(name) + "(x)= ";
            if (a != 1 && a != -1)
            {
                function += Convert.ToString(a) + " cos(";
            }
            else
            {
                if (a == 1) function += "cos(";
                else function += "- cos(";
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
                if (b == 1) function += "x ";
                else function += Convert.ToString(b) + "(x ";
            }
            if (c > 0)
            {
                if (b == 1) function += "- " + Convert.ToString(c) + ")";
                else function += "- " + Convert.ToString(c) + "))";
            }
            else if (c < 0)
            {
                double cNew = Math.Abs(c);
                if (b == 1) function += "+ " + Convert.ToString(cNew) + ")";
                else function += "+ " + Convert.ToString(cNew) + "))";
            }
            if (d != 0)
            {
                if (d > 0)
                {
                    function += " + " + Convert.ToString(d);
                }
                else
                {
                    double dNew = Math.Abs(d);
                    function += " - " + Convert.ToString(dNew);
                }
                if (checkIfBigLetter(name))
                {
                    function += "x";
                }
            }
            return function;
        }

        /// <summary>
        /// Prüft ob Ableiten möglich
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
            newParameters[0] = (parameters[0] * parameters[1]) * -1;
            newParameters[1] = parameters[1];
            newParameters[2] = parameters[2];
            newParameters[3] = 0;
            Function derivative = new Sine(newParameters);
            if (checkIfBigLetter(name) == false) //von F(x) zu f(x) wird kein "'" hinzugefügt
            {
                derivative.primeCount = this.primeCount + 1;
            }
            derivative.name = createDerivativeName(primeCount, name);
            return derivative;
        }

        /// <summary>
        /// Leitet funktion auf
        /// </summary>
        /// <returns></returns>
        public override Function createIntegral()
        {
            double[] newParameters = new double[4];
            newParameters[0] = parameters[0] / parameters[1];
            newParameters[1] = parameters[1];
            newParameters[2] = parameters[2];
            newParameters[3] = parameters[3];
            Function integral = new Sine(newParameters);
            if (this.primeCount > 0)
            {
                integral.primeCount = this.primeCount - 1;
            }
            integral.name = createIntegralName(integral.primeCount, primeCount, this.name);
            return integral;
        }

        //Zeros//
        /// <summary>
        /// Prüft ob Funktion Nullstellen hat
        /// </summary>
        /// <returns></returns>
        public int checkZerosThere()
        {
            if (d == 0) return 2;
            if (a > 0)
            {
                if (d > 0)
                {
                    if (d < a) return 2;
                    if (d == a) return 1;
                    else return 0;
                }
                if (d < 0)
                {
                    double dAbs = Math.Abs(d);
                    if (dAbs < a ) return 2;
                    if (dAbs == a)  return 2;
                    else return 0;
                }
                return 2; //d == 0
            }
            else  // a < 0
            {
                if (d > 0)
                {
                    double aAbs = Math.Abs(a);
                    if (d < aAbs) return 2;
                    if (d == aAbs)  return 2; 
                    else return 0;
                }
                if (d < 0)
                {
                    double dAbs = Math.Abs(d);
                    double aAbs = Math.Abs(a);
                    if (dAbs < aAbs) return 2;
                    if (dAbs == aAbs) return 1;
                    else return 0;
                }
                return 2; //d == 0
            }
        }

        public override double[] calculateZeros()
        {
            int limit = 1000;
            double zero1;   //nullstelle1
            double zero2;   //Nullstelle2
            double period = 2 * Math.PI / Math.Abs(b);
            double distance;    //Abstand der Nullstellen

            if (checkZerosThere() == 2)
            {
                zero1 = ((1 / b) * Math.Acos(-d / a));  //c=0 gesetzt
                if (zero1 < 0) zero1 += period;
                zero2 = period - zero1;         //Ausnutzen der Symmetrie der Nullstellen zur Periode
                distance = Math.Abs(zero2 - zero1);
                if (distance > period) distance -= period;
                //zero1 = ((1 / b) * Math.Acos(-d / a)) + c;  //c auf ausgangswert zurückgesetzt
                zero1 = zero1 + c;  //c auf ausgangswert zurückgesetzt
                zero1 = hopping(zero1, period, ref limit);  //Solange springen bis die Nullstelle innerhalb der ersten Periode gefunden wurde
                if (limit == 0)
                {
                    zeros = new double[0];
                    return zeros;
                }
                if (zero1 + distance > period)      //Wenn die nächste nullstelle außerhalb der ersten Periode liegt
                {
                    //die 2te nullstelle innerhalb der Periode wird berechnet
                    zero2 = zero1 + distance - period;
                    double hilf;    //Tausch da zero1 > zero2
                    hilf = zero1;
                    zero1 = zero2;
                    zero2 = hilf;
                }
                else
                {
                    zero2 = zero1 + distance;
                }
                addZero(Math.Round(zero1, roundDigits));
                addZero(Math.Round(zero2, roundDigits));
                return zeros;
            }
            else if (checkZerosThere() == 1) //Es gibt nur eine Nullstelle pro Periode
            {   //gleiches Verfahren
                zero1 = ((1 / b) * Math.Acos(-d / a)) + c;
                zero1 = hopping(zero1, period, ref limit);
                if (limit == 0)
                {
                    zeros = new double[0];
                    return zeros;
                }
                addZero(Math.Round(zero1, roundDigits));
                return zeros;
            }
            else //keine Nullstellen
            {
                zeros = new double[0];
                return zeros;
            }

        }
        //Zeros//
    }
}
