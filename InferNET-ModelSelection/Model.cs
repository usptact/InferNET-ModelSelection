using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Models;
using MicrosoftResearch.Infer.Distributions;

namespace InferNET_ModelSelection
{
    class Model
    {
        Variable<bool> model;

        Variable<double> probTreated;
        Variable<double> probControl;
        Variable<double> probRecovery;

        VariableArray<bool> treated;
        VariableArray<bool> control;

        Variable<int> numItems;

        public Model()
        {

        }

        public void CreateModel()
        {
            model = Variable.New<bool>();

            probTreated = Variable.New<double>();
            probControl = Variable.New<double>();
            probRecovery = Variable.New<double>();

            numItems = Variable.New<int>();
            Range items = new Range(numItems);

            treated = Variable.Array<bool>(items);
            control = Variable.Array<bool>(items);

            using (Variable.ForEach(items))
            {
                using (Variable.If(model))
                {
                    treated[items] = Variable.Bernoulli(probTreated);
                    control[items] = Variable.Bernoulli(probControl);
                }

                using (Variable.IfNot(model))
                {
                    treated[items] = Variable.Bernoulli(probRecovery);
                    control[items] = Variable.Bernoulli(probRecovery);
                }
            }
        }

        public void SetModelData(ModelData modelData)
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
