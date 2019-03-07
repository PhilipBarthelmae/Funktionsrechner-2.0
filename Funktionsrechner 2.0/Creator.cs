using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Funktionsrechner_2._0
{
    class Creator : Form
    {

        Form1 GUI;                          //Assoziation GUI
        Label title;                        //Überschrift im Fenster
        Font standartStyle = new Font("Century Gothic", 10);
        Font titleFont = new Font("Century Gothic", 10, FontStyle.Bold);
        Button OK = new Button();           

        TextBox[] textBoxes;                //Eingabefelder
        Label[] labels;                     //labels zum Anzeigen des Parameterbuchstaben
        CheckBox[] checkBoxes;              //CheckBoxen falls Parameter von Pi abhängig ist
        Label[] pies;                       //Label zum anzeigen von Pi

        string type;                        //Typ der Funktion
        int degree;                         //Grad der Funktion (bei Polynomen)
        public double[] creatorParameters;  //eingegebene Parameter
        public int[] creatorExponents;      //dazugehöreige Exponenten

        public Creator(Form1 GUI, string type)             //Konstruktor für alles andere
        {
            this.GUI = GUI;
            Text = "Creator";
            Visible = true;
            this.type = type;
            initializeCreatorInteface(type);
        }
        public Creator(Form1 GUI, string type, int degree) //Konstruktor für Polynome
        {
            this.GUI = GUI;
            Text = "Creator";
            Visible = true;
            this.type = type;
            this.degree = degree;
            initializeCreatorInteface(type);
        }

        /// <summary>
        /// Initialisiert den OK Knopf
        /// </summary>
        private void initializeOKButton()
        {
            OK.Font = standartStyle;
            OK.Text = "OK";
            OK.Width = 100;
            Controls.Add(OK);
            OK.Click += OK_Click;
        }

        /// <summary>
        /// Sammelt eingegebene Parameter in Arrays und übergibt sie der GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, EventArgs e)
        {

            if (checkCreatorInput(type) == true)
            {
                if (type == "polynomial")
                {
                    creatorParameters = new double[degree + 1]; //initialisierung der Arrays
                    creatorExponents = new int[degree + 1];     // zum speichern der Eingabe
                    for (int i = 0; i <= degree; i++)           //Einlesen der Eingabe
                    {
                        creatorParameters[i] = Convert.ToDouble(textBoxes[i].Text);
                        creatorExponents[i] = i;
                    }
                    Array.Reverse(creatorExponents);            //da Exponenten innerhalb von Funktionen von Groß nach klein Verlaufen
                    GUI.initializeFunction(type);               //Initialisierung der Funktion
                    GUI.selectLatestFunction();                 //Neue funktionen werden automatisch ausgwählt
                    GUI.updateSelectedFunctionDisplay();        //Update der Anzeigeleiste
                }
                if (type == "line")
                {
                    if (Convert.ToDouble(textBoxes[0].Text) == 0) //horizontale -> nur 1 Parameter
                    {   //Analog zu Polynom
                        creatorParameters = new double[1];
                        creatorParameters[0] = Convert.ToDouble(textBoxes[1].Text);
                        creatorExponents = new int[1];
                        creatorExponents[0] = 0;
                        GUI.initializeFunction(type);
                        GUI.selectLatestFunction();
                        GUI.updateSelectedFunctionDisplay();
                    }
                    else
                    {
                        creatorExponents = new int[2];
                        creatorExponents[0] = 1;
                        creatorExponents[1] = 0;
                        creatorParameters = new double[2];
                        for (int i = 0; i < 2; i++)
                        {
                            creatorParameters[i] = Convert.ToDouble(textBoxes[i].Text);
                        }
                        GUI.initializeFunction(type);
                        GUI.selectLatestFunction();
                        GUI.updateSelectedFunctionDisplay();
                    }
                }
                if (type == "sine" || type == "cosine")
                {   //Analog zu Polynom
                    creatorParameters = new double[4];
                    for (int i = 0; i < 4; i++)
                    {
                        creatorParameters[i] = Convert.ToDouble(textBoxes[i].Text);
                        if (checkBoxes[i].Checked == true)
                        {
                            creatorParameters[i] = creatorParameters[i] * Math.PI;
                        }
                    }
                    GUI.initializeFunction(type);
                    GUI.selectLatestFunction();
                    GUI.updateSelectedFunctionDisplay();
                }

                this.Close();
            }
        }

        /// <summary>
        /// Zeigt eine Fehlermeldung zu falscher Eingabe an
        /// </summary>
        private void showInputError()
        {
            MessageBox.Show("Eingabe der Funktionsparameter"
                                + "\r\n" + "Die Parameter müssen:"
                                + "\r\n" + "-Eine Ganzzahl sein"
                                + "\r\n" + "-Eine Kommazahl sein"
                                + "\r\n" + "-Parameter a != 0 sein"
                                + "\r\n" + "-Bei Sinus/Kosinus: Parameter b != 0 sein"
                                + "\r\n"
                                + "\r\n" + "Alle Felder müssen ausgefüllt sein");
        }

        /// <summary>
        /// Initialisiert und benennt die Überschrift im Fenster
        /// </summary>
        /// <param name="titleText"></param>
        private void setTitle(string titleText)
        {
            title = new Label();
            title.Text = titleText;
            title.Left = 55;
            title.Top = 15;
            if (type == "polynomial") { title.Width = 150; title.Left = 95; }
            title.Visible = true;
            title.Font = titleFont;
            Controls.Add(title);
        }

        /// <summary>
        /// Positioniert alle Oberflächenelemente gemäß der Ausgewählten Funktion und Grad
        /// </summary>
        /// <param name="type"></param>
        private void initializeCreatorInteface(string type)
        {
            if (type == "polynomial")
            {
                setTitle("Polynomfunktion");
                initializeOKButton();

                textBoxes = new TextBox[degree + 1];
                labels = new Label[degree + 1];
                int countery = 50;
                int counterx = 20;
                int distancey = 15;
                int distancex = 5;

                for (int i = 0; i < degree + 1; i++)
                {
                    // row by row
                    labels[i] = new Label();
                    labels[i].Text = Convert.ToString(GUI.smallLetters[i]);
                    labels[i].Width = 20;
                    textBoxes[i] = new TextBox();
                    labels[i].Font = standartStyle;
                    if (i % 2 == 0) //linke Spalte
                    {
                        //positionierung labels
                        labels[i].Left = counterx;
                        labels[i].Top = countery;
                        //positionierung textBoxes
                        textBoxes[i].Left = labels[i].Left + labels[i].Width + distancex;
                        textBoxes[i].Top = labels[i].Top;
                    }
                    if (i % 2 != 0) //rechte Spalte
                    {
                        //positionierung labels
                        labels[i].Left = textBoxes[i - 1].Left + textBoxes[i - 1].Width + distancex;
                        labels[i].Top = countery;
                        //positionierung TextBoxes
                        textBoxes[i].Left = labels[i].Left + labels[i].Width + distancex;
                        textBoxes[i].Top = labels[i].Top;
                        //Update countery (Höhe der Reihen) für die nächste Reihe
                        countery += labels[i].Height + distancey;
                    }
                    Controls.Add(labels[i]);        //Hinzufügen der Elemente
                    Controls.Add(textBoxes[i]);     //Hinzufügen der Elemente
                }
                OK.Top = labels[degree].Top + 2 * distancey;    //Positionierung OK Button
                OK.Left = textBoxes[0].Left;
                OK.Width = textBoxes[1].Left - textBoxes[0].Left + textBoxes[1].Width;
                //Größe des Creator
                this.Width = textBoxes[1].Left + textBoxes[1].Width + 2 * counterx;
                if (degree <= 9)
                {
                    int height = OK.Top + 100;
                    this.Height = height;
                }
                if (degree >= 10)
                {
                    double height = 5 * (textBoxes[0].Height + 50);
                    int windowHeight = Convert.ToInt32(height);
                    this.Height = windowHeight;
                    this.AutoScroll = true;
                    this.Width += 30;
                }
            }
            if (type == "line")
            {
                setTitle("Gerade");

                textBoxes = new TextBox[2];
                labels = new Label[2];
                int countery = 50;
                int counterx = 20;
                int distancey = 15;
                int distancex = 5;

                for (int i = 0; i < 2; i++)     //Creating Labels/textBoxes/checkBoxes
                {
                    labels[i] = new Label();
                    if (i == 0) { labels[i].Text = "m"; }
                    if (i == 1) { labels[i].Text = "b"; }
                    textBoxes[i] = new TextBox();
                    labels[i].Font = standartStyle;
                    //positioning (row by row)
                    //positioning labels
                    labels[i].Top = countery;
                    labels[i].Left = counterx;
                    labels[i].Width = 20;
                    //positioning textBoxes
                    textBoxes[i].Left = labels[i].Left + labels[i].Width + distancex;
                    textBoxes[i].Top = labels[i].Top;
                    countery += labels[i].Height + distancey;
                    //Adding the Elements
                    Controls.Add(labels[i]);
                    Controls.Add(textBoxes[i]);
                }
                this.Width = textBoxes[0].Left + textBoxes[0].Width + 3 * counterx;
                OK.Left = textBoxes[1].Left;
                OK.Top = textBoxes[1].Top + 2 * distancey;
                this.Height = OK.Top + 5 * distancey;
                initializeOKButton();
            }
            if (type == "sine" || type == "cosine")
            {
                if (type == "sine") { setTitle("Sinuskurve"); }
                if (type == "cosine") { setTitle("Kosinuskurve"); }

                initializeOKButton();
                textBoxes = new TextBox[4];
                labels = new Label[4];
                checkBoxes = new CheckBox[4];
                pies = new Label[4];
                int countery = 50;
                int counterx = 20;
                int distancey = 15;
                int distancex = 5;
                int checkdistance = 5;

                for (int i = 0; i < 4; i++)     //Creating Labels/textBoxes/checkBoxes
                {
                    labels[i] = new Label();
                    labels[i].Text = Convert.ToString(GUI.smallLetters[i]);
                    textBoxes[i] = new TextBox();
                    checkBoxes[i] = new CheckBox();
                    pies[i] = new Label();
                    pies[i].Text = "π";
                    labels[i].Font = standartStyle;
                    pies[i].Font = standartStyle;
                    //positioning (row by row)
                    //positioning labels
                    labels[i].Top = countery;
                    labels[i].Left = counterx;
                    labels[i].Width = 20;
                    //positioning textBoxes
                    textBoxes[i].Left = labels[i].Left + labels[i].Width + distancex;
                    textBoxes[i].Top = labels[i].Top;
                    //positioning checkBoxes
                    checkBoxes[i].Top = labels[i].Top - 2;
                    checkBoxes[i].Left = textBoxes[i].Left + textBoxes[i].Width + checkdistance;
                    checkBoxes[i].Width = 15;
                    checkBoxes[i].TabStop = false;
                    //positioning pies
                    pies[i].Left = checkBoxes[i].Left + checkBoxes[i].Width + checkdistance;
                    pies[i].Top = labels[i].Top - 2;
                    countery += labels[i].Height + distancey;
                    //Adding the Elements
                    Controls.Add(labels[i]);
                    Controls.Add(textBoxes[i]);
                    Controls.Add(checkBoxes[i]);
                    Controls.Add(pies[i]);
                }
                this.Width = pies[0].Left + 3 * counterx;
                OK.Left = textBoxes[3].Left;
                OK.Top = textBoxes[3].Top + 2 * distancey;
            }

        }

        /// <summary>
        /// Prüft die Eingabe auf Korrektheit
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool checkCreatorInput(string type)
        {
            int limit;
            if (type == "polynomial")
            {

                limit = degree + 1;
                try                              //Parameter a darf nicht 0 sein.
                {
                    double x = Convert.ToDouble(textBoxes[0].Text);
                    if (x == 0)
                    {
                        showInputError();
                        return false;
                    }
                }
                catch (Exception)
                {
                    showInputError();
                    return false;
                }
                for (int i = 1; i < limit; i++)     //Überpfüfung restlicher Parameter
                {
                    try
                    {
                        double x = Convert.ToDouble(textBoxes[i].Text);
                    }
                    catch (Exception)
                    {
                        showInputError();
                        return false;
                    }
                }
                return true;
            }
            else if (type == "sine" || type == "cosine")
            {
                //erste beiden parameter( dürfen beide nicht 0 sein)
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        double x = Convert.ToDouble(textBoxes[i].Text);
                        if (x == 0)
                        {
                            showInputError();
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        showInputError();
                        return false;
                    }
                }
                //letzte beide Parameter (dürfen 0 sein)
                for (int i = 2; i < 4; i++)
                {
                    try
                    {
                        double x = Convert.ToDouble(textBoxes[i].Text);
                    }
                    catch (Exception)
                    {
                        showInputError();
                        return false;
                    }
                }
                return true;
            }
            else if(type =="line")
            {
                //weil bei einer geraden der erste parameter 0 sein darf ist die Überprüfung separat
                limit = 2;
                for (int i = 0; i < limit; i++)
                {
                    try
                    {
                        double x = Convert.ToDouble(textBoxes[i].Text);
                    }
                    catch (Exception)
                    {
                        showInputError();
                        return false;
                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("Input Error");
                return false;
            }
        }

    }
}
