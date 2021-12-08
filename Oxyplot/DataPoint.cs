using OxyPlot.Series;

using System;

namespace OxyPlotProgram
{
    public class DataPoint
    {
        private float xPoint;
        private float yPoint;

        public static float floor;
        public static float ceiling;

        static Random rand = new Random();

        public DataPoint(float x, float y)
        {
            xPoint = x;
            yPoint = y;
        }
        public DataPoint(bool random = false) //Empty/Random
        {
            if (!random)
            {
                xPoint = 0;
                yPoint = 0;
            }
            else
            {
                xPoint = (float) (rand.NextDouble() * ceiling) + floor;
                yPoint = (float)(rand.NextDouble() * ceiling) + floor;
            }
        }

        //Accessor methods

        public float GetX()
        {
            return xPoint;
        }

        public float GetY()
        {
            return yPoint;
        }

        //Set methods

        public void SetX(float x)
        {
            xPoint = x;
        }
        public void SetY(float y)
        {
            yPoint = y;
        }

        //Operator methods w/ polymorphism

        public DataPoint Add(DataPoint b)
        {
            return new DataPoint(xPoint + b.xPoint, yPoint + b.yPoint);
        }

        public DataPoint Add(DataPoint a, DataPoint b)
        {
            return new DataPoint(a.xPoint + b.xPoint, a.yPoint + b.yPoint);
        }

        public DataPoint Subtract(DataPoint b)
        {
            return new DataPoint(xPoint - b.xPoint, yPoint - b.yPoint);
        }

        public DataPoint Subtract(DataPoint a, DataPoint b)
        {
            return new DataPoint(a.xPoint - b.xPoint, a.yPoint - b.yPoint);
        }

        public DataPoint Multiply(DataPoint b)
        {
            return new DataPoint(xPoint * b.xPoint, yPoint * b.yPoint);
        }

        public DataPoint Multiply(DataPoint a, DataPoint b)
        {
            return new DataPoint(a.xPoint * b.xPoint, a.yPoint * b.yPoint);
        }
        public DataPoint Divide(DataPoint b)
        {
            return new DataPoint(xPoint * b.xPoint, yPoint * b.yPoint);
        }

        public DataPoint Divide(DataPoint a, DataPoint b)
        {
            return new DataPoint(a.xPoint / b.xPoint, a.yPoint / b.yPoint);
        }

        public ScatterPoint ToScatterPoint()
        {
            return new ScatterPoint(xPoint, yPoint);
        }

        //To-string override

        public override string ToString()
        {
            return "X: " + xPoint + "\nY:" + yPoint;
        }

    }
}