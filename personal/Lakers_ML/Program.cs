

using System;
using GamblingML.Model;

namespace GamblingML.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData = new ModelInput()
            {
                Rk = 1F,
                G = 1F,
                Date = Single.NaN,
                Opp = Single.NaN,
                W_L = 109F,
                Tm = 116F,
                Opp2 = 38F,
                FG = 81F,
                FGA = 0.469F,
                FG_ = 9F,
                _3P = 29F,
                _3PA = 0.31F,
                _3P_ = 24F,
                FT = 31F,
                FTA = 0.774F,
                FT_ = 8F,
                ORB = 45F,
                TRB = 22F,
                AST = 4F,
                STL = 2F,
                BLK = 19F,
                TOV = 20F,
                PF = 44F,
                OFG = 0.473F,
                OFGA = 14F,
                O3P = 0.35F,
                O3PA = 14F,
                O3P_ = 19F,
                OFT = 0.737F,
                OFTA = 11F,
                OFT_ = 40F,
                OORB = 22F,
                OTRB = 10F,
                OAST = 3F,
                OSTL = 16F,
                OBLK = 29F,
                OTOV = 0F,
                OPF = 0F,
            };

            // Make a single prediction on the sample data and print results
            var predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual OFG_ with predicted OFG_ from sample data...\n\n");
            Console.WriteLine($"Rk: {sampleData.Rk}");
            Console.WriteLine($"G: {sampleData.G}");
            Console.WriteLine($"Date: {sampleData.Date}");
            Console.WriteLine($"Opp: {sampleData.Opp}");
            Console.WriteLine($"W_L: {sampleData.W_L}");
            Console.WriteLine($"Tm: {sampleData.Tm}");
            Console.WriteLine($"Opp2: {sampleData.Opp2}");
            Console.WriteLine($"FG: {sampleData.FG}");
            Console.WriteLine($"FGA: {sampleData.FGA}");
            Console.WriteLine($"FG_: {sampleData.FG_}");
            Console.WriteLine($"_3P: {sampleData._3P}");
            Console.WriteLine($"_3PA: {sampleData._3PA}");
            Console.WriteLine($"_3P_: {sampleData._3P_}");
            Console.WriteLine($"FT: {sampleData.FT}");
            Console.WriteLine($"FTA: {sampleData.FTA}");
            Console.WriteLine($"FT_: {sampleData.FT_}");
            Console.WriteLine($"ORB: {sampleData.ORB}");
            Console.WriteLine($"TRB: {sampleData.TRB}");
            Console.WriteLine($"AST: {sampleData.AST}");
            Console.WriteLine($"STL: {sampleData.STL}");
            Console.WriteLine($"BLK: {sampleData.BLK}");
            Console.WriteLine($"TOV: {sampleData.TOV}");
            Console.WriteLine($"PF: {sampleData.PF}");
            Console.WriteLine($"OFG: {sampleData.OFG}");
            Console.WriteLine($"OFGA: {sampleData.OFGA}");
            Console.WriteLine($"O3P: {sampleData.O3P}");
            Console.WriteLine($"O3PA: {sampleData.O3PA}");
            Console.WriteLine($"O3P_: {sampleData.O3P_}");
            Console.WriteLine($"OFT: {sampleData.OFT}");
            Console.WriteLine($"OFTA: {sampleData.OFTA}");
            Console.WriteLine($"OFT_: {sampleData.OFT_}");
            Console.WriteLine($"OORB: {sampleData.OORB}");
            Console.WriteLine($"OTRB: {sampleData.OTRB}");
            Console.WriteLine($"OAST: {sampleData.OAST}");
            Console.WriteLine($"OSTL: {sampleData.OSTL}");
            Console.WriteLine($"OBLK: {sampleData.OBLK}");
            Console.WriteLine($"OTOV: {sampleData.OTOV}");
            Console.WriteLine($"OPF: {sampleData.OPF}");
            Console.WriteLine($"\n\nPredicted OFG_: {predictionResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();
        }
    }
}
