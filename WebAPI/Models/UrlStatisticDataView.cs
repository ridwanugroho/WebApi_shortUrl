using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class UrlStatisticDataView
    {
        private List<UrlStatistic> urls;

        public UrlStatisticDataView(List<UrlStatistic> urls)
        {
            this.urls = urls;
            generateClickedTracker(urls);
            ByDate = generateDataByDate(urls);
            ByMonth = generateDataByMonth(urls);
            ByYear = generateDataByYear(urls);
        }

        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public List<Track> Clicked {get; set;}

        public ByTime ByDate { get; set; }
        public ByTime ByMonth { get; set; }
        public ByTime ByYear { get; set; }

        private void generateClickedTracker(List<UrlStatistic> urls)
        {
            var rec = from x in urls
                        group x by x.IpAddress into g
                        let count = g.Count()
                        orderby count descending
                        select new Track{ IP = g.Key, Count = count };

            Clicked = rec.ToList();
        }

        private ByTime generateDataByDate(List<UrlStatistic> urls)
        {
            var rec = from x in urls
                        group x by x.CreatedAt.Date into g
                        let count = g.Count()
                        orderby count descending
                        select new { label = g.Key.ToString(), value = count };

            var record = new ByTime();
            record.Label = (from l in rec select l.label).ToList();
            record.Value = (from l in rec select l.value).ToList();

            return record;
        }

        private ByTime generateDataByMonth(List<UrlStatistic> urls)
        {
            var rec = from x in urls
                      group x by x.CreatedAt.Month into g
                      let count = g.Count()
                      orderby count descending
                      select new { label = g.Key.ToString(), value = count };

            var record = new ByTime();
            record.Label = (from l in rec select l.label).ToList();
            record.Value = (from l in rec select l.value).ToList();

            return record;
        }

        private ByTime generateDataByYear(List<UrlStatistic> urls)
        {
            var rec = from x in urls
                      group x by x.CreatedAt.Year into g
                      let count = g.Count()
                      orderby count descending
                      select new { label = g.Key.ToString(), value = count };

            var record = new ByTime();
            record.Label = (from l in rec select l.label).ToList();
            record.Value = (from l in rec select l.value).ToList();

            return record;
        }
    }

    public class Track
    {
        public string IP { get; set; }
        public int Count { get; set; }
    }

    public class ByTime
    {
        public List<string> Label = null;
        public List<int> Value = null;
    }

}
