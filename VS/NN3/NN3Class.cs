using System;
using System.Collections.Generic;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using NNLib;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.DataSource;
using TSLab.Script.Helpers;
using System.IO;

// Инструменты
// BR
// Eu
// GAZR 
// GMKR
// GOLD
// LKOH
// MGNT
// MIX
// ROSN
// RTS
// Si


namespace NN3
{

    [HandlerCategory("NN")]
    [HandlerName("NN PairTrade")]
    [InputsCount(2, 200)]
    [InputInfo(0, "Инструмент")]

    public class NN3Class : ITwoSourcesHandler, IDoubleReturns, IStreamHandler, IContextUses, IValuesHandlerWithNumber
    {

        [HandlerParameter(Name = "Отладка", Default = "false", NotOptimized = true)]
        public bool Debug { get; set; }

        public IContext Context { set; private get; }

        public IList<double> Execute(params IList<double>[] secArr)
        {
            
            var Filename = "c:\\Temp\\" + Guid.NewGuid().ToString() + ".txt";
            if (Debug == true)
            {
                
            }

            var ValCount = secArr[0].Count;
            var SymbolsCount  = 11;
            var values = new double[ValCount];
            NNClass NNMatlab = new NNClass();
            MWNumericArray NNres;
            MWNumericArray x2 = new double[SymbolsCount];
            double NNtotal;

            for (var i = 0; i < ValCount; i++)
            {
                MWArray result;
                for (var j = 0; j < SymbolsCount; j++)
                {
                    int LocalValCount = secArr[j].Count; //Если есть пропуски то берём последнее
                    if (i >= LocalValCount)
                    {
                        x2[j + 1] = secArr[j][LocalValCount-1];
                    }
                    else
                    {
                        x2[j + 1] = secArr[j][i];
                    }
                }
                //x2[7] = 0; //!!!!!!!! ЗАГРУШКА!!!!!! 
                result = NNMatlab.NNFunc(x2);
                NNres = (MWNumericArray)result[1];
                NNtotal = (double)NNres[1];
                values[i] = NNtotal;

                if (Debug)
                {
                    StreamWriter outputFile = File.AppendText(Filename); ;
                    string TextToWrite = "";
                    for (var j = 0; j < SymbolsCount; j++)
                    {
                        TextToWrite = TextToWrite + ", " + x2[j + 1].ToString();
                    }
                    TextToWrite = TextToWrite + ", " + NNtotal.ToString();
                    outputFile.WriteLine(TextToWrite);
                    outputFile.Close();
                }

            }

          
            return values;
        }

    }
}
