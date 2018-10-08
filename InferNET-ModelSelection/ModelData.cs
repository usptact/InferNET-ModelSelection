using Microsoft.ML.Probabilistic.Distributions;

namespace InferNET_ModelSelection
{
    public struct ModelData
    {
        public Bernoulli modelPriorDist;
        public Beta treatedPriorDist;
        public Beta controlPriorDist;
        public Beta recoverPriorDist;
    }
}
