using Core.Doctor.Entities;

namespace Data.Repositories.Contracts;

public interface IDoctorRepository
{

    public Task<Doctor> Load(Guid id, CancellationToken cancellationToken);

    public Task Add(Doctor doctor, CancellationToken cancellationToken);
}
