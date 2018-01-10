# InferNET-ModelSelection
A simple model showing how model selection works using Bayes factor. Two models are competing at explaining the randomized trials data:
1. There is treatment effect
2. There is no treatment effect

The model works because the Bayesian approach automatically favors the least complicated model if the data supports it. In other words, the Bayesian model does not overfit to the "has treatment" model in all cases since it can also explain "no treatment" data but with more parameters.
