using Core.Appointment.Entities;

namespace Data.Repositories.Contracts;

public interface IAppointmentRepository
{

    public Task<Appointment> Load(Guid id, CancellationToken cancellationToken);

    public Task<List<Appointment>> LoadByDay(DateTime startDateTime, CancellationToken cancellationToken);


    public Task Add(Appointment appointment, CancellationToken cancellationToken);
}
