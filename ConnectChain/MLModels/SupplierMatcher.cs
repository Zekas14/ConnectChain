using Microsoft.ML;

namespace ConnectChain.MLModels
{

    public class SupplierMatcher
    {
        private readonly PredictionEngine<SupplierInput, SupplierPrediction> _engine;

        public SupplierMatcher(string modelPath)
        {
            var mlContext = new MLContext();
            var model = mlContext.Model.Load(modelPath, out _);
            _engine = mlContext.Model.CreatePredictionEngine<SupplierInput, SupplierPrediction>(model);
        }

        public bool IsMatch(SupplierInput input)
        {
            var prediction = _engine.Predict(input);
            return prediction.Fits && prediction.Probability >= 0.7;
        }
    }

}
