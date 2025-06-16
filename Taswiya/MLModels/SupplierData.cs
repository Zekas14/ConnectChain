using Microsoft.ML.Data;

namespace ConnectChain.MLModels
{
    public class SupplierData
    {
        [LoadColumn(0)]
        public string BusinessType;
        [LoadColumn(1)]
        public string Address;
        [LoadColumn(2)]
        public float Rating;
        [LoadColumn(3)]
        public string Category;
        [LoadColumn(4)]
        public bool Fits;
    }

}
