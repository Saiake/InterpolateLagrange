using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace CURS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static public double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size)
        {
            double lagrangePol = 0;

            for (int i = 0; i < size; i++)
            {
                double basicsPol = 1;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
                    }
                }
                lagrangePol += basicsPol * yValues[i];
            }

            return Math.Round(lagrangePol, 2);
        }
        static public bool CanConvert(string data)
        {
            data = data.Replace(',', '.');
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(double));
            return converter.IsValid(data);
        }
    }
}
