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
            // Set this!
            //

            bool hasEffect = true;

            //
            // True parameters for synthetic data generation
            //

            // number of data points to generate
            int n = 100;

            // probability of recovery of administered the treatment
            double probTreated = 0.90;

            // probability of recovery if believed to be administered the treatment
            double probControl = 0.51;

            // probability of recovery if no treatment is administered
            double probRecovery = 0.50;

            bool[] treatedData, controlData;

            if (hasEffect)
                Model.GenerateHasEffectData(n, probTreated, probControl,
                                            out treatedData, out controlData);
            else
                Model.GenerateNoEffectData(n, probRecovery,
                                           out treatedData, out controlData);

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

            //
            // print posteriors
            //

            if (hasEffect)
            {
                Console.WriteLine("Has Effect Model probability: {0} +/- {1}",
                                  posterior.modelPriorDist.GetMean(),
                                  posterior.modelPriorDist.GetVariance());
                Console.WriteLine("probTreated: {0} +/- {1}",
                                  posterior.treatedPriorDist.GetMean(),
                                  posterior.treatedPriorDist.GetVariance());
                Console.WriteLine("probControl: {0} +/- {1}",
                                  posterior.controlPriorDist.GetMean(),
                                  posterior.controlPriorDist.GetVariance());
            }
            else
            {
                Console.WriteLine("No Effect Model probability: {0} +/- {1}",
                                  1 - posterior.modelPriorDist.GetMean(),
                                  posterior.modelPriorDist.GetVariance());
                Console.WriteLine("probRecovery: {0} +/- {1}",
                                  posterior.recoverPriorDist.GetMean(),
                                  posterior.recoverPriorDist.GetVariance());
            }

        }
    }
}
