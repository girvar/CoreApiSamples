using System.Collections.Generic;
using System;
using CoreApiSamples.Services;
using System.Linq.Expressions;
using System.Linq;

namespace CoreApiSamples.Repositories
{
    public interface IPatientRepository
    {
        IReadOnlyList<Patient> GetAll();
        Patient GetByIdentity(Guid identity, bool asNoTracking = default);
        void Put(Patient patient);
        long GetPatientsCount();
    }

    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _patientContext;

        public PatientRepository(PatientDbContext patientContext)
        {
            _patientContext = patientContext;
        }

        public Patient GetByIdentity(Guid identity, bool asNoTracking = default)
        {
            return _patientContext.Patients.SingleOrDefault(x => x.Identity == identity);
        }

        public IReadOnlyList<Patient> GetAll()
        {
            return FindAll(x => true);
        }

        public IReadOnlyList<Patient> FindAll(Expression<Func<Patient, bool>> filter)
        {
            return _patientContext.Patients.Where(filter).ToList();
        }
        public void Put(Patient patient)
        {
            _patientContext.Patients.Add(patient);
            _patientContext.SaveChanges();
        }

        public long GetPatientsCount()
        {
            return _patientContext.Patients.Count();
        }
    }
}
