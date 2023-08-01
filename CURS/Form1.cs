using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace CURS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            myChart.Series.Clear();
            label1.Text = "";
            if (richTextBox1.Text.Trim().Length == 0 || richTextBox2.Text.Trim().Length == 0)
                MessageBox.Show("Одно из полей для значений X и Y пусто!", "ОШИБКА!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (richTextBox1.Text.Trim().Split(' ').Length != richTextBox2.Text.Trim().Split(' ').Length)
                MessageBox.Show("Количество X и Y не совпадает!", "ОШИБКА!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                var items = richTextBox2.Text.Trim().Split(' ');
                var item = richTextBox1.Text.Trim().Split(' ');
                var par = textBox1.Text.Trim();
                bool can = true;
                if (par.Length == 0)
                {
                    MessageBox.Show("В поле аргумента для поиска пусто!", "ОШИБКА!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    can = false;
                }
                else if (!Program.CanConvert(par))
                {
                    MessageBox.Show("В поле аргумента для поиска должно быть только число!", "ОШИБКА!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    can = false;
                }
                foreach (var i in item)
                {
                    if (!Program.CanConvert(i))
                    {
                        MessageBox.Show("В поле X должны быть только числа!", "ОШИБКА!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        can = false;
                        break;
                    }
                }
                foreach (var i in items)
                {
                    if (!Program.CanConvert(i))
                    {
                        MessageBox.Show("В поле Y должны быть только числа!", "ОШИБКА!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        can = false;
                        break;
                    }
                }
                if (can)
                {
                    Series mySeriesOfPoint = new Series();
                    mySeriesOfPoint.ChartType = SeriesChartType.Spline;
                    mySeriesOfPoint.Name = "1";
                    myChart.Series.Add(mySeriesOfPoint);
                    if (par.Contains("."))
                        par = par.Replace('.', ',');
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i].Contains("."))
                            item[i].Replace('.', ',');
                        if (items[i].Contains("."))
                            items[i].Replace('.', ',');
                        myChart.Series["1"].Points.AddXY(Convert.ToDouble(item[i]), 
                            Convert.ToDouble(items[i]));
                    }
                    myChart.Series["1"].Points[0].MarkerStyle = MarkerStyle.Circle;
                    myChart.Series["1"].Points[item.Length - 1].MarkerStyle = MarkerStyle.Circle;
                    Series m = new Series();
                    m.ChartType = SeriesChartType.Line;
                    m.MarkerStyle = MarkerStyle.Circle;
                    m.Name = "m";
                    myChart.Series.Add(m);
                    double res = Program.InterpolateLagrangePolynomial(Convert.ToDouble(par), 
                        Array.ConvertAll(item, Convert.ToDouble), 
                        Array.ConvertAll(items, Convert.ToDouble), item.Length);
                    label1.Text = "При X=" + Convert.ToString(par) + " , Y=" + Convert.ToString(res);
                    myChart.Series["m"].Points.AddXY(Convert.ToDouble(par), res);
                    if (myChart.Series["m"].Points[0].XValue > myChart.Series["1"].Points[item.Length - 1].XValue)
                        myChart.Series["m"].Points.AddXY(myChart.Series["1"].Points[item.Length - 1].XValue, 
                            myChart.Series["1"].Points[item.Length - 1].YValues[0]);
                    else if (myChart.Series["m"].Points[0].XValue < myChart.Series["1"].Points[0].XValue)
                        myChart.Series["m"].Points.AddXY(myChart.Series["1"].Points[0].XValue, 
                            myChart.Series["1"].Points[0].YValues[0]);
                    myChart.ChartAreas[0].AxisY.Minimum = double.NaN;
                    myChart.ChartAreas[0].AxisX.Minimum = double.NaN;
                }
            }
        }
    }
}
