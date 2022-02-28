using CoronaProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoronaProgram
{
    public class PersonsService : SingletonService<PersonsService>
    {
        private List<Person> AllPersons { get; set; }
        private List<Person> Infected => AllPersons.Where(x => x.IsCovidPositive).ToList();
        private List<Person> Healed => AllPersons.Where(x => !x.IsCovidPositive && x is Patient).ToList();
        private List<Person> Isolated => GetIsolatedPersons();

        public PersonsService()
        {
            AllPersons = new List<Person>();
        }

        public override void ResetData()
        {
            AllPersons = new List<Person>();
        }

        public Patient GetPatientByGovId(string govID)
        {
            foreach (Person person in AllPersons)
            {
                if (person is Patient patient && patient.GovtID == govID)
                {
                    return patient;
                }
            }

            return null;
        }

        public Patient GetPatientById(string patientID)
        {
            foreach (Person person in AllPersons)
            {
                if (person is Patient patient && patient.PatientID == patientID)
                {
                    return patient;
                }
            }

            return null;
        }

        public Person GetPersonById(string id)
        {
            Person p = (Person)GetPatientById(id);
            if (p != null)
                return p;
            p = (Person)GetPotentialPatientById(id);
            return p;
        }

        public Patient AddNewPatient(Patient p)
        {
            var patient = GetPatientByGovId(p.GovtID);

            if (patient != null)
                return null;

            if(p.PatientID == null)
            {
                p.PatientID = Guid.NewGuid().ToString(); // The number of patients in the system
                p.SignDate = DateTime.Now;
            }


            AllPersons.Add(p);
            return p;
        }

        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();

            foreach (Person person in AllPersons)
            {
                if (person is Patient)
                {
                    patients.Add(person as Patient);
                }
            }

            return patients;
        }

        public Location AddNewLocation(string patientId, Location location)
        {
            var patient = GetPatientById(patientId);

            if (patient == null || (DateTime.Now - location.DateOfVisit).TotalDays > 7)
                return null;

            if (CheckLocation(patient, location) == false)
                return null;

            patient.Locations.Add(location);

            return location;
        }

        public bool CheckLocation(Patient patient, Location location)
        {
            foreach (var l in patient.Locations)
                if (DateTime.Compare(l.DateOfVisit, location.DateOfVisit) == 0)
                    return false;
            return true;
        }

        public List<Location> GetLocations(string patientID)
        {
            var patient = GetPatientById(patientID);

            if (patient == null)
                return null;

            List<Location> locations = new List<Location>();

            foreach (Location l in patient.Locations)
                locations.Add(l);

            return locations;
        }

        public PotentialPatient AddNewPotentialPatient(string patientID, PotentialPatient potential)
        {
            var patient = GetPatientById(patientID);
            if (patient == null)
                return null;

            if (CheckPotentialInPatient(patient, potential) == true)
                return null;

            if (potential.PotentialPatientID == null) // the PotentialPatient is Not In System
            {
                potential.PotentialPatientID = Guid.NewGuid().ToString(); // The number of patients in the system
                potential.SignDate = DateTime.Now;
                AllPersons.Add(potential);
            }
            patient.EncounteredPersons.Add(potential);

                
            return potential;
        }

        public bool CheckPotentialInPatient(Patient patient, PotentialPatient potential)
        {
            foreach (var p in patient.EncounteredPersons)
            {
                if (p is PotentialPatient p_potential && potential.PotentialPatientID == p_potential.PotentialPatientID)
                    return true;
                else if (p is Patient p_patient && potential.PotentialPatientID == p_patient.PatientID)
                    return true;
            }
            return false;
        }

        public List<Person> GetEncounters(string patientID)
        {
            var patient = GetPatientById(patientID);

            if (patient == null)
                return null;

            return patient.EncounteredPersons;
        }

        public List<Person> GetPatientsSince(DateTime since)
        {
            List<Person> sickPeople = new List<Person>();

            foreach (Person p in AllPersons)
            {
                if(p.IsCovidPositive == true && DateTime.Compare(p.SignDate,since) >=0)
                {
                    if(p is Patient)
                        sickPeople.Add(p as Patient);
                    if(p is PotentialPatient)
                        sickPeople.Add(p as PotentialPatient);
                }
            }

            return sickPeople;
        }

        public List<PotentialPatient> GetPotentialPatients()
        {
            List<PotentialPatient> potentials = new List<PotentialPatient>();

            foreach(var p in AllPersons)
            {
                if (p is PotentialPatient && p.IsCovidPositive == true)
                    potentials.Add(p as PotentialPatient);
            }

            return potentials;
        }

        public List<Person> GetIsolatedPersons()
        {
            List<Person> isolaters = new List<Person>();
            bool isolated = false;
            foreach (var p in AllPersons)
            {
                if (p is PotentialPatient potential)
                    isolated = CovidMgmtApp.GetInstance().LabTests.IsIsolate(potential.PotentialPatientID);
                else if (p is Patient patient)
                    isolated = CovidMgmtApp.GetInstance().LabTests.IsIsolate(patient.PatientID);
                else
                    return null;

                if (isolated && (DateTime.Now - p.SignDate).TotalDays <= 14)
                    isolaters.Add(p);
            }

            return isolaters;
        }

        public Patient TransformPotentialPatient(string potentialPatientID, Patient patient)
        {
            var potentialPatient = GetPotentialPatientById(potentialPatientID);

            if (potentialPatient == null) 
                return null;

            patient.PatientID = potentialPatient.PotentialPatientID;
            patient.SignDate = potentialPatient.SignDate;

            SwitchPotentialWithPatient(potentialPatientID, patient);

            this.AllPersons.Remove(potentialPatient);
            this.AllPersons.Add(patient);

            return patient;
        }
        public PotentialPatient GetPotentialPatientById(string potentialPatientId)
        {
            foreach (Person person in AllPersons)
            {
                if (person is PotentialPatient potentialPatient && potentialPatient.PotentialPatientID == potentialPatientId)
                {
                    return potentialPatient;
                }
            }

            return null;
        }

        public void SwitchPotentialWithPatient(string potentialPatientId, Patient patient)
        {
            PotentialPatient potential = GetPotentialPatientById(potentialPatientId);
            if (potential == null)
                return;

            foreach (Person person in AllPersons)
            {
                if (person is Patient p)
                {
                    var encountered = p.EncounteredPersons.FirstOrDefault(x => x is PotentialPatient && (x as PotentialPatient).PotentialPatientID == potentialPatientId);
                    if (encountered != null)
                    {
                        p.EncounteredPersons.Remove(encountered);
                        p.EncounteredPersons.Add(patient);
                        patient.InfectedByPatientID = p.PatientID;
                    }
                }
            }
        }

        public Statistics GetStatistics()
        {
            return new Statistics()
            {
                Infected = this.Infected.Count,
                Healed = this.Healed.Count,
                Isolated = this.Isolated.Count,
                CityStatistics = this.Infected.GroupBy(x => (x as Patient).Address.City).Select(x => new CityStatistics()
                {
                    Infected = x.Count(),
                    City = x.Key
                }).ToList()
            };
        }
    }
}
