using LinqToExcel;
using LinqToExcel.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ApiFDM.Models
{

    public class Boeing
    {
        public string FlightPhase { get; set; }
        public string Severity { get; set; }
        public string EventName { get; set; }
        public string Value { get; set; }
        public string Minor { get; set; }
        public string Major { get; set; }
        public string Critical { get; set; }
        public string Duration { get; set; }
        public string Aircraft { get; set; }
        public string TOAirport { get; set; }
        public string TDAirport { get; set; }
        public string RecdFltNum { get; set; }
        public string Date { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string IP { get; set; }
        public string StateName { get; set; }
        public string Context { get; set; }
        public string TORunway { get; set; }
        public string TDRunway { get; set; }
        public string TODatetime { get; set; }
        public string TDDatetime { get; set; }
        public string Type { get; set; }
        public string Units { get; set; }
        public string ValueName { get; set; }
        public string EnginePos { get; set; }
        public int RegisterId { get; set; }
        public string FileName { get; set; }
        public int recordNum { get; set; }


        public string Reg
        {
            get
            {
                string aircraft = Aircraft == null ? null : Aircraft.Skip(Aircraft.IndexOf(("EP-"))).ToString();
                return aircraft;
            }
        }

        public float? ValueX
        {
            get
            {
                float? result = null;
                if (float.TryParse(Value, out float x))
                    result = float.Parse(Value);
                else
                    result = null;

                return result;
            }
        }

        public float? MinorX
        {
            get
            {
                float? result = null;
                if (float.TryParse(Minor, out float x))
                    result = float.Parse(Minor);
                else
                    result = null;

                return result;
            }
        }

        public float? MajorX
        {
            get
            {
                float? result = null;
                if (float.TryParse(Major, out float x))
                    result = float.Parse(Major);
                else
                    result = null;

                return result;
            }
        }
        public float? CriticalX
        {
            get
            {
                float? result = null;
                if (float.TryParse(Critical, out float x))
                    result = float.Parse(Critical);
                else
                    result = null;

                return result;
            }
        }

        public float? DurationX
        {
            get
            {
                float? result = 0;

                if (float.TryParse(Duration, out float x))
                {
                    result = float.Parse(Duration);
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public DateTime? DateX
        {
            get
            {
                DateTime? result = new DateTime();

                if (DateTime.TryParse(TODatetime, out DateTime y))
                {
                    result = y;
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public string SeverityX
        {
            get
            {

                string result = null;
                if (Severity.ToUpper().StartsWith("H"))
                    result = "High";
                else if (Severity.ToUpper().StartsWith("M"))
                    result = "Medium";
                else if (Severity.ToUpper().StartsWith("L"))
                    result = "Low";

                return result;
            }
        }



        public string FlightNumber
        {
            get
            {
                string result = null;
                var B737FLTNO = (RecdFltNum == null) ? (int?)null : RecdFltNum.Length;
                if (B737FLTNO == 2)
                    result = "00" + RecdFltNum;
                else
                    result = RecdFltNum;
                return result;
            }
        }
        public bool IsValid
        {
            get
            {
                if (this.DateX == null || this.EventName == null || this.SeverityX == null)
                    return false;
                return true;
            }
        }

    }

    public class MD
    {
        public string ValueName { get; set; }
        public string LimitLevel { get; set; }
        public string description { get; set; }
        public string LevelsValue { get; set; }
        public string limit { get; set; }
        public string Date { get; set; }

        public string FlightNo { get; set; }
        public string Reg { get; set; }
        public string From { get; set; }
        public string To { get; set; }

        public string P1 { get; set; }

        public string P2 { get; set; }

        public string IP { get; set; }

        public string PFLR { get; set; }
        public string Duration { get; set; }
        public int RegisterId { get; set; }
        public string FileName { get; set; }
        public int recordNum { get; set; }

        public float? ValueX
        {
            get
            {
                float? result = null;
                if (float.TryParse(LevelsValue, out float x))
                    result = float.Parse(LevelsValue);
                else
                    result = null;

                return result;
            }
        }

        public float? DurationX
        {
            get
            {
                float? result = 0;

                if (float.TryParse(Duration, out float flt))
                {
                    result = float.Parse(Duration);
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }

        public DateTime? DateX
        {
            get
            {
                DateTime? result = new DateTime();


                //   var _date = Date.Length == 9 ? "20" + Date : Date;

                if (DateTime.TryParse(Date, out DateTime y))
                {
                    result = y;
                }
                else
                {
                    result = null;
                }
                return result;
            }
        }



        public string FlightNumber
        {
            get
            {
                string result = null;
                var B737FLTNO = (FlightNo == null) ? (int?)null : FlightNo.Length;
                if (B737FLTNO == 2)
                    result = "00" + FlightNo;
                else
                    result = FlightNo;
                return result;
            }
        }


        public bool IsValid
        {
            get
            {
                if (this.DateX == null || this.description == null || this.LimitLevelX == null)
                    return false;
                return true;
            }
        }


        public string LimitLevelX
        {
            get
            {

                string result = null;
                if (LimitLevel.ToUpper().StartsWith("H"))
                    result = "High";
                else if (LimitLevel.ToUpper().StartsWith("M"))
                    result = "Medium";
                else if (LimitLevel.ToUpper().StartsWith("L"))
                    result = "Low";

                return result;

                return result;

            }
        }
    }

    public class FailedItmes
    {
        public string flightNo { get; set; }
        public string Severity { get; set; }
        public DateTime? Date { get; set; }
        public string EventName { get; set; }
        public string P1 { get; set; }
        public string P2 { get; set; }
        public string FileName { get; set; }
        public float? Value { get; set; }
        public int? Status { get; set; }
        public float? Duration { get; set; }
        public string Message { get; set; }
    }


}
