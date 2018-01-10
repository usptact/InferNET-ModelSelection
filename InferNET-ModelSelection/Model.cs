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
        public InferenceEngine inferenceEngine;

        Variable<Bernoulli> probModelPrior;
        Variable<Beta> probTreatedPrior;
        Variable<Beta> probControlPrior;
        Variable<Beta> probRecoveryPrior;

        Variable<bool> probModel;
        Variable<double> probTreated;
        Variable<double> probControl;
        Variable<double> probRecovery;

        VariableArray<bool> treated;
        VariableArray<bool> control;

        Variable<int> numItems;

        public void CreateModel()
        {
            probModelPrior = Variable.New<Bernoulli>();

            probTreatedPrior = Variable.New<Beta>();
            probControlPrior = Variable.New<Beta>();
            probRecoveryPrior = Variable.New<Beta>();

            probModel = Variable.Random<bool, Bernoulli>(probModelPrior);

            probTreated = Variable.Random<double, Beta>(probTreatedPrior);
            probControl = Variable.Random<double, Beta>(probControlPrior);
            probRecovery = Variable.Random<double, Beta>(probRecoveryPrior);

            numItems = Variable.New<int>();
            Range items = new Range(numItems);

            treated = Variable.Array<bool>(items);
            control = Variable.Array<bool>(items);

            using (Variable.If(probModel))
            {
                treated[items] = Variable.Bernoulli(probTreated).ForEach(items);
                control[items] = Variable.Bernoulli(probControl).ForEach(items);
            }
            using (Variable.IfNot(probModel))
            {
                treated[items] = Variable.Bernoulli(probRecovery).ForEach(items);
                control[items] = Variable.Bernoulli(probRecovery).ForEach(items);
            }

            if (inferenceEngine == null)
                inferenceEngine = new InferenceEngine();
        }

        public void SetModelData(ModelData modelData)
        {
            probModelPrior.ObservedValue = modelData.modelPriorDist;

            probTreatedPrior.ObservedValue = modelData.treatedPriorDist;
            probControlPrior.ObservedValue = modelData.controlPriorDist;
            probRecoveryPrior.ObservedValue = modelData.recoverPriorDist;
        }

        public ModelData InferModelData(bool[] treatedData, bool[] controlData)
        {
            ModelData posteriors = new ModelData();

            numItems.ObservedValue = treatedData.Length;
            treated.ObservedValue = treatedData;
            control.ObservedValue = controlData;

            posteriors.modelPriorDist = inferenceEngine.Infer<Bernoulli>(probModel);
            posteriors.treatedPriorDist = inferenceEngine.Infer<Beta>(probTreated);
            posteriors.controlPriorDist = inferenceEngine.Infer<Beta>(probControl);
            posteriors.recoverPriorDist = inferenceEngine.Infer<Beta>(probRecovery);

            return posteriors;
        }

        // generate has-effect synthetic data
        public static void GenerateHasEffectData(int n, double probTreated, double probControl,
                                        out bool[] treatedData, out bool[] controlData)
        {
            treatedData = new bool[n];
            controlData = new bool[n];

            Bernoulli treated = new Bernoulli(probTreated);
            Bernoulli control = new Bernoulli(probControl);

            for (int i = 0; i < n; i++)
            {
                treatedData[i] = treated.Sample();
                controlData[i] = control.Sample();
            }
        }

        // generate no-effect synthetic data
        public static void GenerateNoEffectData(int n, double probRecovery,
                                                out bool[] treatedData, out bool[] controlData)
        {
            treatedData = new bool[n];
            controlData = new bool[n];

            Bernoulli recovery = new Bernoulli(probRecovery);

            for (int i = 0; i < n; i++)
            {
                treatedData[i] = recovery.Sample();
                controlData[i] = recovery.Sample();
            }
        }

    }
}
