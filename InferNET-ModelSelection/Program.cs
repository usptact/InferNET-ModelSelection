using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer.Distributions;

namespace InferNET_ModelSelection
{
    class Program
    {
        static void Main(string[] args)
        {
            //
            // True parameters for synthetic data generation
            //

            int n = 100;
            double probModel = 0.75;
            double probTreated = 0.90;
            double probControl = 0.55;
            double probRecovery = 0.50;

            Model.GenerateData(n, probModel, probTreated, probControl, probRecovery, out bool[] treatedData, out bool[] controlData);

            //
            // set priors
            //

            ModelData priors = new ModelData
            {
                modelPriorDist = new Bernoulli(0.5),
                treatedPriorDist = new Beta(1, 1),
                controlPriorDist = new Beta(1, 1),
                recoverPriorDist = new Beta(1, 1)
            };

            //
            // modeling
            //

            Model model = new Model();

            model.CreateModel();
            model.SetModelData(priors);
            ModelData posterior = model.InferModelData(treatedData, controlData);
        }
    }
}
