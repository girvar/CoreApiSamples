using CoreApiSamples.Repositories;
using System;
using System.Collections.Generic;

namespace CoreApiSamples.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repository;
        public PatientService(IPatientRepository patientRepository)
        {
            _repository = patientRepository;
        }

        public Patient CreatePatient(Patient patient)
        {
            patient.Identity = Guid.NewGuid();
            patient.CreatedDate = DateTime.Now;
            _repository.Put(patient);
            return patient;
        }

        public IReadOnlyList<Patient> GetAll()
        {
            return _repository.GetAll();
        }

        public Patient GetByIdentity(Guid identity)
        {
            return _repository.GetByIdentity(identity);
        }
    }

    public interface IPatientService
    {
        IReadOnlyList<Patient> GetAll();
        Patient GetByIdentity(Guid identity);
        Patient CreatePatient(Patient patient);
    }

    public class Patient
    {

        public Guid Identity { get; set; }
        public string ID1 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? DayOfBirth { get; set; }

        public Gender? Sex { get; set; }
        public Guid? HospitalId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public enum Gender { MALE, FEMALE, OTHER };
}