using Microsoft.ML;

namespace ConnectChain.MLModels
{
    public class SupplierMatchingModelTrainer
    {
        public void TrainAndSaveModel(string dataPath, string modelPath)
        {
            var context = new MLContext();

            var data = context.Data.LoadFromTextFile<SupplierData>(dataPath, hasHeader: true, separatorChar: ',');

            var pipeline = context.Transforms.Categorical.OneHotEncoding("BusinessType")
                .Append(context.Transforms.Categorical.OneHotEncoding("Address"))
                .Append(context.Transforms.Categorical.OneHotEncoding("Category"))
                .Append(context.Transforms.Concatenate("Features", "BusinessType", "Address", "Rating", "Category"))
                .Append(context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Fits", featureColumnName: "Features"));

            var model = pipeline.Fit(data);
            context.Model.Save(model, data.Schema, modelPath);
        }
    }

}
