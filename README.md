# InferNET-ModelSelection
A simple model showing how model selection works using Bayes factor. Two models are competing at explaining the randomized trials data:
1. There is treatment effect (the "has effect" model)
2. There is no treatment effect (the "no effect" model)

The data are binary trials outcomes: a patient has recovered or not.

The "has treatment" model separates the group that has received a treatment from a group that hasn't. The Bernoulli posterior distributions are different and are inferred.

The "no treatment" model assumes that there are two groups but have the same underlying recovery rate. The Bernoulli posterior distribution is inferred.

One can reasonably argue that the "has effect" model can also explain the data that has been actually generated by the "no effect" data. The Bayesian approach automatically selects the correct least complicated model. If the models are faithful at describing the generative process, Bayes inference will correctly infer it.

This is a nice demo on why Bayesian methods do not overfit.
