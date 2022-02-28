# FinalProjectSE_CoronaApp
Final Project of the course "Software Engineering", 3rd year Degree Computer Engineering.
This project made by Ofir Nahshoni and Ofek Ben Atar.
Language: C#. API .NET Core.

General Concept and macro details:
The system monitors Patients (Infected and Healed people) and Potential Patients (people that were encountered by infected people).
The project is basically a Covid-19 tracking system, based on REST API requests (Postman). The system contains 1 solution (), seperate into 2 projects:
1. CoronaProgram.Tests - Contains all the unit tests.
2. CoronaProgram - Contains all the code of program.

Automatic build instructions:
To build successfuly the program you need to

Creational Design Patterns:
We used "Singleton" in our project as a creational design pattern.
"Singleton" ensures that only one object (C# class) of its kind exists and provides a single point of access to it for any other code.
The classes inplemented by this design pattern are: SingletonService<T>.cs, CovidMgmtApp.cs, PersonsService.cs, LabTestsService.cs.

All the services classses created to handle the logics from API requests.
  
For each REST API path we created a Controller class:
1. PersonsController - Supports all the requests related to path /patients
2. PersonsController - Supports all the requests related to path /labtests
3. PersonsController - Supports all the requests related to path /statistics
To each request we implemented a method that invokes one or more functions from one of the Services classes (mentioned ealier).

Unit Tests:
We used Xunit for writing the unit tests. The tests are all in the project CoronaProgram.Tests.
Each method, used in our system, was checked by several unit tests.
For example: the method AddPatient (PUT) was checked by several Xunit tests:
1. If we try to add a Patient that was already signed in the system, the test should return null.
2. If we try to add a Patient that wasn't signed in the system yet, the test should return the Patient we added.
3. Checking that the Patient that we added actually was added to the system as a Patient.

Note: In this repository we uploaded a pdf file that contains the class diagram of the system.
