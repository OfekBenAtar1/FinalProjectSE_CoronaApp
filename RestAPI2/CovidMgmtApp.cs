using CoronaProgram.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaProgram
{
    public class CovidMgmtApp : SingletonService<CovidMgmtApp>
    {
        public PersonsService Persons 
        {
            get
            {
                return PersonsService.GetInstance();
            }
        }

        public LabTestsService LabTests
        {
            get
            {
                return LabTestsService.GetInstance();
            }
        }

        public override void ResetData()
        {
            Persons.ResetData();
            LabTests.ResetData();
        }
    }
}
