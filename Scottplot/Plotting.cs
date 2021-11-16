using System;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OxyPlotProgram
{
    public class Plotting
    {
        static Random rand = new Random();

        public static PlotModel OxyParams(string sub, string t, AxisPosition a = AxisPosition.Left, AxisPosition b = AxisPosition.Bottom)
        {
            PlotModel p = new PlotModel()
            {
                Subtitle = sub,
                Title = t
            };

            var a1 = new LinearAxis();
            a1.Position = a;
            p.Axes.Add(a1);

            var a2 = new LinearAxis();
            a2.Position = b;
            p.Axes.Add(a2);

            return p;
        }
        public static void AddScatterPlotting(PlotModel p, DataPoint[] d)
        {
            var scatterSeries = new ScatterSeries();
            for (int i = 0; i < d.Length; i++)
            {
                scatterSeries.Points.Add(d[i].ToScatterPoint());
            }
            p.Series.Add(scatterSeries);
        }

        public static void AddEquations(PlotModel p, Func<double, double> f, OxyColor? c, double lower = -10, double upper = 10, double acc = .05)
        {
            OxyColor color;
            if(!c.HasValue)
            {
                color = OxyColor.Parse(string.Format("#{0:X6}", rand.Next(0x1000000)));
            }
            else
            {
                color = c.Value;
            }
            if (f != null)
            {
                p.Series.Add(new FunctionSeries(f, lower, upper, acc));
            }
        }
    }
}
