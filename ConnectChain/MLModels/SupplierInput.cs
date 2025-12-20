using Microsoft.ML.Data;

namespace ConnectChain.MLModels
{
    public class SupplierInput
    {
        [LoadColumn(0)]
        public string BusinessType { get; set; }

        [LoadColumn(1)]
        public string Address { get; set; }

        [LoadColumn(2)]
        public float Rating { get; set; }

        [LoadColumn(3)]
        public string Category { get; set; }

        [LoadColumn(4)]
        public bool Fits { get; set; }
    }

}
