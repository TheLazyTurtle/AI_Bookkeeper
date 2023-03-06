using DataManager.Adapters;
using DataManager.Models;
using Microsoft.ML;
using ML_algo;

namespace DataManager.Services;

public static class PredictionManager
{
    public static void Retrain()
    {
        var mlContext = new MLContext();
        if (File.Exists(MLModel2.MLNetModelPath))
            mlContext.Model.Load(MLModel2.MLNetModelPath, out var model);
        
        var trainingData = TransactionManager.GetTransactionsWithCategory().ToList();
        var adaptedTrainingData = trainingData.Select(TransactionMlModelAdapter.Adapt);
        var view = mlContext.Data.LoadFromEnumerable(adaptedTrainingData);
        var pipeline = MLModel2.RetrainPipeline(mlContext, view);
        
        mlContext.Model.Save(pipeline, view.Schema, MLModel2.MLNetModelPath);
    }
    
    public static Category Predict(Transaction transaction)
    {
        if (!File.Exists(MLModel2.MLNetModelPath))
            Retrain();
            
        var adaptedTransaction = TransactionMlModelAdapter.Adapt(transaction);
        var predictionOutput = MLModel2.Predict(adaptedTransaction);
        
        var categoryId = predictionOutput.PredictedLabel;
        
        return CategoryManager.Categories.First(c => c.Id == categoryId);
    }
}