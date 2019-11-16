using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutofacSample.Services
{
    public class CalcService : ICalcService
    {
        public int Add(int x, int y)
        {
            return x + y;
        }

        public int Substract(int x, int y)
        {
            return x - y;
        }
    }

    public interface ICalcService
    {
        int Add(int x, int y);
        int Substract(int x, int y);
    }
}