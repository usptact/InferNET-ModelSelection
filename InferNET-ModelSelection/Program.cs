using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InferNET_ModelSelection
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // Parameters for synthetic data generation
            //

            int n = 100;
            double probModel = 0.75;
            double probTreated = 0.90;
            double probControl = 0.55;
            double probRecovery = 0.50;

            Model.GenerateData(n, probModel, probTreated, probControl, probRecovery, out bool[] treatedData, out bool[] controlData);
        }
    }
}
