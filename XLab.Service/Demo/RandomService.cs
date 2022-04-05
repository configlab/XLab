using System;
using System.Collections.Generic;
using System.Text;

namespace XLab.Service.Demo
{
    public class RandomService: IRandomService
    {
        public string GetRandom()
        {
            return new Random().Next(0, int.MaxValue).ToString();
        }
    }
}
