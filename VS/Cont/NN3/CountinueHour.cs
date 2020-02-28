using System;
using System.Collections.Generic;
using TSLab.Script;
using TSLab.Script.Handlers;
using TSLab.DataSource;
using TSLab.Script.Helpers;

namespace Hour
{

    [HandlerCategory("NN")]
    [HandlerName("ContinueHour")]

    public class ContinueHourClass : IOneSourceHandler, ISecurityInputs, ISecurityReturns, IStreamHandler, IContextUses
    {

        public IContext Context { set; private get; }

        public ISecurity Execute(ISecurity sec)
        {

            var ValCount = sec.Bars.Count;
            int prevhour = 0;
            IList<Bar> NewBars = new List<Bar>();
            
            foreach(var bar in sec.Bars)
            {
                
                if(bar.Date.Hour - prevhour  > 1 && prevhour > 0)
                {
                    for (int i = 1; i < bar.Date.Hour - prevhour; i++)
                    {
                        Bar newbar = new Bar(bar.Color, bar.Date.AddHours(1), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume);
                        NewBars.Add(newbar);
                    }
                }
                NewBars.Add(bar);
                prevhour = bar.Date.Hour;
            }

            ISecurity newsec = sec.CloneAndReplaceBars(NewBars);
            return newsec;
        }

    }
}
