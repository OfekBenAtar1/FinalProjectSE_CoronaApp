using System;
using Xunit;
using CoronaProgram;
using CoronaProgram.Models;
using System.Collections.Generic;
using System.Linq;

namespace CoronaProgram.Tests
{
    public class UnitTest1 : IDisposable
    {
        public void Dispose()
        {
            CovidMgmtApp.GetInstance().ResetData();
        }

        [Fact]
        public void AddPatient_PatientExit_ReturnNull()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };
            
            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);
            var result = CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            Assert.Null(result);
        }

        [Fact]
        public void AddPatient_PatientNotExist_ReturnNotNull()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            var result = CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            Assert.NotNull(result);
        }

        [Fact]
        public void AddPatient_IsAdded_ReturnPatient()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);
            var result = CovidMgmtApp.GetInstance().Persons.GetPatientByGovId(p.GovtID);

            Assert.Equal(result.GovtID,p.GovtID);
        }

        [Fact] // works
        public void GetPatients_IsAdded_ReturnNumOfPatients()
        {
            Patient p = new Patient
            {
                GovtID = "2346"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatients();

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact]
        public void AddLocation_PatientNotExist_ReturnNull()
        {
            Patient p = new Patient
            {
                PatientID = "11"
            };

            Location l = new Location { DateOfVisit = DateTime.Now.AddDays(-4) };

            var result = CovidMgmtApp.GetInstance().Persons.AddNewLocation(p.PatientID,l);

            Assert.Null(result);
        }

        [Fact]
        public void AddLocation_IncorrectDate_ReturnNull()
        {
            Patient p = new Patient
            {
                PatientID = "432"
            };
            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            Location l = new Location { DateOfVisit = DateTime.Now.AddDays(-8) };
            var result = CovidMgmtApp.GetInstance().Persons.AddNewLocation(p.PatientID, l);

            Assert.Null(result);
        }

        [Fact]
        public void AddLocation_IsLocationExist_ReturnNull()
        {
            Patient p = new Patient
            {
                PatientID = "4326"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);
            var Ddate = DateTime.Now.AddDays(-4);
            Location l1 = new Location { DateOfVisit = Ddate,
                                        SiteAddress = null,
                                        SiteName = "Habonim"};

            Location l2 = new Location
            {
                DateOfVisit = Ddate,
                SiteAddress = null,
                SiteName = "Habonim"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewLocation(p.PatientID, l1);
            var result = CovidMgmtApp.GetInstance().Persons.AddNewLocation(p.PatientID, l2);

            Assert.Null(result);
        }

        [Fact]
        public void GetLocations_PatienDoesntExist_ReturnNull()
        {
            var result = CovidMgmtApp.GetInstance().Persons.GetLocations("1234");

            Assert.Null(result);
        }

        [Fact]
        public void GetLocations_LocationIsAdded_ReturnNumOfLocations()
        {
            Patient p = new Patient
            {
                GovtID = "2346"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            Location l = new Location
            {
                DateOfVisit = DateTime.Now.AddDays(-4),
                SiteAddress = null,
                SiteName = "Habonim"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewLocation(p.PatientID,l);

            var result = CovidMgmtApp.GetInstance().Persons.GetLocations(p.PatientID);

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact]
        public void AddPotentialPatient_PatientNotExist_ReturnNull()
        {
            PotentialPatient p = new PotentialPatient
            {
                FirstName = "Ofir",
                PhoneNumber = "0576"
            };

            var result = CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient("1233",p);
            
            Assert.Null(result);
        }

        [Fact]
        public void AddPotentialPatient_PotentialPatientExist_ReturnNull()
        {
            Patient p = new Patient
            {
                GovtID = "2346"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            PotentialPatient pp = new PotentialPatient
            {
                FirstName = "Ofir",
                PhoneNumber = "0576"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);
            var result = CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);

            Assert.Null(result);
        }

        [Fact]
        public void GetEncounters_PatientNotExist_ReturnNull()
        {
            var result = CovidMgmtApp.GetInstance().Persons.GetEncounters("1154");

            Assert.Null(result);
        }

        [Fact]
        public void GetEncounters_EncounterAdded_ReturnNumOfEncounter()
        {
            Patient p = new Patient
            {
                GovtID = "1123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            PotentialPatient pp = new PotentialPatient
            {
                FirstName = "Ofir",
                PhoneNumber = "0576"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);

            var result = CovidMgmtApp.GetInstance().Persons.GetEncounters(p.PatientID);

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact] // works
        public void GetPatientsSince_IsCovidPositiveFalse_ReturnEmptyList()
        {
            Patient p = new Patient
            {
                GovtID = "115623",
                IsCovidPositive = false
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(-1));

            if (result == null)
                Assert.True(false);

            Assert.Empty(result);
        }

        [Fact]
        public void GetPatientsSince_SinceGreaterThanSignDate_ReturnEmptyList()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(+4));

            if (result == null)
                Assert.True(false);

            Assert.Empty(result);
        }

        [Fact]
        public void GetPatientsSince_IsPatientAdded_ReturnNomOfList()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(-4));

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact]
        public void GetPatientsSince_IsPotentialPatientAdded_ReturnNomOfList()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            PotentialPatient pp = new PotentialPatient
            {
                FirstName = "Ofek",
                PhoneNumber = "436732"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(-4));

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact]
        public void GetPatientsSince_IsSameType_ReturnTypePatient()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(-4));

            if (result == null)
                Assert.True(false);

            Assert.Equal(typeof(Patient), result[0].GetType());
        }

        [Fact] // works
        public void GetPatientsSince_IsSameType_ReturnTypePotentialPatient()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            PotentialPatient pp = new PotentialPatient
            {
                FirstName = "Ofek",
                PhoneNumber = "7532",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);

            var result = CovidMgmtApp.GetInstance().Persons.GetPatientsSince(DateTime.Now.AddDays(-4));

            if (result == null)
                Assert.True(false);

            Assert.Equal(typeof(PotentialPatient), result[1].GetType());
        }

        [Fact] // works
        public void GetPotentialPatients_PotentialPersonAdded_ReturnNumPotentialPatient()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            PotentialPatient pp = new PotentialPatient
            {
                FirstName = "Ofek",
                PhoneNumber = "7532",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(p.PatientID, pp);

            var result = CovidMgmtApp.GetInstance().Persons.GetPotentialPatients();

            if (result == null)
                Assert.True(false);

            Assert.Single(result);
        }

        [Fact]
        public void TransformPotentialPatient_PotentialPatientNotExist_ReturnNull()
        {
            Patient p = new Patient
            {
                GovtID = "1623",
                IsCovidPositive = true
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.TransformPotentialPatient("12154", p);

            Assert.Null(result);
        }

        [Fact]
        public void TransformPotentialPatient_IsPotentialPatientTransformed_ReturnPatientID()
        {
            Patient patient = new Patient
            {
                GovtID = "34443"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(patient);

            PotentialPatient potential = new PotentialPatient
            {
                FirstName = "Ofir"
            };

            potential = CovidMgmtApp.GetInstance().Persons.AddNewPotentialPatient(patient.PatientID, potential);

            var result = CovidMgmtApp.GetInstance().Persons.TransformPotentialPatient(potential.PotentialPatientID, new Patient { GovtID = "20461" });

            Assert.Equal(potential.PotentialPatientID, result.PatientID);
        }

        [Fact]
        public void AddLabTest_LabTestExist_ReturnNull()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            LabTestResult t = new LabTestResult { patientID = p.PatientID,
                                                testID = "122"};

            CovidMgmtApp.GetInstance().LabTests.AddNewTest(t);

            var result = CovidMgmtApp.GetInstance().LabTests.AddNewTest(t);
            
            Assert.Null(result);
        }

        [Fact]
        public void AddLabTest_PersonNotExist_ReturnNull()
        {
            LabTestResult t = new LabTestResult
            {
                patientID = "9876",
                testID = "122"
            };

            var result = CovidMgmtApp.GetInstance().LabTests.AddNewTest(t);

            Assert.Null(result);
        }

        [Fact] // works
        public void AddLabTest_IsPositiveTest_ReturnPersonCovidStatus()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            LabTestResult t = new LabTestResult
            {
                patientID = p.PatientID,
                testID = "1522",
                isCovidPositive = true
            };

            CovidMgmtApp.GetInstance().LabTests.AddNewTest(t);

            Assert.True(p.IsCovidPositive);
        }

        [Fact]
        public void GetPersonDetailsTests_NoTests_ReturnEmptyTestsList()
        {
            var result = CovidMgmtApp.GetInstance().LabTests.GetPersonTests("4561");

            Assert.Empty(result);
        }

        [Fact]
        public void GetPersonDetailsTests_OneTest_ReturnSingleTestsList()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            LabTestResult t = new LabTestResult
            {
                patientID = p.PatientID,
                testID = "1234"
            };

            CovidMgmtApp.GetInstance().LabTests.AddNewTest(t);

            var result = CovidMgmtApp.GetInstance().LabTests.GetPersonTests(p.PatientID);

            Assert.Single(result);
        }

        [Fact] // works
        public void GetIsolated_NoTests_ReturnSingleIsolatersList()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var result = CovidMgmtApp.GetInstance().Persons.GetIsolatedPersons();

            Assert.Single(result);
        }

        [Fact] // works
        public void GetIsolated_IncorrectDate_ReturnEmptyIsolatersList()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            p.SignDate = DateTime.Now.AddDays(-16);

            var result = CovidMgmtApp.GetInstance().Persons.GetIsolatedPersons();

            Assert.Empty(result);
        }

        [Fact] // works
        public void GetIsolated_TwoNegativeTests_ReturnEmptyIsolatersList()
        {
            Patient p = new Patient
            {
                GovtID = "123"
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            LabTestResult t1 = new LabTestResult
            {
                patientID = p.PatientID,
                testID = "123454",
                testDate = DateTime.Now.AddDays(-2),
                isCovidPositive = false
            };

            LabTestResult t2 = new LabTestResult
            {
                patientID = p.PatientID,
                testID = "123434",
                testDate = DateTime.Now.AddDays(-1),
                isCovidPositive = false
            };

            CovidMgmtApp.GetInstance().LabTests.AddNewTest(t1);
            CovidMgmtApp.GetInstance().LabTests.AddNewTest(t2);

            var result = CovidMgmtApp.GetInstance().Persons.GetIsolatedPersons();

            Assert.Empty(result);
        }

        [Fact] // works
        public void GetStatistics_IsNumberInfected_ReturnNumInfected()
        {
            Patient p = new Patient
            {
                GovtID = "12345",
                IsCovidPositive = true,
                Address = new Address { City = "Kfar Yona" }
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var infected = CovidMgmtApp.GetInstance().Persons.GetStatistics().Infected;

            Assert.Equal(1, infected);
        }

        [Fact] // works
        public void GetStatistics_IsNumberHealed_ReturnNumHealed()
        {
            Patient p = new Patient
            {
                GovtID = "12345",
                IsCovidPositive = false,
                Address = new Address { City = "Kfar Yona" }
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var healed = CovidMgmtApp.GetInstance().Persons.GetStatistics().Healed;

            Assert.Equal(1, healed);
        }

        [Fact] // works
        public void GetStatistics_IsNumberIsolated_ReturnNumIsolated()
        {
            Patient p = new Patient
            {
                GovtID = "12345",
                IsCovidPositive = true,
                Address = new Address { City = "Kfar Yona" }
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var Isolated = CovidMgmtApp.GetInstance().Persons.GetStatistics().Isolated;

            Assert.Equal(1, Isolated);
        }

        [Fact] // works
        public void GetStatistics_IsInfectedPerCity_ReturnNumInfectedPerCity()
        {
            Patient p = new Patient
            {
                GovtID = "12345",
                IsCovidPositive = true,
                Address = new Address { City = "Kfar Yona" }
            };

            CovidMgmtApp.GetInstance().Persons.AddNewPatient(p);

            var cityStatistics = CovidMgmtApp.GetInstance().Persons.GetStatistics().CityStatistics;

            Assert.Equal(1, cityStatistics[0].Infected);
        }
    }
}
