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
        double rightMostChecked;    //der letze Überprüfte Intervall rechts
        double leftMostChecked;     //der letzte Überprüfte Intervall links
        public Interval[] intervals = new Interval[0];      //Intervalle in denen ein VZW gefunden wurde
        public bool additionalVZWfound = false;             //Weitere VZW außerhalb der Mitte[-30;30] gefunden?

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
                    if (parameters[i] == -1)//Die 1 wird vor dem x weggelassen
                    {
                        part2 += "-"; 
                    }
                    if (parameters[i] == 1) //Die 1 wird vor dem x weggelassen
                    {
                        part2 += "+ ";
                    }
                    else if (parameters[i] > 0 && parameters[i] != 1) //Normallfall => wird normal hingeschrieben
                    {
                        part2 += "+ " + Math.Round(parameters[i], 2);
                    }
                    else //Parameter ist kleiner Null
                    {
                        if (parameters[i] != -1) part2 += Math.Round(parameters[i], 2);
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

        /// <summary>
        /// Berechnet die Fläche zwischen Graph und x-Achse zwischen 2 Grenzen
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        /// <returns></returns>
        public override double calculateArea(double lb, double ub)
        {
            //lb = lower bound | ub = upper bound
            double Area = 0;
            Interval bounds = new Interval(lb, ub);
            Function integral = createIntegral();
            zeros = calculateZeros();
            double[] relevantPoints = new double[0]; //enthält alle relevanten Nullstellen sowie Integrationsgrenzen
            addArrayEntry(lb, ref relevantPoints);
            addArrayEntry(ub, ref relevantPoints);

            for (int i = 0; i < zeros.Length; i++) //Alle Nullstellen zwischen Integrationsgrenzen sind relevant
            {
                if (zeros[i] > lb && zeros[i] < ub)
                {
                    addArrayEntry(zeros[i], ref relevantPoints);
                }
            }
            Array.Sort(relevantPoints);
            Array.Reverse(relevantPoints); //Von links nach rechts integrieren

            for (int i = 0; i < relevantPoints.Length - 1; i++) //Berechnen und Aufsummieren der Teilflächen
            {
                Area += Math.Abs(integral.calculateYValue(relevantPoints[i + 1]) - integral.calculateYValue(relevantPoints[i]));
            }
            Area = Math.Round(Area, roundDigits);
            return Area;
        }


        //GFS-Relevant
        #region GFS

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
        /// Fügt das Intervall (in dem ein VZW gefunden wurde) zum Array intervals[] hinzu
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
        /// Gibt den Intervall zurück, in welchem "zero" liegt
        /// </summary>
        /// <param name="zero"></param>
        /// <returns></returns>
        public Interval getInterval(double zero)
        {
            Interval I;
            double newZero = Math.Floor(zero);
            for (double i = newZero; i < newZero + 1; i += 0.5)
            {
                if (i <= zero && zero < i + 0.5)
                {
                    I = new Interval(i, i + 0.5); return I;
                }
                else if (i < zero && zero <= i + 0.5)
                {
                    I = new Interval(i, i + 0.5); return I;
                }
            }
            I = new Interval(-1, 1); return I; //Falls ein Fehler auftritt
        }

        /// <summary>
        /// Überprüft die Intervalle, die rechts vom Intervall liegen, der als letztes auf der rechten Seite überprüft wurde
        /// </summary>
        public void checkRightIntervals()
        {   
            additionalVZWfound = false;             //Weiterer VZW gefunden?
            double lowerSearchBound = rightMostChecked;
            double upperSearchBound = rightMostChecked + 20;
            checkIntervals(lowerSearchBound, upperSearchBound);
            rightMostChecked = upperSearchBound;
        }

        /// <summary>
        /// Überprüft die Intervalle, die links vom Intervall liegen, der als letztes auf der linken Seite überprüft wurde
        /// </summary>
        public void checkLeftIntervals()
        {   //Analog check Right Intervals
            additionalVZWfound = false;
            double upperSearchBound = leftMostChecked;
            double lowerSearchBound = leftMostChecked - 20;
            checkIntervals(lowerSearchBound, upperSearchBound);
            leftMostChecked = lowerSearchBound;
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
                if (checkVZW(i, i + 0.5) == true) 
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
            //Überprüft ein Intervall auf VZW
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
            //Kürzng des Wertes auf 4 nachkommastellen
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
            Function derivative = this.createDerivative(); //erste Ableitung der Funktion
            double estimate1; // Eine Schätzung der Nullstelle
            double estimate2; // Eine bessere Schätzung der nullstelle, die aus der ersten hervorgeht
            double middle = I.getIntervallAverage(); //Findet Intervallmitte

            if (calculateYValue(middle) == 0) { addZero(Math.Round(middle,roundDigits)); } //nullstelle schon gefunden
            while (derivative.calculateYValue(middle) == 0) //Das Newton verfahren funktioniert nicht bei Steigung = 0
            {
                middle += 0.00001; //Verschiebeung des Startwerts, sodass Verfahren funktioniert
            }

            estimate1 = middle; //Erster Schätzwert der Nullstelle ist die Intervallmitte
            int counter = 0;    //zählt wie oft beide Schätzwerte gleich waren
            for (int i = 0; i < iterations; i++) 
            {
                //Berechnung nes nächsten Schätzwertes
                if (derivative.calculateYValue(estimate1) != 0) //Steigung des Graphen darf nicht 0 sein
                {
                    //Berechnung der nächsten Schätzung
                    estimate2 = estimate1 - (this.calculateYValue(estimate1) / derivative.calculateYValue(estimate1));
                    if (compareEstimates(estimate1, estimate2)) //Wenn beide die gleichen ersten vier nachkommaziffern haben
                    {
                        counter++;
                        if (counter == 10) { return estimate1; }//Beide hatten 10 mal die gleichen ersten vier nachkommaziffern -> genau genug
                    }
                    else counter = 0;
                    estimate1 = estimate2;
                }
                else estimate1 += 0.1; //Verschiebung, sodass das Verfahren weitermacht wenn es auf Steigung 0 trifft
            }
            return estimate1;
        }

        /// <summary>
        /// Berechnet die Nullstellen der Funktion und gibt sie als Array zurück
        /// </summary>
        /// <returns></returns>
        public override double[] calculateZeros()
        {
            int degree = getFunctionDegree();       //Grad der Funktion
            rightMostChecked = Middle.upperBound;   //Letzter überprüfter Intervall rechts
            leftMostChecked = Middle.lowerBound;    //Letzter überprüfter Intervall links

            // konstanter Term wie f(x) = 5
            if (degree == 0)
            {
                return zeros;   //Hat keine Nullstellen
            }

            //  linie wie f(x) = 3x + 5
            else if (degree == 1)
            {
                double a = parameters[0];
                double b = parameters[1];
                double zero = (-b) / a; //Berechnung Nullstelle
                addZero(Math.Round(zero, roundDigits)); //Speichern der Nullstelle im Array
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
                //Nullstelle muss eine Zahl sein (Bei negativer Diskriminante nicht der Fall)
                if (zero1 <= 0 || zero1 > 0) addZero(Math.Round(zero1, roundDigits));
                zero2 = (-b - Math.Sqrt(Math.Pow(b, 2) - (4 * a * c))) / (2 * a);
                if (zero2 != zero1) 
                {   //Nullstelle muss eine Zahl sein 
                    if (zero2 <= 0 || zero2 > 0) addZero(Math.Round(zero2, roundDigits));
                }   
                return zeros;
            }

            // alle funktionen mit grad > 2
            else 
            {
                // Überprüfung der Mitte [-30,30]
                bool VZWfound = false;  //Vorzeichenwechsel gefunden
                for (double i = Middle.lowerBound ; i < Middle.upperBound; i += 0.25)
                {   //Vorzeichenwechsel im Intervall gefunden?
                    if (checkVZW(i, i + 0.25) == true)
                    {
                        addIntervallEntry(i, i + 0.25); //Intervall merken
                        VZWfound = true;
                    }
                }

                //Wenn im Intervall [-30,30] Nullstellen gefuden wurden, wird dort weitergesucht
                if (VZWfound == true)
                {
                    do  //Durchsuchen der nächsten 40 Teilintervalle (positiv) mit länge = 0,5
                    {   //wenn ein eine Nullstelle gefunden wurde 
                        checkRightIntervals();
                    } while (additionalVZWfound);

                    do  //Durchsuchen der nächsten 40 Teilintervalle (negativ) mit länge = 0,5
                    {   //wenn ein eine Nullstelle gefunden wurde 
                        checkLeftIntervals();
                    } while (additionalVZWfound);
                }

                //Es wurden keine Nullstellen im Intervall [-30,30] gefunden, nun wird mit
                //der Newton Methode nach einer gesucht
                else 
                {
                    Interval Start = new Interval(-0.5 ,0.5); //Startpunkt der Suche
                    double zero;
                    zero = NewtonAlgorithm(Start, 200); //Suche starten
                    double YAtZero; //der Funktionswert an der stelle "zero"
                    YAtZero = calculateYValue(zero); //Y-Wert des Ergebnisses der Suche berechnen
                    YAtZero = Math.Abs(YAtZero);
                    if(YAtZero < 0.1) //Ein Y-Wert < 0,1 geht als gefundene Nulstelle durch
                    {
                        VZWfound = true;
                        Interval I = getInterval(zero); //Finden des Intervalls der Nullstelle
                        //wenn ein Fehler aufgetreten ist
                        if (I.lowerBound == -1 && I.upperBound == 1) return zeros; 
                        addIntervallEntry(I.lowerBound, I.upperBound); //Intervall merken
                        //Setzen der Anfangswerte für die Suche links und rechts der Nullstelle
                        rightMostChecked = I.upperBound;    
                        leftMostChecked = I.lowerBound;     
                        do
                        {   //gleiches Verfahren wie vorher
                            checkRightIntervals();
                        } while (additionalVZWfound);
                        do
                        {
                            checkLeftIntervals();
                        } while (additionalVZWfound);
                    }
                }
                //Evaluierung der gesmmelten Intervalle
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
