using Microsoft.ML.Data;

namespace ConnectChain.MLModels
{
    public class SupplierPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Fits;
        public float Probability;
    }

}
