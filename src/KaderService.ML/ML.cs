using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.ML;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace KaderService.ML
{
    public class ML
    {
        private const string TrainingDataLocation = @"D:\Compare.txt";
        public static async Task<Dictionary<int, double>> Run(Request request)
        {
            CreateFile(request.ItemCustomersList);
            var mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromTextFile(path: TrainingDataLocation,
                                                      new[]
                                                                {
                                                                    new TextLoader.Column("Label", DataKind.Single, 0),
                                                                    new TextLoader.Column(nameof(ProductEntry.UserNumber), DataKind.UInt32, new [] { new TextLoader.Range(0) }, new KeyCount(10000)),
                                                                    new TextLoader.Column(nameof(ProductEntry.RelatedPostId), DataKind.UInt32, new [] { new TextLoader.Range(1) }, new KeyCount(10000))
                                                                },
                                                      hasHeader: true);

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = nameof(ProductEntry.UserNumber),
                MatrixRowIndexColumnName = nameof(ProductEntry.RelatedPostId),
                LabelColumnName = "Label",
                LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass,
                Alpha = 0.01,
                Lambda = 0.025
            };

            MatrixFactorizationTrainer est = mlContext.Recommendation().Trainers.MatrixFactorization(options);
            ITransformer model = est.Fit(dataView);
            PredictionEngine<ProductEntry, PredictionScore> predictionEngine = mlContext.Model.CreatePredictionEngine<ProductEntry, PredictionScore>(model);
            var scores = new Dictionary<int, double>();

            foreach (int itemId in request.AllItemsIds)
            {
                var entry = new ProductEntry
                {
                    UserNumber = (uint)request.CustomerNumber.GetHashCode(),
                    RelatedPostId = (uint)itemId.GetHashCode()
                };

                PredictionScore predictionScore = predictionEngine.Predict(entry);
                double finalScore = Math.Round(predictionScore.Score, 3);
                scores.Add(itemId, finalScore);
            }

            scores = scores.ToDictionary(pair => pair.Key, pair => pair.Value);

            return scores;
        }

        private static void CreateFile(IEnumerable<ItemsCustomers> customersItems)
        {
            File.Delete(@"D:\Compare.txt");
            File.AppendAllText(@"D:\Compare.txt", $"ProductID	ProductID_Copurchased{Environment.NewLine}");

            foreach (var customerItem in customersItems)
            {
                File.AppendAllText(@"D:\Compare.txt", $"{customerItem.CustomerNumber}	{customerItem.PostId}{Environment.NewLine}");
            }
        }

        //public static string GetAbsolutePath(string relativeDatasetPath)
        //{
        //    FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
        //    string assemblyFolderPath = _dataRoot.Directory.FullName;

        //    string fullPath = Path.Combine(assemblyFolderPath, relativeDatasetPath);

        //    return fullPath;
        //}
    }
}
