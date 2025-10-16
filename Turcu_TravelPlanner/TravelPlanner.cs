using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turcu_TravelPlanner
{
    public partial class TravelPlanner : Form
    {
        int dir = 0, oo = 1000000000, ruta = 0;
        int[] drum_masina = new int[100];
        int l_drum_masina = 0;
        int[] drum_tren = new int[100];
        int l_drum_tren = 0;
        int[] drum_pejos = new int[100];
        int l_drum_pejos = 0;
        Pen pen = new Pen(Color.Black, 10);
        private void pen_select(ref Pen pen)
        {
            if(ruta==0)
                pen = new Pen(Color.Black, 10);
            else if (ruta == 1)
                pen = new Pen(Color.Blue, 10);
            else if (ruta == 2)
                pen = new Pen(Color.Red, 10);
            else if (ruta == 3)
                pen = new Pen(Color.Green, 10);
        }
        
        public TravelPlanner()
        {
            InitializeComponent();
            Punct();
        }
        private void citire(ref int[,] h,ref int n, StreamReader f)
        {
            //StreamReader f = new StreamReader("..\\..\\input.txt");
            string linie;
            string[] chr;
            linie = f.ReadLine();
            chr = linie.Split(' ');
            n = int.Parse(chr[0]);
            int nod1, nod2, cost;
            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= n; j++)
                    if (i == j)
                        h[i, j] = 0;
                    else
                        h[i, j] = oo;
            while ((linie = f.ReadLine()) != null)
            {
                chr = linie.Split(' ');
                nod1 = int.Parse(chr[0]);
                nod2 = int.Parse(chr[1]);
                cost = int.Parse(chr[2]);
                h[nod1, nod2] = cost;
                h[nod2, nod1] = cost;
            }
            f.Close();
        }
        
        private void generare_drum(int x, ref int[] d,int[] v, int[,] h, int n, ref int[] t)
        {
            int i, j, minim, y = 0;
            v[x] = 1;
            for (i = 1; i <= n; i++)
            {
                d[i] = h[x, i];
                if (i != x && d[x] < oo)
                    t[i] = x;
            }
            for (i = 1; i <= n; i++)
            {
                minim = oo;
                for (j = 1; j <= n; j++)
                    if (v[j] == 0 && d[j] < minim)
                    {
                        minim = d[j];
                        y = j;
                    }
                v[y] = 1;
                for (j = 1; j <= n; j++)
                {
                    if (v[j] == 0 && d[j] > d[y] + h[y, j])
                    {
                        d[j] = d[y] + h[y, j];
                        t[j] = y;
                    }
                }
            }
        }
        
        private void drum(int i, ref int j, int[] t, ref int[] poteca)
        {
            if (t[i] != 0)
                drum(t[i], ref j, t, ref poteca);
            poteca[j] = i; j++;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Refresh();

            if (textBoxPlecare.Text == "" && departureButton != null)
            {
                departureButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                departureButton = null;
            }

            if (textBoxDestinatie.Text == "" && destinationButton != null)
            {
                destinationButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                destinationButton = null;
            }

            if (departureButton != null)
                departureButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
            if (destinationButton != null)
                destinationButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
            for (int i = 1; i < orase.Length; i++)
            {
                if (textBoxPlecare.Text == orase[i])
                {
                    // Assuming the button names are nod1, nod2, nod3, etc.
                    // You can adjust this according to your button names
                    Button button = Controls.Find("nod" + i, true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        departureButton = button;
                        departureButtonClicked = true;
                        button.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_plecare;
                        break; // Exit the loop after the first match is found
                    }
                }

            }
            for (int i = 1; i < orase.Length; i++)
            {
                if (textBoxDestinatie.Text == orase[i])
                {

                    // Assuming the button names are nod1, nod2, nod3, etc.
                    // You can adjust this according to your button names
                    Button button = Controls.Find("nod" + i, true).FirstOrDefault() as Button;
                    if (button != null)
                    {
                        destinationButton = button;
                        destinationButtonClicked = true;
                        button.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_destinatie;
                        break; // Exit the loop after the first match is found
                    }
                }
            }
            if (departureButton != null && destinationButton != null)
            {
                pen_select(ref pen);
                linie(departureButton.Name, destinationButton.Name, pen);
                panelTraseu.Visible = true;

                StreamReader f_car = new StreamReader("..\\..\\car.txt");
                StreamReader f_tren = new StreamReader("..\\..\\tren.txt");
                StreamReader f_km = new StreamReader("..\\..\\km.txt");

                int[,] ad_car = new int[100, 100];
                int[,] ad_tren = new int[100, 100];
                int[,] ad_km = new int[100, 100];
                int[] traseu_car = new int[100];
                int[] traseu_tren = new int[100];
                int[] traseu_km = new int[100];
                int[] cost_car = new int[100];
                int[] cost_tren = new int[100];
                int[] cost_km = new int[100];
                int[] v_car = new int[100];
                int[] v_tren = new int[100];
                int[] v_km = new int[100];
                int[] poteca_car = new int[100];
                int[] poteca_tren = new int[100];
                int[] poteca_km = new int[100];

                int nod1, nod2, j_car = 0, j_tren = 0, j_km = 0, n_car =0, n_tren = 0, n_km = 0;

                nod1 = Convert.ToInt32(departureButton.Name.Substring(3));
                nod2 = Convert.ToInt32(destinationButton.Name.Substring(3));
                
                citire(ref ad_car, ref n_car,f_car);
                citire(ref ad_tren, ref n_tren, f_tren);
                citire(ref ad_km, ref n_km, f_km);
                generare_drum(nod1, ref cost_car, v_car, ad_car, n_car,ref traseu_car);
                generare_drum(nod1, ref cost_tren, v_tren, ad_tren, n_tren, ref traseu_tren);
                generare_drum(nod1, ref cost_km, v_km, ad_km, n_km, ref traseu_km);
                drum(nod2, ref j_car, traseu_car, ref poteca_car);
                drum(nod2, ref j_tren, traseu_tren, ref poteca_tren);
                drum(nod2, ref j_km, traseu_km, ref poteca_km);

                int km_drum_masina = 0, km_drum_tren = 0;
                for (int z = 0; z < j_car - 1; z++)
                    km_drum_masina += ad_km[poteca_car[z],poteca_car[z+1]];
                for (int z = 0; z < j_tren - 1; z++)
                    km_drum_tren += ad_km[poteca_tren[z], poteca_tren[z + 1]];

                int t_drum_masina = cost_car[nod2];
                if (t_drum_masina % 60 == 0)
                    label2.Text = t_drum_masina / 60 + " h";
                else if (t_drum_masina > 60)
                    label2.Text = t_drum_masina / 60 + " h " + t_drum_masina % 60 + " min";
                else
                    label2.Text = t_drum_masina + " min";
                label4.Text = km_drum_masina + " km";

                int t_drum_tren = cost_tren[nod2];
                if (t_drum_tren % 60 == 0)
                    label6.Text = t_drum_tren / 60 + " h";
                else if (t_drum_masina > 60)
                    label6.Text = t_drum_tren / 60 + " h " + t_drum_tren % 60 + " min";
                else
                    label6.Text = t_drum_tren + " min";
                label5.Text = km_drum_tren + " km";

                int t_drum_pejos = cost_km[nod2]*12;
                if(t_drum_pejos % 1440 == 0)
                    label8.Text = t_drum_pejos / 1440 + " d";
                else if (t_drum_pejos > 1440)
                {
                    if (t_drum_pejos % 60 == 0)
                        label8.Text = t_drum_pejos / 1440 + " d " + t_drum_pejos % 1440 / 60 + " h";
                    else
                        label8.Text = t_drum_pejos / 1440 + " d " + t_drum_pejos % 1440 / 60 + " h " + t_drum_pejos % 60 + " min";
                }
                else if (t_drum_pejos % 60 == 0)
                    label8.Text = t_drum_pejos / 60 + " h";
                else if (t_drum_pejos > 60)
                    label8.Text = t_drum_pejos / 60 + " h " + t_drum_pejos % 60 + " min";
                else
                    label8.Text = t_drum_pejos + " min";
                label7.Text = cost_km[nod2] + " km";

                drum_masina = poteca_car;
                l_drum_masina = j_car;
                drum_tren = poteca_tren;
                l_drum_tren = j_tren;
                drum_pejos = poteca_km;
                l_drum_pejos = j_km;
            }

            else
            {
                MessageBox.Show("Introduceti corect punctul de plecare si destinatia!", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (textBoxPlecare.Text == "")
                {
                    textBoxPlecare.Focus();
                    dir = 1;
                }
                    
                else
                {
                    textBoxDestinatie.Focus();
                    dir = 2;   
                }
                    
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ruta = 1;
            pen_select(ref pen);
            Refresh();
            gen_linie(l_drum_masina, drum_masina);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ruta = 2;
            pen_select(ref pen);
            Refresh();
            gen_linie(l_drum_tren, drum_tren);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ruta = 3;
            pen_select(ref pen);
            Refresh();
            gen_linie(l_drum_pejos, drum_pejos);
        }
        private void gen_linie(int j, int[] poteca)
        {
            int i;
            for (i = 0; i < j; i++)
            {
                string n1 = "nod" + poteca[i].ToString();
                string n2 = "nod" + poteca[i + 1].ToString();
                linie(n1, n2, pen);
            }
        }
        private void linie(string buttonName1, string buttonName2, Pen pen)
        {
            Control[] controls1 = this.Controls.Find(buttonName1, true);
            Control[] controls2 = this.Controls.Find(buttonName2, true);

            if (controls1.Length > 0 && controls2.Length > 0)
            {
                Button btn1 = (Button)controls1[0];
                Button btn2 = (Button)controls2[0];

                Point p1 = btn1.PointToScreen(Point.Empty);
                Point p2 = btn2.PointToScreen(Point.Empty);

                // Convert button coordinates to form coordinates
                p1 = this.PointToClient(p1);
                p2 = this.PointToClient(p2);

                // Calculate center points
                int x1 = p1.X + btn1.Width / 2;
                int y1 = p1.Y + btn1.Height / 2;
                int x2 = p2.X + btn2.Width / 2;
                int y2 = p2.Y + btn2.Height / 2;

                startPoint = new Point(x1, y1);
                endPoint = new Point(x2, y2);

                // Draw line
                using (Graphics g = this.CreateGraphics())
                {
                    g.DrawLine(pen, x1, y1, x2, y2);
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Punct()
        {
            foreach (Control control in Controls)
            {
                if (control is Button && control.Name.StartsWith("nod"))
                {
                    ((Button)control).Click += Punct_Click;
                    ((Button)control).MouseEnter += Punct_MouseEnter;
                    ((Button)control).MouseLeave += Punct_MouseLeave;
                }
            }
        }
        private bool departureButtonClicked = false;
        private bool destinationButtonClicked = false;
        private Button departureButton = null;
        private Button destinationButton = null;
        private Button currenthoveredButton = null;

        private void Punct_Click(object sender, EventArgs e)
        {
            ruta = 0;
            pen_select(ref pen);
            Button clickedButton = sender as Button;
            panelTraseu.Visible = false;
            if (clickedButton != null)
            {
                if (dir == 0)
                {
                    dir = 1;
                }
                if (dir == 1)
                {
                    textBoxPlecare.Font = new Font(textBoxPlecare.Font, FontStyle.Regular);
                    textBoxPlecare.ForeColor = SystemColors.ControlText;
                    Refresh();
                    if (destinationButton != null)
                    {
                        destinationButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                        destinationButton = null;
                        textBoxDestinatie.Text = "";
                    }
                    if (departureButton != clickedButton)
                    {
                        ResetPreviouslySelectedButtons();
                        departureButton = clickedButton;
                        departureButtonClicked = true;
                        destinationButtonClicked = false;
                        clickedButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_plecare;
                        textBoxPlecare.Text = orase[Convert.ToInt32(clickedButton.Name.Substring(3))];
                    }
                    dir = 2;
                }
                else if (dir == 2)
                {
                    textBoxDestinatie.Font = new Font(textBoxDestinatie.Font, FontStyle.Regular);
                    textBoxDestinatie.ForeColor = SystemColors.ControlText;
                    if (destinationButton != null)
                        Refresh();
                    if (destinationButton != clickedButton)
                    {
                        ResetPreviouslySelectedButtons2();
                        destinationButton = clickedButton;
                        departureButtonClicked = false;
                        destinationButtonClicked = true;
                        clickedButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_destinatie;
                        textBoxDestinatie.Text = orase[Convert.ToInt32(clickedButton.Name.Substring(3))];
                    }
                    if (departureButtonClicked && departureButton != destinationButton)
                    {
                        departureButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_plecare;
                    }
                }
            }

            string buttonName = clickedButton.Name;
            string numericalPart = buttonName.Substring(3); // Cut "nod" from the button name

        }

        private void Punct_MouseEnter(object sender, EventArgs e)
        {
            Button hoveredButton = sender as Button;

            if (hoveredButton != null)
            {
                Label cityLabel = new Label();
                cityLabel.Text = orase[Convert.ToInt32(hoveredButton.Name.Substring(3))];
                cityLabel.AutoSize = true;
                cityLabel.BackColor = Color.White;
                cityLabel.BorderStyle = BorderStyle.FixedSingle;
                cityLabel.Name = "cityLabel_" + hoveredButton.Name.Substring(3); // Update label name
                if (hoveredButton.Name == "nod31")
                    cityLabel.Location = new Point(hoveredButton.Location.X + hoveredButton.Width / 2 - cityLabel.Width / 2 + 5, hoveredButton.Location.Y + hoveredButton.Height);
                else
                    cityLabel.Location = new Point(hoveredButton.Location.X, hoveredButton.Location.Y - cityLabel.Height + 5); // Adjust position as needed
                Controls.Add(cityLabel);
                if (dir == 0)
                    hoveredButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_hover;
                if (dir == 1 && hoveredButton != departureButton)
                {
                    if (currenthoveredButton != null && currenthoveredButton != departureButton && currenthoveredButton != destinationButton)
                    {
                        currenthoveredButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                    }
                    currenthoveredButton = hoveredButton;
                    hoveredButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_plecare_hover;
                    
                }
                else if (dir == 2 && hoveredButton != destinationButton)
                {
                    if (currenthoveredButton != null && currenthoveredButton != departureButton && currenthoveredButton != destinationButton)
                    {
                        currenthoveredButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                    }
                    currenthoveredButton = hoveredButton;
                    hoveredButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_destinatie_hover;
                    
                }
            }
        }

        private void Punct_MouseLeave(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
            {
                if (control is Label && control.Name.StartsWith("cityLabel"))
                {
                    Controls.Remove(control);
                    break;
                }
            }
            if (departureButton != null && destinationButton != null && ruta==0)
                linie(departureButton.Name, destinationButton.Name, pen);
            else if (departureButton != null && destinationButton != null && ruta == 1)
                gen_linie(l_drum_masina, drum_masina);
            else if (departureButton != null && destinationButton != null && ruta == 2)
                gen_linie(l_drum_tren, drum_tren);
            else if (departureButton != null && destinationButton != null && ruta == 3)
                gen_linie(l_drum_pejos, drum_pejos);
            Button leftButton = sender as Button;
            if (leftButton != null && !(leftButton == departureButton || leftButton == destinationButton))
            {
                leftButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
            }
            if(leftButton == departureButton)
                leftButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_plecare;
            if (leftButton == destinationButton)
                leftButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct_destinatie;
        }

        private void ResetPreviouslySelectedButtons()
        {
            if (departureButton != null)
            {
                departureButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                departureButtonClicked = false;
            }
            if (destinationButton != null)
            {
                destinationButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                destinationButtonClicked = false;
            }
        }
        private void ResetPreviouslySelectedButtons2()
        {
            if (destinationButton != null)
            {
                destinationButton.BackgroundImage = global::Turcu_TravelPlanner.Properties.Resources.pct;
                destinationButtonClicked = false;
            }
        }
        private void textBoxPlecare_MouseCaptureChanged(object sender, EventArgs e)
        {
            dir = 1;
            textBoxPlecare.Text = "";
            textBoxPlecare.Font = new Font(textBoxPlecare.Font, FontStyle.Regular);
            textBoxPlecare.ForeColor = SystemColors.ControlText;
        }

        private void textBoxDestinatie_MouseCaptureChanged(object sender, EventArgs e)
        {
            dir = 2;
            textBoxDestinatie.Text = "";
            textBoxDestinatie.Font = new Font(textBoxDestinatie.Font, FontStyle.Regular);
            textBoxDestinatie.ForeColor = SystemColors.ControlText;
        }

        string[] orase = {
            "", // 0
            "Satu Mare", // 1
            "Baia Mare", // 2
            "Oradea", // 3
            "Zalau", // 4
            "Bistrita", // 5
            "Suceava", // 6
            "Botosani", // 7
            "Cluj-Napoca", // 8
            "Targu Mures", // 9
            "Miercurea Ciuc", // 10
            "Piatra Neamt", // 11
            "Iasi", // 12
            "Bacau", // 13
            "Vaslui", // 14
            "Arad", // 15
            "Alba Iulia", // 16
            "Sfantu Gheorghe", // 17
            "Timisoara", // 18
            "Deva", // 19
            "Sibiu", // 20
            "Brasov", // 21
            "Focsani", // 22
            "Galati", // 23
            "Resita", // 24
            "Targu Jiu", // 25
            "Ramnicu Valcea", // 26
            "Pitesti", // 27
            "Targoviste", // 28
            "Ploiesti", // 29
            "Buzau", // 30
            "Braila", // 31
            "Tulcea", // 32
            "Drobeta-Turnu Severin", // 33
            "Craiova", // 34
            "Slatina", // 35
            "Alexandria", // 36
            "Giurgiu", // 37
            "Bucuresti", // 38
            "Slobozia", // 39
            "Calarasi", // 40
            "Constanta" // 41
        };

        private Point? startPoint;
        private Point? endPoint;

        

        public void TravelPlanner_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        
    }
}
