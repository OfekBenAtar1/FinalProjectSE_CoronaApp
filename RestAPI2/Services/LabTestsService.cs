using CoronaProgram.Models;
using System;
using System.Collections.Generic;

namespace CoronaProgram
{
    public class LabTestsService : SingletonService<LabTestsService>
    {
        private List<LabTestResult> AllLabTests { get; set; }

        public LabTestsService()
        {
            AllLabTests = new List<LabTestResult>();
        }

        public override void ResetData()
        {
            AllLabTests = new List<LabTestResult>();
        }

        public List<LabTestResult> GetPersonTests(string Id)
        {
            List<LabTestResult> patientTests = new List<LabTestResult>();
            foreach (LabTestResult test in AllLabTests)
            {
                if (test.patientID == Id)
                {
                    patientTests.Add(test);
                }
            }

            return patientTests;
        }

        public LabTestResult AddNewTest(LabTestResult testResult)
        {
            LabTestResult test = getTestByID(testResult.testID);
            if(test != null) // If the test exists
                return null;

            Person p = CovidMgmtApp.GetInstance().Persons.GetPersonById(testResult.patientID);
            if (p == null) // If p doesnt exits
                return null;

            AllLabTests.Add(testResult);

            if (testResult.isCovidPositive == true) // If Test is Positive So Person is Positive
                p.IsCovidPositive = true;
            

            if (p.IsCovidPositive == false) // If new Test Negative and Person already Negative
                return testResult;

            // Last & New TestResult are Negative and Person Is Positive 
            var personTests = GetPersonTests(testResult.patientID);
            if (personTests.Count == 1)  // If Person Doesn't Have Tests
                return testResult;
            if (personTests[personTests.Count - 2].isCovidPositive == false)
            {
                p.IsCovidPositive = false;
            }
            return testResult;
        }

        private LabTestResult getTestByID(string testID)
        {
            foreach (LabTestResult test in AllLabTests)
            {
                if (test.testID == testID)
                    return test;
            }

            return null;
        }

        public bool IsIsolate(String id)
        {
            List<LabTestResult> tests = GetPersonTests(id);
            int negativeTests = 0;

            foreach(var t in tests)
            {
                if ((DateTime.Now - t.testDate).TotalDays <= 14 && t.isCovidPositive == false)
                    negativeTests++;
            }

            return negativeTests < 2;
        }
    }
}
