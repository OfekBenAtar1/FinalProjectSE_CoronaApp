using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoronaProgram.Models
{

	public class Patient : Person
	{
		public string PatientID { get; set; }
		public string GovtID { get; set; }
		public DateTime BirthDate { get; set; }
		public string Email { get; set; }

		public Address Address { get; set; }

		public int HouseResidentsAmount { get; set; }
		public string InfectedByPatientID { get; set; }
		[JsonIgnore] public List<Location> Locations { get; set; } = new List<Location>();
		[JsonIgnore] public List<Person> EncounteredPersons { get; set; } = new List<Person>();


	}
}
