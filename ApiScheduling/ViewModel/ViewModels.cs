using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiScheduling.ViewModel
{
    public class RosterFDPDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<RosterFDPId> ids { get; set; }
        public int crewId { get; set; }
        public string rank { get; set; }
        public int index { get; set; }
        public List<string> flights { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public int homeBase { get; set; }
        public string flts { get; set; }
        public string route { get; set; }
        public string key { get; set; }
        public string group { get; set; }
        public string scheduleName { get; set; }
        public string no { get; set; }
        public int? extension { get; set; }
        public decimal? maxFDP { get; set; }

        public bool split { get; set; }

        public bool? IsSplitDuty { get; set; }
        public int? SplitValue { get; set; }

        public int? IsAdmin { get; set; }

        public int? DeletedFDPId { get; set; }

        public List<RosterFDPDtoItem> items { get; set; }

        public double getDuty()
        {
            return (this.items.Last().sta.AddMinutes(30) - this.items.First().std.AddMinutes(-60)).TotalMinutes;
        }
        public double getFlight()
        {
            double flt = 0;
            foreach (var x in this.items)
                flt += (x.sta - x.std).TotalMinutes;
            return flt;
        }
        public static List<RosterFDPDtoItem> getItems(List<string> flts)
        {
            List<RosterFDPDtoItem> result = new List<RosterFDPDtoItem>();
            foreach (var x in flts)
            {
                var parts = x.Split('_');
                var item = new RosterFDPDtoItem();
                item.flightId = Convert.ToInt32(parts[0]);
                item.dh = Convert.ToInt32(parts[1]);
                var stdStr = parts[2];
                var staStr = parts[3];
                item.std = new DateTime(Convert.ToInt32(stdStr.Substring(0, 4)), Convert.ToInt32(stdStr.Substring(4, 2)), Convert.ToInt32(stdStr.Substring(6, 2))
                    , Convert.ToInt32(stdStr.Substring(8, 2))
                    , Convert.ToInt32(stdStr.Substring(10, 2))
                    , 0
                    ).ToUniversalTime();
                item.sta = new DateTime(Convert.ToInt32(staStr.Substring(0, 4)), Convert.ToInt32(staStr.Substring(4, 2)), Convert.ToInt32(staStr.Substring(6, 2))
                   , Convert.ToInt32(staStr.Substring(8, 2))
                   , Convert.ToInt32(staStr.Substring(10, 2))
                   , 0
                   ).ToUniversalTime();
                item.no = parts[4];
                item.from = parts[5];
                item.to = parts[6];

                result.Add(item);
            }

            return result;
        }

        public static int getRank(string rank)
        {
            if (rank.StartsWith("IP"))
                return 12000;
            if (rank.StartsWith("P1"))
                return 1160;
            if (rank.StartsWith("P2"))
                return 1161;
            if (rank.ToUpper().StartsWith("SAFETY"))
                return 1162;
            if (rank.ToUpper().StartsWith("FE"))
                return 1165;
            if (rank.StartsWith("ISCCM"))
                return 10002;
            if (rank.StartsWith("SCCM"))
                return 1157;
            if (rank.StartsWith("CCM"))
                return 1158;
            if (rank.StartsWith("OBS"))
                return 1153;
            if (rank.StartsWith("CHECK"))
                return 1154;
            if (rank.StartsWith("00103"))
                return 12001;
            if (rank.StartsWith("004"))
                return 12002;
            if (rank.StartsWith("005"))
                return 12003;

            return -1;

        }
        public static string getRankStr(int rank)
        {
            if (rank == 12000)
                return "IP";
            if (rank == 1160)
                return "P1";
            if (rank == 1161)
                return "P2";
            if (rank == 1162)
                return "SAFETY";
            if (rank == 10002)
                return "ISCCM";
            if (rank == 1157)
                return "SCCM";
            if (rank == 1158)
                return "CCM";
            if (rank == 1153)
                return "OBS";
            if (rank == 1154)
                return "CHECK";
            return "";
        }


    }



    public class RosterFDPId
    {
        public int id { get; set; }
        public int dh { get; set; }
    }

    public class RosterFDPDtoItem
    {
        public int flightId { get; set; }
        public int dh { get; set; }
        public DateTime std { get; set; }
        public DateTime sta { get; set; }
        public int index { get; set; }
        public int rankId { get; set; }
        public string no { get; set; }
        public string from { get; set; }
        public string to { get; set; }




    }

}