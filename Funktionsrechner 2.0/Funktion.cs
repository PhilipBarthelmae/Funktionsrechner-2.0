using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funktionsrechner_2._0
{
    abstract class Function
    {

        public int primeCount = 0;              //Anzahl der "Striche" einer Funktion (Grad der ableitung) zB .f' 
        public string name;                     //Name der Funktion
        public double[] zeros = new double[0];  //nullstellen
        public int roundDigits = 3;             //auf wie viele Nachkommastellen gerundet werden soll
        public Interval Middle = new Interval(-30, 30); //Mitte (Wird als erstes auf Nullstellen durchsucht)
        public double[] parameters;             //Parameter einer Funktion
        public int[] exponents;                 //Exponenten einer Funktion

        double[] extremeValues = new double[0];     //extremstellen
        double[] turnValues = new double[0];        //wendestellen
        public double[] maxima = new double[0];     //hochpunkte
        public double[] minima = new double[0];     //Tiefpunte
        public double[] turns = new double[0];     //wendepunkte
        public double[] saddles = new double[0];    //sattelpunkte

        //Arrays für die Namensgebung / Erstellen des Strings
        public string[] smallLetters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        public string[] bigLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        public string[] superScript = { "⁰", "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹" };
        public string[] subScript = { "₁", "₂", "₃", "₄", "₅", "₆", "₇", "₈", "₉" };

        /// <summary>
        /// Erzeugt den Namenfür die nächste Ableitung
        /// </summary>
        /// <param name="primeCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string createDerivativeName(int primeCount, string name)
        {
            char letter = name[0];
            string newName = "error";

            for (int i = 0; i < 26; i++)
            {
                if (letter.ToString() == bigLetters[i])
                {
                    newName = smallLetters[i];
                    return newName;
                }
            }

            newName = name + "'";
            return newName;
        }

        /// <summary>
        /// Erzeigt den Namen für die nächste Stammfunktion
        /// </summary>
        /// <param name="IntegralPrimeCount"></param>
        /// <param name="primeCount"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string createIntegralName(int IntegralPrimeCount, int primeCount, string name)
        {
            char letter = name[0];
            string newName = "error";

            if (IntegralPrimeCount == 0 && primeCount == 0) //z.B f => F
            {
                for (int i = 0; i < 26; i++)
                {
                    if (letter.ToString() == smallLetters[i])
                    {
                        newName = bigLetters[i];
                        return newName;
                    }
                }
            }
            if (IntegralPrimeCount == 0 && primeCount == 1) //z.B f' => f
            {
                newName = letter.ToString();
                return newName;
            }
            else
            {
                newName = letter.ToString();
                for (int i = 0; i < IntegralPrimeCount; i++)//z.B f'' => f'
                {
                    newName += "'";
                }
            }
            return newName;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Function() { }

        /// <summary>
        /// Gibt eine Zahl als superscript als string zurück
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string getSuperScript(int number)
        {
            string superscript;
            if (number < 10)
            {
                superscript = superScript[number];
            }
            else
            {
                int singleDigit = number % 10;
                int doubleDigit = (number - singleDigit) / 10;
                superscript = superScript[doubleDigit] + superScript[singleDigit];
            }
            return superscript;
        }

        /// <summary>
        /// Gibt eine Zahl als subscript als string zurück
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string getSubScript(int number)
        {
            string subscript;
            if (number < 10)
            {
                subscript = subScript[number];
            }
            else
            {
                int singleDigit = number % 10;
                int doubleDigit = (number - singleDigit) / 10;
                subscript = subScript[doubleDigit] + subScript[singleDigit];
            }
            return subscript;
        }

        /// <summary>
        /// Überprüft ob der Name in Großbuchstabe(und damit eine Stammfunktion) ist
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool checkIfBigLetter(string name)
        {
            char letter = name[0];
            for (int i = 0; i < 26; i++)
            {
                if (bigLetters[i] == letter.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt den Grad einer Polyomfunktion zurück
        /// </summary>
        /// <returns></returns>
        public virtual int getFunctionDegree()
        {
            return -1;
        }

        /// <summary>
        /// Springt mit einer Nullstelle solange in eine Richtung, bis diese sich im Intervall [0, p] befindet (p=Periode)
        /// </summary>
        /// <param name="zero"></param>
        /// <param name="period"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public double hopping(double zero, double period, ref int limit)
        {
            if (zero < 0) //links vom Interval [0;p]
            {
                while (zero < 0 && limit > 0)
                {
                    zero += period;
                    limit--;
                }
            }
            if (zero > period)  //rechts vom Intervall [0;p]
            {
                while (zero > period && limit > 0)
                {
                    zero -= period;
                    limit--;
                }
            }
            return zero;
        }

        /// <summary>
        /// Springt mit einer Nullstelle periodenweise bis ein Limit erreicht wurde und speichert dabei jede Nullstelle auf dem weg
        /// </summary>
        /// <param name="n"></param>
        /// <param name="period"></param>
        /// <param name="ub"></param>
        /// <param name="relevantPoints"></param>
        public void addHopping(double n, double period, double ub, ref double[] relevantPoints)
        {
            while (n < ub)
            {
                addArrayEntry(n, ref relevantPoints);
                n += period;
            }
        }

        /// <summary>
        /// Fügt eine noch nicht gefundene Nullstelle dem Nullstellenarray hinzu
        /// </summary>
        /// <param name="newEntry"></param>
        public void addZero(double newEntry)
        {
            bool alreadyFound = false;
            for (int i = 0; i < zeros.Length; i++)
            {
                if (newEntry == zeros[i])
                {
                    alreadyFound = true;
                }
            }
            if (alreadyFound == false)
            {
                Array.Resize(ref zeros, zeros.Length + 1);
                zeros[zeros.Length - 1] = newEntry;
            }
        }

        /// <summary>
        /// Fügt einen noch nicht enthaltenen Eintrag einem beliebigen Array hinzu
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="array"></param>
        public void addArrayEntry(double entry, ref double[] array)
        {
            bool alreadyFound = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (entry == array[i])
                {
                    alreadyFound = true;
                }
            }
            if (alreadyFound == false)
            {
                Array.Resize(ref array, array.Length + 1);
                array[array.Length - 1] = entry;
            }
        }

        /// <summary>
        /// Berechnet die Extrempunkte einer Funktion
        /// </summary>
        public void calculateMaxima()
        {
            Function derivative = createDerivative();                       //steigung
            Function secondDerivative = derivative.createDerivative();      //krümmung
            extremeValues = derivative.calculateZeros();                    //extremstellen
            for (int i = 0; i < extremeValues.Length; i++)
            {
                if (secondDerivative.calculateYValue(extremeValues[i]) < 0) //Hochpunkt
                {
                    addArrayEntry(extremeValues[i], ref maxima);
                }
                else if (secondDerivative.calculateYValue(extremeValues[i]) > 0) //Tiefpunkt
                {
                    addArrayEntry(extremeValues[i], ref minima);
                }
            }
        }

        /// <summary>
        /// Berechnet die Wendepunkte einer Funktion
        /// </summary>
        public void calculateTurns()
        {
            Function derivative = createDerivative();                       //steigung
            Function secondDerivative = derivative.createDerivative();      //krümmung
            Function thirdDerivative = secondDerivative.createDerivative(); //prüfung
            turnValues = secondDerivative.calculateZeros();                 //Wendestellen
            double[] zeroSlope = derivative.calculateZeros();               //stellen mit steigung = 0

            for (int i = 0; i < turnValues.Length; i++)
            {
                if (thirdDerivative.calculateYValue(turnValues[i]) != 0)
                {
                    bool turningZeroFound = false; //Wendepunkt mit steigung null (Sattelpunkt)
                    for (int j = 0; j < zeroSlope.Length; j++)
                    {
                        if (zeroSlope[j] == turnValues[i]) turningZeroFound = true;
                    }
                    if (turningZeroFound == true)
                    {
                        addArrayEntry(turnValues[i], ref saddles);
                    }
                    else
                    {
                        addArrayEntry(turnValues[i], ref turns);
                    }
                }
            }
        }

        /// <summary>
        /// Berechnet die Fläche unter einer Funktion zwischen zwei Grenzen
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="ub"></param>
        /// <returns></returns>
        public virtual double calculateArea(double lb, double ub)
        {
            //Da der Code für Sin/Cos der gleiche und sehr lange ist lasse ich ihn einmal in der basisklase und überschreibe ihn
            //für Polynomfunktionen

            //Erklärung zum Grobverfahren: (Ziel: Alle nullstellen zwischen den Integrationsgrenzen finden um dann Teilflächen berechnen zu können)
            //es gilt lb = n +k*p ; also wenn man die die nullstelle k mal verschiebt gelangt man zur unteren Integrationsgrenze
            //k wird dann auf die nächst kleinere Ganze Zahl gerundet um Rundungsfehler im weiteren verlauf zu vermeiden
            //Dann wird geschaut ob die nullstelle n + k*p innerhlab der Integrationgrenzen liegt
            //Wenn nicht, dann wird p draufaddiert. Wenn die nullstelle nun zwischen lb und ub liegt, wird solange p draufaddiert 
            //und sich die Nullstellen gemerkt die Innerhalb lb und ub liegen, bis irgendwann ub überschritten wurde.
            //nun hat man alle Vielfache der Nullstelle von n1 die innerhalb der Grenzen liegen. Nun gleiches verfahren mit n2 (mit Ausgangswert  n2 = n1+distance)
            Function integral = createIntegral();
            double Area = 0;
            double a = parameters[0];
            double b = parameters[1];
            double c = parameters[2];
            double d = parameters[3];
            zeros = calculateZeros();
            Array.Sort(zeros);
            double[] relevantPoints = new double[0]; //enthält alle relevanten Nullstellen sowie Integrationsgrenzen
            addArrayEntry(lb, ref relevantPoints);
            addArrayEntry(ub, ref relevantPoints);
            double period = Math.Abs(2 * Math.PI / b);

            if (zeros.Length == 2)
            {
                double distance = Math.Abs(zeros[1] - zeros[0]);
                double k; //lb = n + k * p also die nullstelle um k perioden verschoben
                double estimate1; // der errechnete wert für lb
                double estimate2;
                k = Math.Floor((lb - zeros[0]) / period);
                estimate1 = zeros[0] + k * period;
                if (estimate1 < lb) estimate1 += period;
                estimate2 = estimate1 + distance;
                addHopping(estimate1, period, ub, ref relevantPoints);
                addHopping(estimate2, period, ub, ref relevantPoints);

            }
            if (zeros.Length == 1)
            {
                double k; //lb = n + k * p also die nullstelle um k perioden verschoben
                double estimate1; // der errechnete wert für lb
                k = Math.Floor((lb - zeros[0]) / period);
                estimate1 = zeros[0] + k * period;
                if (estimate1 < lb) estimate1 += period;
                if (estimate1 < ub)
                {
                    addArrayEntry(estimate1, ref relevantPoints);
                    addHopping(estimate1, period, ub, ref relevantPoints);
                }
            }

            Array.Sort(relevantPoints);
            Array.Reverse(relevantPoints); //Von links nach rechts integrieren

            for (int i = 0; i < relevantPoints.Length - 1; i++) //Aufsummieren und Berechnen der teilintervalle
            {
                Area += Math.Abs(integral.calculateYValue(relevantPoints[i + 1]) - integral.calculateYValue(relevantPoints[i]));
            }
            Area = Math.Round(Area, roundDigits);
            return Area;

        }

        //Vorlagen für Kindklassen
        public abstract string getFunctionType();           //Gibt Funktionstyp zurück
        public abstract double calculateYValue(double x);   //Berechnet Y Wert 
        public abstract bool checkDerivationPossible();     //Prüft ob ableiten möglich
        public abstract Function createDerivative();        //Leitet die Funktion ab
        public abstract bool checkIntegrationPossible();    //Prüft ob Aufleiten möglich
        public abstract Function createIntegral();          //Leitet Funktion auf
        public abstract string printFunction();             //Gibt die Funktion als string zurück
        public abstract double[] calculateZeros();          //Berechnet Nullstellen 
    }
}
