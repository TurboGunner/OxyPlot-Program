using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OxyPlot;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace OxyPlotProgram
{
    public partial class Form1 : Form
    {
        public bool random = false;
        public bool panel1Scroll = false;
        public bool independent = false;
        public bool enableEquations = false;
        public bool disablePoints = false;

        public static string[] labels;

        public List<Parser> parser;

        public Form1()
        {
            InitializeComponent();

            labels = new string[2];

            parser = new List<Parser>();
            parser.Add(new Parser());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Text = "Grapher On Roids!";

            //Random parameters/defaults
            darkLabel2.Text = "Floor: ";
            darkLabel3.Text = "Ceiling: ";
            darkTextBox1.Text = "0";
            darkTextBox2.Text = "100";

            //Title/Label parameters
            darkLabel4.Text = "Graph Title: ";
            darkLabel5.Text = "Graph Subtitle: ";
            darkLabel6.Text = "Equations";
            darkLabel7.Text = "Lower Bound: ";
            darkLabel8.Text = "Upper Bound: ";
            darkLabel9.Text = "Accuracy: ";
            darkLabel10.Text = "Units for X:";
            darkLabel11.Text = "Units for Y: ";

            //Misc Defaults
            darkTextBox3.Text = "Graph Subtitle";
            darkTextBox4.Text = "Graph Title";
            darkTextBox6.Text = "-10";
            darkTextBox7.Text = "10";
            darkTextBox8.Text = ".05";

            //Button Texts
            darkButton1.Text = "Update Graph";
            darkButton2.Text = "Add Point";
            darkButton3.Text = "Add Equation";

            //Checkbox Texts
            darkCheckBox1.Text = "Random?";
            darkCheckBox2.Text = "Independent #?";
            darkCheckBox3.Text = "Enable Equations?";
            darkCheckBox4.Text = "Disable Points?";

            darkTextBox5.Visible = false;
            darkLabel12.Visible = false;
            darkLabel13.Visible = false;

            WindowState = FormWindowState.Maximized;

            BackColor = Color.FromArgb(40, 42, 43);

            SideBar_Load(); //To avoid clogging the form load method
            Equation_Load();
        }

        private void SideBar_Load() //Also serves additive functions
        {
            int xMargin = 20;
            int yMargin = 20; //# of pixels of seperation between entry

            TextBox[] textBoxes = new TextBox[2];

            var darkLabel = new DarkUI.Controls.DarkLabel()
            {
                Location = new Point(0, yMargin * (darkSectionPanel1.Controls.Count / 2)),
                Text = (darkSectionPanel1.Controls.Count / 2 + 1) + "",
                BackColor = DarkUI.Config.Colors.GreyBackground
            };
            darkLabel.Size = new Size(new Point(darkLabel.Size.Width, (int)(darkLabel.Size.Height * 2.5) / 3));

            for (int i = 0; i < textBoxes.Length; i++)
            {
                textBoxes[i] = new TextBox()
                {
                    Location = new Point(((xMargin + 80) * i) + 20, yMargin * (darkSectionPanel1.Controls.Count / 2))
                };
                darkSectionPanel1.Controls.Add(textBoxes[i]);
                darkSectionPanel2.Controls.Add(darkLabel);
            }
            if (darkSectionPanel1.Size.Height <= ((darkSectionPanel1.Controls.Count / 2) + 1) * yMargin && !panel1Scroll) //Long if statement dictating maximum buffer margins, adds scrolling only after that buffer
            {
                ScrollBar vScrollBar1 = new VScrollBar()
                {
                    Size = new Size((darkSectionPanel2.Size.Width / 2), darkSectionPanel2.Size.Height),
                    Location = new Point(darkSectionPanel1.Location.X + darkSectionPanel1.Size.Width + 5, darkSectionPanel1.Location.Y)
                };
                vScrollBar1.Scroll += (sender, e) => { darkSectionPanel1.VerticalScroll.Value = vScrollBar1.Value; };
                vScrollBar1.Scroll += (sender, e) => { darkSectionPanel2.VerticalScroll.Value = vScrollBar1.Value; };
                ActiveForm.Controls.Add(vScrollBar1);
                panel1Scroll = true;
            }
            darkLabel1.Text = "Count: " + darkSectionPanel2.Controls.Count;
        }

        private void Equation_Load()
        {
            TextBox equation = new TextBox()
            {
                Location = new Point(9, 25 * darkSectionPanel3.Controls.Count),
                Size = new Size(518, 20)
            };
            darkSectionPanel3.Controls.Add(equation);
        }

        private async Task<Func<double, double>[]> EquationParsingLogic()
        {
            int length = darkSectionPanel3.Controls.Count;
            Func<double, double>[] functions = new Func<double, double>[length];

            for (int i = 0; i < darkSectionPanel3.Controls.Count; i++)
            {
                if(i != 0 && parser.Count != darkSectionPanel3.Controls.Count)
                {
                    parser.Add(new Parser());
                }
                parser[i].input = darkSectionPanel3.Controls[i].Text;
                parser[i].RunCode();
            }
            Func<double, double>[] output = new Func<double, double>[darkSectionPanel3.Controls.Count];
            for (int i = 0; i < parser.Count; i++)
            {
                output[i] = parser[i].output;
            }
            return await Task.Run(() => output);
        }

        Func<double, double>[] equations;
        PlotModel model;

        private DataPoint[] ParsingLogic()
        {
            int total = darkSectionPanel1.Controls.Count;
            int count = darkSectionPanel2.Controls.Count;

            List<float> temp1 = new List<float>();
            List<float> temp2 = new List<float>();

            DataPoint[] output = new DataPoint[count];
            for (int i = 0; i < total; i += 2)
            {
                float.TryParse(darkSectionPanel1.Controls[i].Text, out float oof);
                temp1.Add(oof);
                for (int j = 1; j < total; j += 2)
                {
                    float.TryParse(darkSectionPanel1.Controls[j].Text, out float oof2);
                    temp2.Add(oof2);
                }
            }
            for (int i = 0; i < count; i++)
            {
                output[i] = new DataPoint(temp1[i], temp2[i]);
            }
            return output;
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            model = Plotting.OxyParams(darkTextBox3.Text, darkTextBox4.Text);

            float.TryParse(darkTextBox1.Text, out DataPoint.floor);
            float.TryParse(darkTextBox2.Text, out DataPoint.ceiling);

            darkLabel12.Visible = true;
            darkLabel13.Visible = true;

            darkLabel12.Text = darkTextBox10.Text;
            darkLabel13.Text = darkTextBox9.Text;

            List<DataPoint> points = new List<DataPoint>();
            int length; //For random control and to ensure that independent mode for random works properly

            if (!darkSectionPanel3.Controls[0].Text.Equals("") && enableEquations)
            {
                Timer();
            }
            if (!disablePoints)
            {
                if (random && independent)
                {
                    int.TryParse(darkTextBox5.Text, out length);
                }
                else
                {
                    length = darkSectionPanel2.Controls.Count;
                }
                for (int i = 0; i < length; i++)
                {
                    if (random)
                    {
                        points.Add(new DataPoint(true));
                    }
                    else
                    {
                        if (i == 0) //Making sure that only on the first iteration, that points is overriden if random = false
                        {
                            points = ParsingLogic().ToList(); //Because it converts it with all elements already added, it breaks this loop
                        }
                        break;
                    }
                }
                Plotting.AddScatterPlotting(model, points.ToArray());
            }
            if (!enableEquations)
            {
                plotView1.Model = model;
            }
        }

        static System.Timers.Timer aTimer;

        private async void Timer()
        {
            equations = await EquationParsingLogic();
            for (int i = 0; i < darkSectionPanel3.Controls.Count; i++)
            {
                aTimer = new System.Timers.Timer()
                {
                    AutoReset = true,
                    Interval = 200,
                    Enabled = true
                };
                aTimer.Elapsed += new ElapsedEventHandler(Handler);
            }
        }

        private void Handler(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < equations.Length; i++)
            {
                if (equations[i] == null)
                {
                    return;
                }
            }

            if (equations.Length == darkSectionPanel3.Controls.Count)
            {
                for(int i = 0; i < parser.Count; i++)
                {
                    if(i != 0)
                    {
                        parser.RemoveAt(i);
                    }
                }
                aTimer.AutoReset = false;
                aTimer.Stop();
                double[] data = new double[3];
                double.TryParse(darkTextBox6.Text, out data[0]);
                double.TryParse(darkTextBox7.Text, out data[1]);
                double.TryParse(darkTextBox8.Text, out data[2]);

                for (int i = 0; i < darkSectionPanel3.Controls.Count; i++)
                {
                    Plotting.AddEquations(model, equations[i], null, data[0], data[1], data[2]);
                    equations[i] = null;
                }
                plotView1.Model = model;
            }
        }

        private void darkButton2_Click(object sender, EventArgs e)
        {
            SideBar_Load();
        }

        private void darkButton3_Click(object sender, EventArgs e)
        {
            Equation_Load();
        }

        private void darkCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            random = !random;
            darkTextBox5.Visible = (independent && random);
        }

        private void darkCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            independent = !independent;
            darkTextBox5.Visible = (independent && random);
        }

        private void darkCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            enableEquations = !enableEquations;
        }

        private void darkCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            disablePoints = !disablePoints;
        }
    }
}