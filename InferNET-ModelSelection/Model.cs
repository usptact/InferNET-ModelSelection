using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;

namespace InferNET_ModelSelection
{
    class Model
    {
        public Model()
        {

        }

        // generate synthetic data
        public static void GenerateData(int n, double modelProb, double probTreated, double probControl, double probRecovery,
                                        out bool[] treatedData, out bool[] controlData)
        {
            treatedData = new bool[n];
            controlData = new bool[n];

            Bernoulli model = new Bernoulli(modelProb);

            Bernoulli treated = new Bernoulli(probTreated);
            Bernoulli control = new Bernoulli(probControl);

            Bernoulli recovery = new Bernoulli(probRecovery);

            for (int i = 0; i < n; i++)
            {
                if (model.Sample())
                {
                    // has effect
                    treatedData[i] = treated.Sample();
                    controlData[i] = control.Sample();
                }
                else
                {
                    // no effect
                    treatedData[i] = recovery.Sample();
                    controlData[i] = recovery.Sample();
                }
            }
        }
    }
}
