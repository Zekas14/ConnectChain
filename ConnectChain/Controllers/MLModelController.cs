using ConnectChain.MLModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConnectChain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MLModelController : ControllerBase
    {
        [HttpPost("Train")]
        public IActionResult TrainModel()
        {
            try
            {
                var dataPath = "MLModels/SupplierTrainingData.csv";
                var modelPath = "MLModels/MatchingModel.zip";

                var trainer = new SupplierMatchingModelTrainer();
                trainer.TrainAndSaveModel(dataPath, modelPath);

                return Ok("✅ Model trained and saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"❌ Error training model: {ex.Message}");
            }
        }
    }
}
