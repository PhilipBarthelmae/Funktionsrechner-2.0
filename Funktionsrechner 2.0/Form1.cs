using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Funktionsrechner_2._0
{
    public partial class Form1 : Form
    {
        public Form1() //Initialisiert Text-Boxen und Radio-Buttons
        {
            InitializeComponent();
            setUpRadioButtonsArray();
            textBox5.ReadOnly = true;
            textBox7.ReadOnly = true;
            textBox9.ReadOnly = true;
            textBox11.ReadOnly = true;
            textBox13.ReadOnly = true;
            textBox14.ReadOnly = true;
            textBox15.ReadOnly = true;
            textBox16.ReadOnly = true;
            textBox11.Font = new Font(textBox1.Font, FontStyle.Bold);
        }

        public string[] smallLetters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        public string[] bigLetters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        public RadioButton[] radioButtons = new RadioButton[15];

        Function[] function = new Function[15]; //Erstellte Funktionen
        int degree;                             //Grad der Funktion (bei polynomen)
        int functionIndex = 0;                  //Zeigt auf den nächsten freien Platz im function array
        Creator creator;                        //Interface zur Eingabe der Variablen


        /// <summary>
        /// Passt die Oberfläche der gewählten Funktion an
        /// </summary>
        /// <param name="x"></param>
        private void setInputMode(int x)
        {
            if (x == 1)
            {
                label2.Visible = true;
                textBox2.Visible = true;
            }
            else
            {
                label2.Visible = false;
                textBox2.Visible = false;
            }
        }

        /// <summary>
        /// Gibt eine Fehlermeldung bei falscher Eingabe aus
        /// </summary>
        /// <param name="x"></param>
        private void showInputError(string x)
        {
            if (x == "polynomial")
            {
                MessageBox.Show("Gib einen Funktionsnamen ein!"
                                + "\r\n" + "Der Name muss:"
                                + "\r\n" + "-Ein Buchstabe sein"
                                + "\r\n" + "-Kleingeschrieben sein"
                                + "\r\n"
                                + "\r\n" + "Grad der Funktion muss > 1 sein"
                                + "\r\n" + "Grad der Funktion muss < 26 sein");
            }
            else
            {
                MessageBox.Show("Gib einen Funktionsnamen ein!"
                               + "\r\n" + "Der Name muss:"
                               + "\r\n" + "-Ein Buchstabe sein"
                               + "\r\n" + "-Kleingeschrieben sein");
            }
        }

        /// <summary>
        /// Gibt eine Fehlermeldung bei falscher Eingabe der Parameter zur berechnung aus
        /// </summary>
        private void showCalculationInputError()
        {
            MessageBox.Show("Bitte geben Sie eine Dezimalzahl ein");
        }

        /// <summary>
        /// Zeigt einen Fehler zu nicht ausgewählten Funktionen
        /// </summary>
        private void showFunctionSelectionError()
        {
            MessageBox.Show("Bitte wählen Sie eine Funktion aus");
        }

        /// <summary>
        /// Erstellt einen Array mit den RadioButtons zur weiteren Verwendung
        /// </summary>
        private void setUpRadioButtonsArray()
        {
            int index = 0;
            foreach (RadioButton c in groupBox2.Controls)
            {
                radioButtons[index] = c;
                radioButtons[index].Visible = false;
                index++;
            }
            Array.Reverse(radioButtons);
        }

        /// <summary>
        /// Wählt beim Auf/Ableiten die neue Funktion direkt aus und löscht die 
        /// angezeigten Ergebnisse der zuvor ausgewählten Funktion
        /// </summary>
        private void updateFunctionSelection()
        {
            int x = getSelectedFunctionArrayIndex();
            x++;
            radioButtons[x].Checked = true;
            clearTextBoxes();
        }

        /// <summary>
        /// Löscht alle TextBoxen in denen Ergebnisse angezeigt werden
        /// </summary>
        private void clearTextBoxes()
        {
            textBox7.Text = String.Empty;
            textBox5.Text = String.Empty;
            textBox9.Text = String.Empty;
            textBox13.Text = String.Empty;
            textBox14.Text = String.Empty;
            textBox15.Text = String.Empty;
            textBox16.Text = String.Empty;
        }

        /// <summary>
        /// Zeigt die ausgewählte Funktion in der großen Anzeigeleiste (TextBox11) an
        /// </summary>
        public void updateSelectedFunctionDisplay()
        {
            try
            {
                int index = getSelectedFunctionArrayIndex();
                if (index >= 0) textBox11.Text = function[index].printFunction();
                else textBox11.Text = String.Empty;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// wählt die zuletzt erstellte/generierte Funktion aus
        /// </summary>
        public void selectLatestFunction()
        {
            if (functionIndex > 0)
            {
                radioButtons[functionIndex - 1].Checked = true;
            }
        }

        /// <summary>
        /// Überprüft den eingegebenen Funktionsnamen
        /// </summary>
        /// <param name="pName"></param>
        /// <returns></returns>
        private bool checkName(string pName)
        {
            try
            {
                string name = Convert.ToString(pName);
                for (int i = 0; i < 26; i++)
                {   //darf nur ein einzelner Kleinbuchstabe sein
                    if (name == smallLetters[i]) return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Überprüft restliche Eingaben auf Form1
        /// </summary>
        /// <returns></returns>
        private bool checkInput()
        {
            if (radioButton1.Checked)
            {
                try
                {
                    string nameTest = Convert.ToString(textBox1.Text);
                    int degreeTest = Convert.ToInt32(textBox2.Text);
                    if (checkName(nameTest) && degreeTest > 1 && degreeTest < 26)
                    {
                        degree = degreeTest;
                        return true;
                    }
                    else
                    {
                        showInputError("polynomial");
                        return false;
                    }
                }
                catch (Exception)
                {
                    showInputError("polynomial");
                    return false;
                }
            }
            else
            {
                string nameTest = Convert.ToString(textBox1.Text);
                if (checkName(nameTest))
                {
                    return true;
                }
                else
                {
                    showInputError("Error bei checkInput()");
                    return false;
                }
            }
        }

        /// <summary>
        /// Üperprüft die Eingabe einer Textbox
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        private bool checkCalculationInput(TextBox tb)
        {
            try
            {
                Convert.ToDouble(tb.Text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// Zeigt den Error für falsche Eingabewerte bei der Flächenberechnung an
        /// </summary>
        private void showAreaInputError()
        {
            MessageBox.Show("Fehler bei Eingabe der Integrationsgrenzen" + "\r\n"
                             + "\r\n" + "-Grenzen können nicht gleichgroß sein"
                             + "\r\n" + "-untere Grenze muss kleiner als die obere sein");
        }

        /// <summary>
        /// Überprüft die Eingabewerte für die Flächenberechnung
        /// </summary>
        /// <returns></returns>
        private bool checkAreaInput()
        {
            double lb, ub;
            try
            {
                lb = Convert.ToDouble(textBox18.Text);
                ub = Convert.ToDouble(textBox17.Text);
                if (lb > ub || lb == ub)
                {   //dürfen nicht gleich sein und müssen in der RIchtigen Reihenfolge sein
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }   
            return true;
        }

        /// <summary>
        /// Initialisiert den Creator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                if (radioButton1.Checked)
                {
                    creator = new Creator(this, "polynomial", degree);
                }
                else if (radioButton2.Checked)
                {
                    creator = new Creator(this, "line");
                }
                else if (radioButton3.Checked)
                {
                    creator = new Creator(this, "sine");
                }
                else if (radioButton4.Checked)
                {
                    creator = new Creator(this, "cosine");
                }
            }
        }

        /// <summary>
        /// Initialisiert die neu erstellte Funktion mit Parameter und Namen und zeigt diese dann auf der GUI an
        /// </summary>
        /// <param name="type"></param>
        public void initializeFunction(string type)
        {
            if (type == "line")
            {   //Erzeugen der Funktion
                function[functionIndex] = new Polynomial(creator.creatorParameters, creator.creatorExponents); 
                function[functionIndex].name = Convert.ToString(textBox1.Text);             //Name setzen
                radioButtons[functionIndex].Text = function[functionIndex].printFunction(); //Anzeigen auf der GUI
            }
            if (type == "sine")
            {
                function[functionIndex] = new Sine(creator.creatorParameters);
                function[functionIndex].name = Convert.ToString(textBox1.Text);
                radioButtons[functionIndex].Text = function[functionIndex].printFunction();
            }
            if (type == "cosine")
            {
                function[functionIndex] = new Cosine(creator.creatorParameters);
                function[functionIndex].name = Convert.ToString(textBox1.Text);
                radioButtons[functionIndex].Text = function[functionIndex].printFunction();
            }
            if (type == "polynomial")
            {
                function[functionIndex] = new Polynomial(creator.creatorParameters, creator.creatorExponents);
                function[functionIndex].name = Convert.ToString(textBox1.Text);
                radioButtons[functionIndex].Text = function[functionIndex].printFunction();
            }
            radioButtons[functionIndex].Visible = true;
            functionIndex++;
        }

        /// <summary>
        /// Gibt den Arrayindex der ausgewählten Funktion zurück
        /// </summary>
        /// <returns></returns>
        public int getSelectedFunctionArrayIndex()
        {
            for (int i = 0; i < radioButtons.Length; i++)
            {
                if (radioButtons[i].Checked == true)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gibt den Typ der ausgewählten Funktion zurück
        /// </summary>
        /// <returns></returns>
        public string getSelectedFunctionType()
        {
            string type = "";
            int index = getSelectedFunctionArrayIndex();
            if (index >= 0) type = function[index].getFunctionType();
            return type;
        }

        /// <summary>
        /// Zeigt eine Fehlermeldung bei fehlgeschlagenem integrieren an
        /// </summary>
        public void showIntegrationError()
        {
            MessageBox.Show("Die Funktion konnte nicht integriert werden" + "\r\n"
                            + "Bitte beachten Sie:" + "\r\n"
                            + "Stammfunktionen können nicht integriert werden" + "\r\n"
                            + "Funktionen der Form f(x) = 0 können nicht integriert werden");
        }

        /// <summary>
        /// Zeigt eine Fehlermeldung bei fehlgeschlagenem ableiten an
        /// </summary>
        public void showDerivationError()
        {
            MessageBox.Show("Die Funktion konnte nicht abgeleitet werden" + "\r\n"
                           + "Bitte beachten Sie:" + "\r\n"
                           + "Funktionen der Form f(x) = 0 können nicht weiter abgeleitet werden");
        }

        #region radioButtonClicks
        private void radioButton1_CheckedChanged(object sender, EventArgs e) { setInputMode(1); }

        private void radioButton2_CheckedChanged(object sender, EventArgs e) { setInputMode(2); }

        private void radioButton3_CheckedChanged(object sender, EventArgs e) { setInputMode(2); }

        private void radioButton4_CheckedChanged(object sender, EventArgs e) { setInputMode(2); }
        #endregion

        #region berechnungen
        private void button10_Click(object sender, EventArgs e) //Löschen von Funktionen
        {
            int x = getSelectedFunctionArrayIndex();
            if (x >= 0)
            {
                for (int i = x; i < functionIndex - 1; i++) //Verschieben der Funktionen unter der gelöschten
                {
                    if (functionIndex >= 1 && function[i + 1] != null) 
                    {
                        function[i] = function[i + 1];
                        radioButtons[i].Text = function[i].printFunction();
                    }
                }
                if (functionIndex > 0) { functionIndex--; }
                radioButtons[functionIndex].Visible = false; //letzten Radio Button unsichtbar machen
                if (x > 0)
                {
                    if (radioButtons[x].Visible == false)    //Falls die unterste Funktion auf der GUI gelöscht wurde
                    {
                        radioButtons[x - 1].Select();        //Auswählen der jetzt letzten Funktion
                    }
                }
                if (functionIndex == 0)                     //Falls die letzte Funktion überhaupt im Array gelöscht wurde
                {
                    radioButtons[0].Checked = false;        
                }
                clearTextBoxes();
            }
        }

        private void button6_Click(object sender, EventArgs e) //Funktionswertberechnung
        {
            int index = getSelectedFunctionArrayIndex();
            if (index >= 0)
            {
                if (checkCalculationInput(textBox8) == true)
                {
                    int roundDigits = function[index].roundDigits;
                    double x = Convert.ToDouble(textBox8.Text);
                    textBox7.Text = Math.Round(function[index].calculateYValue(x), roundDigits).ToString();
                }
                else
                {
                    showCalculationInputError();
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button5_Click(object sender, EventArgs e) //Steigungsberechnung
        {
            int index = getSelectedFunctionArrayIndex();
            
            if (index >= 0)
            {
                if (checkCalculationInput(textBox6) == true)
                {
                    int roundDigits = function[index].roundDigits;
                    double x = Convert.ToDouble(textBox6.Text);
                    Function derivative = function[index].createDerivative();
                    textBox5.Text = Math.Round(derivative.calculateYValue(x),roundDigits).ToString();
                }
                else
                {
                    showCalculationInputError();
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button4_Click(object sender, EventArgs e) //Integralberechnung
        {
            int index = getSelectedFunctionArrayIndex();

            if (index >= 0)
            {
                if (checkCalculationInput(textBox3) == true && checkCalculationInput(textBox4) == true)
                {
                    double x1 = Convert.ToDouble(textBox3.Text);
                    double x2 = Convert.ToDouble(textBox4.Text);
                    Function Integral = function[index].createIntegral();
                    double result;
                    result = Integral.calculateYValue(x2) - Integral.calculateYValue(x1);
                    result = Math.Round(result, 3);
                    textBox9.Text = result.ToString();
                }
                else
                {
                    showCalculationInputError();
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button2_Click(object sender, EventArgs e) //Aufleiten
        {
            if (functionIndex == 15)
            {
                MessageBox.Show("Limit an Funktionen erreciht");
            }
            else
            {
                int selected = getSelectedFunctionArrayIndex();
                if (selected >= 0)
                {
                    if (function[selected].checkIntegrationPossible())
                    {
                        function[functionIndex] = function[selected].createIntegral();
                        radioButtons[functionIndex].Text = function[functionIndex].printFunction();
                        radioButtons[functionIndex].Visible = true;
                        functionIndex++;
                        updateFunctionSelection();
                        updateSelectedFunctionDisplay();
                    }
                    else
                    {
                        showIntegrationError();
                    }
                }
                else
                {
                    showFunctionSelectionError();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e) //Ableiten
        {
            if (functionIndex == 15)
            {
                MessageBox.Show("Limit an Funktionen erreciht");
            }
            else
            {
                int selected = getSelectedFunctionArrayIndex();
                if (selected >= 0)
                {

                    if (function[selected].checkDerivationPossible())
                    {
                        function[functionIndex] = function[selected].createDerivative();
                        radioButtons[functionIndex].Text = function[functionIndex].printFunction();
                        radioButtons[functionIndex].Visible = true;
                        functionIndex++;
                        updateFunctionSelection();
                        updateSelectedFunctionDisplay();
                    }
                    else
                    {
                        showDerivationError();
                    }
                }
                else
                {
                    showFunctionSelectionError();
                }
            }
        }

        private void button9_Click(object sender, EventArgs e) //Nullstellenberechnung
        {
            int index = getSelectedFunctionArrayIndex();
            textBox13.Text = String.Empty;

            if (getSelectedFunctionType() == "polynomial")
            {
                string x;
                if (index >= 0)
                {
                    double[] zeros = function[index].calculateZeros();
                    if (zeros.Length == 0)
                    {
                        textBox13.Text = "Keine Nullstellen gefunden";
                    }
                    else
                    {
                        textBox13.Text = "Folgende Nullstellen wurden gefunden:" + "\r\n";
                        for (int i = 0; i < zeros.Length; i++)
                        {
                            x = "x" + function[index].getSubScript(i) + " ≈ ";
                            textBox13.Text += x;
                            textBox13.Text += zeros[i].ToString() + "\r\n";
                        }
                    }
                }
                else
                {
                    showFunctionSelectionError();
                }
            }
            else if (getSelectedFunctionType() == "sine" || getSelectedFunctionType() == "cosine")
            {
                if (index >= 0)
                {
                    double[] zeros = function[index].calculateZeros();

                    if (zeros.Length == 0)
                    {
                        textBox13.Text = "Es wurden keine Nullstellen gefunden";
                    }
                    else
                    {
                        textBox13.Text = "Folgende Nullstellen wurden gefunden:" + "\r\n";
                        string x;
                        for (int i = 0; i < zeros.Length; i++)
                        {
                            zeros[i] = Math.Round(zeros[i], 4);
                            x = "x" + function[index].getSubScript(i) + " ≈ ";
                            x += zeros[i].ToString() + " + k * p ; k ∈ ℤ";
                            textBox13.Text += x + "\r\n";
                        }
                    }
                }
                else
                {
                    showFunctionSelectionError();
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button11_Click(object sender, EventArgs e) // Extrempunkte
        {
            textBox14.Text = String.Empty;
            int index = getSelectedFunctionArrayIndex();

            if (index >= 0)
            {
                int roundDigits = function[index].roundDigits;
                function[index].calculateMaxima();
                double[] maxima = function[index].maxima;
                double[] minima = function[index].minima;

                if (maxima.Length == 0 && minima.Length == 0)
                {
                    textBox14.Text = "Es wurden keine Extrempunkte gefunden";
                }
                else
                {
                    if (function[index].getFunctionType() == "polynomial")
                    {
                        textBox14.Text = "Es wurden folgende Extrempunkte gefunden:" + "\r\n";
                    }
                    else
                    {
                        textBox14.Text = "Es wurden folgende (periodische) Extrempunkte gefunden:" + "\r\n";
                    }
                }
                
                for (int i = 0; i < maxima.Length; i++)
                {
                    string x = "HP (" + Math.Round(maxima[i],roundDigits) + " | " + Math.Round(function[index].calculateYValue(maxima[i]),roundDigits) + ")" + "\r\n";
                    textBox14.Text += x;
                }
                for (int i = 0; i < minima.Length; i++)
                {
                    string x = "TP (" + Math.Round(minima[i],roundDigits) + " | " + Math.Round(function[index].calculateYValue(minima[i]),roundDigits) + ")" + "\r\n";
                    textBox14.Text += x;
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button12_Click(object sender, EventArgs e) //Wendepunkte
        {
            textBox15.Text = String.Empty;
            int index = getSelectedFunctionArrayIndex();

            if (index >= 0)
            {
                int roundDigits = function[index].roundDigits;
                function[index].calculateTurns();
                double[] turns = function[index].turns;
                double[] saddles = function[index].saddles;

                if (turns.Length == 0 && saddles.Length == 0)
                {
                    textBox15.Text = "Es wurden keine Wendepunkte gefunden";
                }
                else
                {
                    if (function[index].getFunctionType() == "polynomial")
                    {
                        textBox15.Text = "Es wurden folgende Wendepunkte gefunden:" + "\r\n";
                    }
                    else
                    {
                        textBox15.Text = "Es wurden folgende (periodische) Wendepunkte gefunden:" + "\r\n";
                    }

                    for (int i = 0; i < turns.Length; i++)
                    {
                        string x = "WP (" + Math.Round(turns[i], roundDigits) + " | " + Math.Round(function[index].calculateYValue(turns[i]), roundDigits) + ")" + "\r\n";
                        textBox15.Text += x;
                    }
                    for (int i = 0; i < saddles.Length; i++)
                    {
                        string x = "SP (" + Math.Round(saddles[i], roundDigits) + " | " + Math.Round(function[index].calculateYValue(saddles[i]), roundDigits) + ")" + "\r\n";
                        textBox15.Text += x;
                    }
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }

        private void button13_Click(object sender, EventArgs e) //Fläche
        {
            int index = getSelectedFunctionArrayIndex();

            if (index >= 0)
            {
                if (checkAreaInput() == true)
                {
                    double lb = Convert.ToDouble(textBox18.Text);
                    double ub = Convert.ToDouble(textBox17.Text);
                    double Area = function[index].calculateArea(lb, ub);
                    textBox16.Text = Area.ToString();
                }
                else
                {
                    showAreaInputError();
                }
            }
            else
            {
                showFunctionSelectionError();
            }
        }
        #endregion

        #region RadioButtons Changed
        private void radioButton5_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton6_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton7_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton8_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton9_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton10_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton11_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton12_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton13_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton14_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton15_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton16_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton17_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }

        private void radioButton18_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); updateSelectedFunctionDisplay(); }
        
        private void radioButton19_CheckedChanged(object sender, EventArgs e) { clearTextBoxes(); }
        #endregion

        
    }
}
