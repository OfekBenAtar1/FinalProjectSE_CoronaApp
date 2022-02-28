using System;

namespace CoronaProgram.Models
{
    public class LabTestResult
    {
        public string labID { get; set; }
        public string testID { get; set; }
        public string patientID { get; set; }
        public DateTime testDate { get; set; }
        public bool isCovidPositive { get; set; }

    }
}