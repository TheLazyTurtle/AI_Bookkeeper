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
        mlContext.Model.Load("MLModel2.zip", out var model);
        
        var trainingData = TransactionManager.GetTransactionsWithCategory().ToList();
        var adaptedTrainingData = trainingData.Select(TransactionMlModelAdapter.Adapt);
        var view = mlContext.Data.LoadFromEnumerable(adaptedTrainingData);
        var pipeline = MLModel2.RetrainPipeline(mlContext, view);
        
        mlContext.Model.Save(pipeline, view.Schema, "MLModel2.zip");
    }
    
    public static Category Predict(Transaction transaction)
    {
        var adaptedTransaction = TransactionMlModelAdapter.Adapt(transaction);
        var predictionOutput = MLModel2.Predict(adaptedTransaction);
        
        var categoryId = predictionOutput.PredictedLabel;

        if (transaction.Category != null)
        {
        }
        
        return CategoryManager.Categories.First(c => c.Id == categoryId);
    }
}