using System;
using System.Text.Json.Serialization;

namespace CoronaProgram.Models
{
    public class Person
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
	    public string PhoneNumber { get; set; }
		[JsonIgnore] public bool IsCovidPositive { get; set; }

		[JsonIgnore] public DateTime SignDate { get; set; }
	}
}
