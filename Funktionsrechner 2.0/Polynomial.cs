using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    class Polynomial:Function
    {
        //pulled from mother
        public Interval[] intervals = new Interval[0];      //Intervalle in denen ein VZW gefunden wurde
        public bool additionalVZWfound = false;             //Weitere VZW außerhalb der Mitte[-30;30] gefunden?
        // f(x) 0,1 -0,5    -0,5    2
        public Polynomial(double[] parameters, int[] exponents) //Konstruktor
        {
            this.parameters = parameters;
            this.exponents = exponents;
        }

        /// <summary>
        /// Gibt den Funktionstyp zurück
        /// </summary>
        /// <returns></returns>
        public override string getFunctionType() { return "polynomial"; }

        /// <summary>
        /// Berechnet den Y-Wert an der Stelle x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public override double calculateYValue(double x)
        {
            double y = 0;
            for (int i = 0; i < parameters.Length; i++)
            {
                y += parameters[i] * Math.Pow(x, exponents[i]);
            }
            return y;
        }

        /// <summary>
        /// Erstellt die Ableitung der Funktion
        /// </summary>
        /// <returns></returns>
        public override Function createDerivative()
        {
            double[] newParameters;
            int[] newExponents;

            if (parameters.Length == 1) //Wenn nu eine Konstante im Funktionsterm ist (z.B. f(x)=4)
            {   //Muss extre gemacht werden weil sonst parameters.Length und es exponents.Length null wären
                newParameters = new double[1] {0.0};    //neue Parameter
                newExponents = new int[1] {0};          //neue Exponenten

                Function derivative = new Polynomial(newParameters, newExponents);  //Erstellen der neuen Funktion
                derivative.primeCount = this.primeCount + 1;                        //Hinzufügen des Strichs f => f'
                derivative.name = createDerivativeName(derivative.primeCount, name);//Erzeugung des Namens
                return derivative;
            }
            else    //for regular Polynomials
            {
                newParameters = new double[parameters.Length - 1]; // weil ein Parameter verloren geht
                newExponents = new int[exponents.Length - 1]; // weil ein Exponent verloren geht

                for (int i = 0; i < newParameters.Length; i++)  //Ableiten der Monome
                {
                    newParameters[i] = parameters[i] * exponents[i];
                    newExponents[i] = exponents[i] - 1;
                }
                Function derivative = new Polynomial(newParameters, newExponents);  //erzeugen der Funktion
                if (checkIfBigLetter(name) == false) //von F(x) zu f(x) wird kein Strich (') hinzugefügt
                {
                    derivative.primeCount = this.primeCount + 1;
                }
                derivative.name = createDerivativeName(primeCount, name);   //erzeugung des Namens
                return derivative;
            }
        }

        /// <summary>
        /// Erstellt die Stammfunktion der Funktion
        /// </summary>
        /// <returns></returns>
        public override Function createIntegral()
        {
            double[] newParameters = new double[parameters.Length + 1]; //neue Parameter
            int[] newExponents = new int[exponents.Length + 1];         //neue Exponenten
            newExponents[newExponents.Length - 1] = 0;
            newParameters[newParameters.Length - 1] = 0;
            for (int i = 0; i < parameters.Length; i++)                 //Aufleiten der Monome
            {
            newExponents[i] = exponents[i] + 1;
            newParameters[i] = parameters[i] / newExponents[i];
            }
            Function Integral = new Polynomial(newParameters, newExponents);    //Erzeugung der neuen Funktion
            if (this.primeCount > 0) 
            {                   
                Integral.primeCount = this.primeCount - 1; //Anpassen der Striche z.B f'' => f'
            }
            Integral.name = createIntegralName(Integral.primeCount, primeCount, this.name); //Erzeugen des neuen namens
            return Integral;
        }

        /// <summary>
        /// Gibt die Funktion als String zurück
        /// </summary>
        /// <returns></returns>
        public override string printFunction()
        {
            string function = "";
            function += name + "(x) = ";

            if (parameters.Length == 1 && exponents[0] == 0) // horizontale
            {
                function += Math.Round(parameters[0], 2);
                return function;
            }
            //normale Gerade
            if (parameters.Length == 2 && exponents[0] == 1 && exponents[1] == 0)
            {
                if (parameters[0] == 1)
                {
                    function += "x ";
                }
                else
                {
                    function += Math.Round(parameters[0], 2) + "x ";
                }
                if (parameters[1] > 0)
                {
                    function += "+ " + Math.Round(parameters[1], 2);
                }
                if (parameters[1] < 0)
                {
                    double number = Math.Abs(parameters[1]);
                    function += "- " + Math.Round(number, 2);
                }
                return function;
            }
            //Polynomfunktion (haben immer eine Parameter.Length > 2)
            string part1;       //das erst Monom des Strings (Dort wird bei negativem Vorzeichen kein Leerzeichen eingefügt)
            string part2 = ""; //der rest des Strings (Wird nachher in seiner Länge korrigiert)

            if (parameters[0] == 1) //eins vor dem x wird weggelassen
            {
                part1 = "x" + getSuperScript(exponents[0]) + " ";
            }
            else
            {
                part1 = Math.Round(parameters[0], 2) + "x" + getSuperScript(exponents[0]) + " ";
            }

            for (int i = 1; i < parameters.Length; i++) 
            {
                if (parameters[i] != 0) //bei Null wird Komplettes Monom weggelassen
                {
                    if (parameters[i] == 1) //Die 1 wird vor dem x meist weggelassen
                    {
                        part2 += "+ ";
                    }
                    else if (parameters[i] > 0 && parameters[i] != 1) //Normallfall => wird normal hingeschrieben
                    {
                        part2 += "+ " + Math.Round(parameters[i], 2);
                    }
                    else //Parameter ist kleiner Null
                    {
                        part2 += Math.Round(parameters[i], 2);
                    }
                    if (exponents[i] > 1) //Normaler Exponent
                    {
                        part2 += "x" + getSuperScript(exponents[i]) + " ";
                    }
                    if (exponents[i] == 1) //Exponent von 1 wird weggelassen
                    {
                        part2 += "x ";
                    }
                    // falls der letzte parameter 1 ist wird er sonst weggelassen 
                    if (i == parameters.Length - 1 && parameters[i] == 1 && exponents[i] == 0) 
                    {
                        part2 += "1";
                    }
                }
            }
            part2 = correctSpacing(part2); // Länge / Leerzeichen müssen korrigiert werden 
            function += part1 + part2;
            return function;
        }

        /// <summary>
        /// Fügt bei negativen Monomen ein Leerzeichen nach dem Minus ein damit es schöner aussieht:
        /// 5x³ -2x² -x -2 => 5x³ - 2x² - x - 2
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public string correctSpacing(string part)
        {
            string corrected ="";
            char[] chars = part.ToCharArray();
            int minusCount = 0;

            for (int i = 0; i < chars.Length; i++)  //Zählt die Minuszeichen
            {
                if (chars[i] == '-') minusCount++;
            }

            char[] LentghCorrectedChars = new char[chars.Length + minusCount]; //Array korrekter Länge
            int addedSpaces = 0;    //Beriets hinzugefügte Leerzeichen

            for (int i = 0; i < chars.Length; i++)  //fügt Leerzeichen nach Minuszeichen ein
            {
                LentghCorrectedChars[i + addedSpaces] = chars[i];
                if (chars[i] == '-')
                {
                    LentghCorrectedChars[i + 1 + addedSpaces] = ' ';
                    addedSpaces++;
                }
            }

            for (int i = 0; i < LentghCorrectedChars.Length; i++) //Setzt das Ergebnis zusammen
            {
                corrected += LentghCorrectedChars[i];
            }
            return corrected;
        }

        /// <summary>
        /// Überprüft ob das Integrieren möglich ist
        /// </summary>
        /// <returns></returns>
        public override bool checkIntegrationPossible()
        {
            if (parameters[0] == 0) //funktionsterm darf nicht 0 sein
            {
                return false;
            }
            for (int i = 0; i < bigLetters.Length; i++) //darf keine Stammfunktion sein
            {
                if (name[0].ToString() == bigLetters[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Überprüft ob das Ableiten möglich ist
        /// </summary>
        /// <returns></returns>
        public override bool checkDerivationPossible()
        {
            if (parameters[0] == 0) return false;
            else return true;
        }

        //GFS-Relevant
        #region GFS
        /// <summary>
        /// Überprüft ob der Interval schon gefunden wurde
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        /// <returns></returns>
        public bool checkIntervalAlreadyFound(double lb, double ub)
        { //ub = upperBound || lb = lowerBound
            Interval I = new Interval(lb, ub);
            for (int i = 0; i < intervals.Length; i++)
            {
                if (I.lowerBound == intervals[i].lowerBound) 
                {
                    if (I.upperBound == intervals[i].upperBound)    
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt den Grad der Polynomfunktion zurück
        /// </summary>
        /// <returns></returns>
        public override int getFunctionDegree()
        {
            int degree;
            degree = exponents.Length - 1;
            return degree;
        }

        /// <summary>
        /// Fügt den Intervall (in dem ein VZW gefunden wurde) zum Array intervals[] hinzu
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        public void addIntervallEntry(double x1, double x2)
        {
            Interval newEntry = new Interval(x1, x2);
            Array.Resize(ref intervals,intervals.Length + 1);
            intervals[intervals.Length - 1] = newEntry;
        }

        /// <summary>
        /// Überpruft ob der Intervall in der Mitte[-30,30] liegt
        /// </summary>
        /// <param name="I"></param>
        /// <returns></returns>
        public bool checkIfIntervalIsInMiddle(Interval I)
        {
            if (I.lowerBound>= Middle.lowerBound && I.lowerBound < Middle.upperBound)
            {
                if (I.upperBound > Middle.lowerBound && I.upperBound <= Middle.upperBound)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt den Interval aus dem Array intervals[] zurück, der am weitesten rechts liegt
        /// </summary>
        /// <returns></returns>
        public Interval getRightMostInterval()
        {
            Interval rightMostInterval = intervals[0];
            for (int i = 0; i < intervals.Length; i++)
            {
                if (rightMostInterval.upperBound < intervals[i].upperBound)
                {
                    rightMostInterval = intervals[i];
                }
            }
            return rightMostInterval;
        }

        /// <summary>
        /// Gibt den Interval aus dem Array intervals[] zurück, der am weitesten links liegt
        /// </summary>
        /// <returns></returns>
        public Interval getLeftMostInterval()
        {
            Interval leftMostInterval = intervals[0];
            for (int i = 0; i < intervals.Length; i++)
            {
                if (leftMostInterval.lowerBound > intervals[i].lowerBound)
                {
                    leftMostInterval = intervals[i];
                }
            }
            return leftMostInterval;
        }

        /// <summary>
        /// Gibt den Intervall zurück, in welchem "zero" liegt
        /// </summary>
        /// <param name="zero"></param>
        /// <returns></returns>
        public Interval getInterval(double zero)
        {
            Interval A;
            if (zero < 0)
            {
                double sw = -0.5;                       //Schrittweite
                double upperBound = Middle.lowerBound;  //Startintervall
                double lowerBound = upperBound + sw;    //Startintervall
                A = searchInterval(lowerBound, upperBound, sw, zero); //Suche
                return A;
            }
            else
            {
                double sw = 0.5;
                double lowerBound = Middle.upperBound;
                double upperBound = lowerBound + sw;
                A = searchInterval(lowerBound, upperBound, sw, zero);
                return A;
            }
        }

        /// <summary>
        /// Sucht den Interval in welchem "zero" liegt
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        /// <param name="sw"></param>
        /// <param name="zero"></param>
        /// <returns></returns>
        public Interval searchInterval(double lb, double ub, double sw, double zero)
        {
            Interval Error = new Interval(-1, 1);    //Falls ein Fehlerauftritt
            int counter = 10000;                    //Damit notfalls abgebrochen wird
            bool found = false;                     //Intervall gefunden?
            while (found == false && counter != 0)  //Geht nach und nach von der Mitte[-30,30] Intervalle durch 
            {                                       //Und schaut ob die "zero" drin ist
                counter--;
                if (zero >= lb && zero <= ub)
                {
                    found = true;
                    Interval I = new Interval(lb, ub);
                    return I;
                }
                lb += sw;
                ub += sw;
            }
            return Error;
        }

        /// <summary>
        /// Überprüft die rechts-liegenden Intervalle
        /// </summary>
        public void checkRightIntervals()
        {   
            additionalVZWfound = false;             //Weiterer VZW gefunden?
            Interval I = getRightMostInterval();    //Intervall ganz rechts außen

            if (checkIfIntervalIsInMiddle(I) == true)   //Intervall ist in der Mitte?
            {
                double lowerSearchBound = Middle.upperBound;    //Es wird ab der Mitte gesucht
                double upperSearchBound = lowerSearchBound + 20;
                checkIntervals(lowerSearchBound, upperSearchBound);
            }
            else
            {
                double lowerSearchBound = I.upperBound; //Es wird ab der geundenen Nullstelle gesucht
                double upperSearchBound = lowerSearchBound + 20;
                checkIntervals(lowerSearchBound, upperSearchBound);
            }
        }

        /// <summary>
        /// Überprüft die links-liegenden Intervalle
        /// </summary>
        public void checkLeftIntervals()
        {   //Analog check Left Intervals
            additionalVZWfound = false;
            Interval I = getLeftMostInterval();

            if (checkIfIntervalIsInMiddle(I) == true)
            {
                double upperSearchBound = Middle.lowerBound;
                double lowerSearchBound = upperSearchBound - 20;
                checkIntervals(lowerSearchBound, upperSearchBound);
            }
            else
            {
                double upperSearchBound = I.lowerBound;
                double lowerSearchBound = upperSearchBound - 20;
                checkIntervals(lowerSearchBound, upperSearchBound);
            }
        }

        /// <summary>
        /// Überprüft alle Intervalle zwischen lb und ub auf VZW
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        public void checkIntervals(double lb, double ub)
        {
            for (double i = lb; i < ub; i += 0.5)
            {
                if (checkVZW(i, i + 0.5) == true && checkIntervalAlreadyFound(i, i + 0.5) == false)
                {
                    addIntervallEntry(i, i + 0.5);
                    additionalVZWfound = true;
                }
            }
        }

        /// <summary>
        /// Überprüft einen Intervall auf VZW
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public bool checkVZW(double x1, double x2)
        {
            double y1 = calculateYValue(x1);
            double y2 = calculateYValue(x2);
            if (y1 <= 0 && y2 >= 0) { return true; }
            if (y1 >= 0 && y2 <= 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Vergleicht beide Schätzungen für die Nullstelle
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public bool compareEstimates(double e1, double e2)
        {
            e1 = Math.Truncate(e1 * 10000) / 10000;
            e2 = Math.Truncate(e2 * 10000) / 10000;
            if (e1 == e2) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Annäherungsverfahren zur Nullstellenberechnung
        /// </summary>
        /// <param name="I"></param>
        /// <param name="iterations"></param>
        /// <returns></returns>
        public double NewtonAlgorithm(Interval I, int iterations)
        {
            Function derivative = this.createDerivative();
            double estimate1; // Eine schätzung der Nullstelle
            double estimate2; // Eine bessere Schätzung der nullstelle, die aus der ersten hervorgeht
            double middle = I.getIntervallAverage();

            if (calculateYValue(middle) == 0) { addZero(middle); } //nullstelle schon gefunden
            while (derivative.calculateYValue(middle) == 0) //Das Newton verfahren geht nicht bei Steigung = 0
            {
                middle += 0.00001; //Verschiebeung des Startwerts, sodass Verfahren funktioniert
            }

            estimate1 = middle;
            int counter = 0;
            for (int i = 0; i < iterations; i++) 
            {
                //berechnung nes nächsten Schätzwertes
                estimate2 = estimate1 - (this.calculateYValue(estimate1) / derivative.calculateYValue(estimate1));
                if (compareEstimates(estimate1, estimate2)) //Wenn beide die gleichen ersten vier nachkommaziffern haben
                {
                    counter++;
                    if (counter == 10) { return estimate1; }//Beide hatten 10 mal die gleichen ersten vier nachkommaziffern -> genau genug
                }
                else counter = 0;
                estimate1 = estimate2;
            }
            return estimate1;
        }

        /// <summary>
        /// Berechnet die Nullstellen der Funktion 
        /// </summary>
        /// <returns></returns>
        public override double[] calculateZeros()
        {
            int degree = getFunctionDegree();

            // konstanter term wie f(x) = 5
            if (degree == 0)
            {
                //hier
                return zeros;
            }

            //  linie wie f(x) = 3x + 5
            else if (degree == 1)
            {
                double a = parameters[0];
                double b = parameters[1];
                double zero = (-b) / a;
                addZero(Math.Round(zero, roundDigits));
                return zeros;
            }

            // quadratische Funktionen wie f(x) = 2x² + 5x - 3
            else if (degree == 2)
            {
                double zero1;
                double zero2;
                double a = parameters[0];
                double b = parameters[1];
                double c = parameters[2];

                //abc-Formel
                zero1 = (-b + Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
                if (zero1 <= 0 || zero1 > 0) addZero(Math.Round(zero1, roundDigits));
                zero2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
                if (zero2 != zero1)
                {
                    if (zero2 <= 0 || zero2 > 0) addZero(Math.Round(zero2, roundDigits));
                }
                return zeros;
            }

            // alle funktionen mit grad > 2
            else 
            {
                // Überprüfung der Mitte [-30,30]
                bool VZWfound = false;
                for (double i = Middle.lowerBound ; i < Middle.upperBound; i += 0.25)
                {
                    if (checkVZW(i, i + 0.25) == true)
                    {
                        addIntervallEntry(i, i + 0.25);
                        VZWfound = true;
                    }
                }

                //Wenn im Intervall [-30,30] Nullstellen gefuden wurden, wird dort weitergesucht
                if (VZWfound == true)
                {
                    do  //Durchsuchen der nächsten 40 Teilintervalle (positiv) mit delta x = 0,5
                    {   //wenn ein eine Nullstelle gefunden wurde 
                        checkRightIntervals();
                    } while (additionalVZWfound);

                    do  //Durchsuchen der nächsten 40 Teilintervalle (negativ) mit delta x = 0,5
                    {   //wenn ein eine Nullstelle gefunden wurde 
                        checkLeftIntervals();
                    } while (additionalVZWfound);
                }

                //Es wurden keine Nullstellen im Intervall [-30,30] gefunden, nun wird mit
                //der Newton Methode nach einer gesucht
                else 
                {
                    Interval Start = new Interval(-0.5 ,0.5);
                    double zero;
                    zero = NewtonAlgorithm(Start, 200);
                    double YAtZero; //der Funktionswert an der stelle "zero"
                    YAtZero = calculateYValue(zero);
                    YAtZero = Math.Abs(YAtZero);
                    if(YAtZero < 0.1)
                    {
                        VZWfound = true;
                        Interval I = getInterval(zero);
                        //wenn ein fehler aufgetreten ist
                        if (I.lowerBound == -1 && I.upperBound == 1) return zeros; 
                       
                        addIntervallEntry(I.lowerBound, I.upperBound);
                        do
                        {
                            checkRightIntervals();
                        } while (additionalVZWfound);
                        do
                        {
                            checkLeftIntervals();
                        } while (additionalVZWfound);
                    }
                }
                //evaluierung der gesmmelten Intervalle
                if (intervals.Length > 0)
                {
                    for (int i = 0; i < intervals.Length; i++)
                    {
                        double x = NewtonAlgorithm(intervals[i], 35);
                        addZero(Math.Round(x,roundDigits));
                    }
                }
                return zeros;
            }
        }
        #endregion
    }
}
