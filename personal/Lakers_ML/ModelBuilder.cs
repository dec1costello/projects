

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using GamblingML.Model;

namespace GamblingML.ConsoleApp
{
    public static class ModelBuilder
    {
        private static string TRAIN_DATA_FILEPATH = @"C:\Users\decla\Desktop\lakers_stats.tsv";
        private static string MODEL_FILE = ConsumeModel.MLNetModelPath;

        // Create MLContext to be shared across the model creation workflow objects 
        // Set a random seed for repeatable/deterministic results across multiple trainings.
        private static MLContext mlContext = new MLContext(seed: 1);

        public static void CreateModel()
        {
            // Load Data
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: TRAIN_DATA_FILEPATH,
                                            hasHeader: true,
                                            separatorChar: '\t',
                                            allowQuoting: true,
                                            allowSparse: false);

            // Build training pipeline
            IEstimator<ITransformer> trainingPipeline = BuildTrainingPipeline(mlContext);

            // Train Model
            ITransformer mlModel = TrainModel(mlContext, trainingDataView, trainingPipeline);

            // Evaluate quality of Model
            Evaluate(mlContext, trainingDataView, trainingPipeline);

            // Save model
            SaveModel(mlContext, mlModel, MODEL_FILE, trainingDataView.Schema);
        }

        public static IEstimator<ITransformer> BuildTrainingPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.IndicateMissingValues(new[] { new InputOutputColumnPair("Date_MissingIndicator", "Date"), new InputOutputColumnPair("Opp_MissingIndicator", "Opp") })
                                      .Append(mlContext.Transforms.Conversion.ConvertType(new[] { new InputOutputColumnPair("Date_MissingIndicator", "Date_MissingIndicator"), new InputOutputColumnPair("Opp_MissingIndicator", "Opp_MissingIndicator") }))
                                      .Append(mlContext.Transforms.ReplaceMissingValues(new[] { new InputOutputColumnPair("Date", "Date"), new InputOutputColumnPair("Opp", "Opp") }))
                                      .Append(mlContext.Transforms.Concatenate("Features", new[] { "Date_MissingIndicator", "Opp_MissingIndicator", "Date", "Opp", "Rk", "G", "W/L", "Tm", "Opp2", "FG", "FGA", "FG%", "3P", "3PA", "3P%", "FT", "FTA", "FT%", "ORB", "TRB", "AST", "STL", "BLK", "TOV", "PF", "OFG", "OFGA", "O3P", "O3PA", "O3P%", "OFT", "OFTA", "OFT%", "OORB", "OTRB", "OAST", "OSTL", "OBLK", "OTOV", "OPF" }));
            // Set the training algorithm 
            var trainer = mlContext.Regression.Trainers.FastForest(labelColumnName: @"OFG%", featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(trainer);

            return trainingPipeline;
        }

        public static ITransformer TrainModel(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            Console.WriteLine("=============== Training  model ===============");

            ITransformer model = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine("=============== End of training process ===============");
            return model;
        }

        private static void Evaluate(MLContext mlContext, IDataView trainingDataView, IEstimator<ITransformer> trainingPipeline)
        {
            // Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics
            Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            var crossValidationResults = mlContext.Regression.CrossValidate(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "OFG%");
            PrintRegressionFoldsAverageMetrics(crossValidationResults);
        }

        private static void SaveModel(MLContext mlContext, ITransformer mlModel, string modelRelativePath, DataViewSchema modelInputSchema)
        {
            // Save/persist the trained model to a .ZIP file
            Console.WriteLine($"=============== Saving the model  ===============");
            mlContext.Model.Save(mlModel, modelInputSchema, GetAbsolutePath(modelRelativePath));
            Console.WriteLine("The model is saved to {0}", GetAbsolutePath(modelRelativePath));
        }

        public static string GetAbsolutePath(string relativePath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }

        public static void PrintRegressionMetrics(RegressionMetrics metrics)
        {
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Metrics for Regression model      ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       LossFn:        {metrics.LossFunction:0.##}");
            Console.WriteLine($"*       R2 Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Absolute loss: {metrics.MeanAbsoluteError:#.##}");
            Console.WriteLine($"*       Squared loss:  {metrics.MeanSquaredError:#.##}");
            Console.WriteLine($"*       RMS loss:      {metrics.RootMeanSquaredError:#.##}");
            Console.WriteLine($"*************************************************");
        }

        public static void PrintRegressionFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
            var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
            var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
            var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
            var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Regression model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average L1 Loss:       {L1.Average():0.###} ");
            Console.WriteLine($"*       Average L2 Loss:       {L2.Average():0.###}  ");
            Console.WriteLine($"*       Average RMS:           {RMS.Average():0.###}  ");
            Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
            Console.WriteLine($"*       Average R-squared:     {R2.Average():0.###}  ");
            Console.WriteLine($"*************************************************************************************************************");
        }
    }
}
