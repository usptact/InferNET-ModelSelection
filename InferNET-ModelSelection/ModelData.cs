using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrosoftResearch.Infer;
using MicrosoftResearch.Infer.Distributions;

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
