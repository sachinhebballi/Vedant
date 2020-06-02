using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using SGMH.Healthcare.Vedant.API.Controllers;
using SGMH.Healthcare.Vedant.Business.Domain;
using SGMH.Healthcare.Vedant.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SGMH.Healthcare.Vedant.API.UnitTests
{
    public class PatientsControllerTests
    {
        private readonly IPatientLogic _patientLogic;
        private readonly IValidator<PatientModel> _patientValidator;
        private readonly PatientsController _patientsController;

        public PatientsControllerTests()
        {
            _patientLogic = Substitute.For<IPatientLogic>();
            _patientValidator = Substitute.For<IValidator<PatientModel>>();

            _patientsController = new PatientsController(_patientLogic, _patientValidator);
        }

        [Fact]
        public void ShouldReturnAllThePatients()
        {
            var pageNumber = 1;
            var pageSize = 10;
            var expectedPatients = GetPatients();

            _patientLogic.GetAllPatients(null, pageNumber, pageSize).Returns(expectedPatients);

            var patients = _patientsController.GetPatients(pageNumber, pageSize, null);

            patients.Should().NotBeNull();
            patients.Should().BeOfType<PagedResult<PatientModel>>();
            patients.Should().BeEquivalentTo(expectedPatients);
        }

        [Fact]
        public void ShouldReturnThePatientById()
        {
            var expectedPatient = GetPatients().Result.First();

            _patientLogic.GetPatient(1).Returns(expectedPatient);

            var patient = _patientsController.GetPatient(1);

            patient.Should().NotBeNull();
            patient.Should().BeOfType<PatientModel>();
            patient.Should().BeEquivalentTo(expectedPatient);
        }

        [Fact]
        public void ShouldSearchPatientsByQueryText()
        {
            var pageNumber = 1;
            var pageSize = 10;

            var expectedPatients = GetPatients();

            _patientLogic.SearchPatients("John", null, pageNumber, pageSize).Returns(expectedPatients);

            var patients = _patientsController.SearchPatients("John", pageNumber, pageSize, null);

            patients.Should().NotBeNull();
            patients.Should().BeOfType<PagedResult<PatientModel>>();
            patients.Should().BeEquivalentTo(expectedPatients);
        }

        [Fact]
        public void ShouldSavePatients()
        {
            var patient = GetPatients().Result.First();
            var validationResult = Substitute.For<ValidationResult>();
            validationResult.IsValid.Returns(true);

            _patientValidator.Validate(patient).Returns(validationResult);

            _patientLogic.Save(patient);

            _patientsController.Save(patient);

            _patientLogic.Received(1);

        }

        private PagedResult<PatientModel> GetPatients()
        {
            return new PagedResult<PatientModel>
            {
                Count = 10,
                Index = 1,
                Result = new List<PatientModel>
                {
                    new PatientModel
                    {
                        PatientId = 1,
                        PatientName = "John Doe",
                        Age = 30,
                        City = "Melbourne",
                        Email = "john.doe@gmail.com",
                        Gender = "Male",
                        Mobile = "0123412345",
                        RegisteredCentreId = 1,
                        RegisteredDate = new System.DateTime(2019,01,01),
                        RegistrationNumber = "MEL0001"
                    }
                }
            };
        }
    }
}
