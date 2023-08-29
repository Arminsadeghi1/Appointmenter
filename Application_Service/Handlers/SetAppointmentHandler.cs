using Core.Appointment.Entities;
using Core.Doctor.Entities;
using Data.Repositories.Contracts;
using Domain.Appointment.Commands;
using Domain.Appointment.Exceptions;
using Domain.Doctor.Enum;
using Domain.Doctor.Exceptions;

namespace Application_Service.Handlers;

public sealed class SetAppointmentHandler
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;

    public SetAppointmentHandler(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
    }

    public async Task Handle(SetAppointmentCommand command, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.Load(command.DoctorId, cancellationToken);
        if (doctor == null)
            throw new DoctorNotFoundException();

        var dayAppointments = await _appointmentRepository.LoadByDay(command.AppointmentStartDateTime, cancellationToken);


        CheckAppointmentDuration(
            doctor.LevelType, command.DurationMinutes);

        CheckAppointmentTimeAccordingClinicSchedule(
            command.AppointmentStartDateTime, command.DurationMinutes);

        CheckAppointmentTimeAccordingDoctorSchedule(
            command.AppointmentStartDateTime, command.DurationMinutes, doctor.WeekLySchedule);

        CheckNumberOfPatientAppointmentsPerDay(
            dayAppointments, command.PatientId);

        CheckOverlapOfPatientAppointmentsInDay(
            command.AppointmentStartDateTime, command.DurationMinutes, dayAppointments, command.PatientId);

        CheckOverlapForDoctors(
            doctor.LevelType, command.AppointmentStartDateTime, command.DurationMinutes, dayAppointments);

    }


    private void CheckAppointmentDuration(
        LevelType doctorLevelType, int durationMinutes)
    {
        //ToDo: load time limits dynamic from DB
        byte MinTimeForGeneral = 5;
        byte MaxTimeForGeneral = 15;
        byte MinTimeForExpert = 10;
        byte MaxTimeForExpert = 30;

        if (doctorLevelType == LevelType.General)
            if (durationMinutes < MinTimeForGeneral || durationMinutes > MaxTimeForGeneral)
                throw new InvalidDurationMinutesForGeneralDoctorException();

        if (doctorLevelType == LevelType.Expert)
            if (durationMinutes < MinTimeForExpert || durationMinutes > MaxTimeForExpert)
                throw new InvalidDurationMinutesForGeneralDoctorException();
    }

    private void CheckAppointmentTimeAccordingClinicSchedule(
        DateTime appointmentStartDateTime, int durationMinutes)
    {
        //ToDo: Clinic Schedule time limits dynamic from DB
        DayOfWeek[] clinicWorkingDays = { DayOfWeek.Sunday, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday };

        TimeSpan clinicStartTime = new TimeSpan(9, 0, 0);
        TimeSpan clinicEndTime = new TimeSpan(18, 0, 0);

        if (!clinicWorkingDays.Contains(appointmentStartDateTime.DayOfWeek))
            throw new TheClinicIsClosedOnThisDayException();

        if (appointmentStartDateTime.TimeOfDay < clinicStartTime)
            throw new TheClinicIsClosedOnThisTimeException();
        
        if (appointmentStartDateTime.TimeOfDay.Add(new TimeSpan(0,durationMinutes,0))  < clinicEndTime)
            throw new TheClinicIsClosedOnThisTimeException();
    }

    private void CheckAppointmentTimeAccordingDoctorSchedule(
        DateTime appointmentStartDateTime, int durationMinutes, List<DoctorSchedule> weekLySchedule)
    {
        var schedule = weekLySchedule.SingleOrDefault(s => s.DayOfWeek == appointmentStartDateTime.DayOfWeek);
        if (schedule == null)
            throw new DoctorIsNotAvailableOnThisDayException();

        if (appointmentStartDateTime.TimeOfDay < schedule.StartTime)
            throw new DoctorIsNotAvailableOnThisTimeException();

        if (appointmentStartDateTime.TimeOfDay.Add(new TimeSpan(0, durationMinutes, 0)) < schedule.EndTime)
            throw new DoctorIsNotAvailableOnThisDayException();

    }

    private void CheckNumberOfPatientAppointmentsPerDay(
        List<Appointment> dayAppointments, Guid patientId)
    {
        if (dayAppointments.Count(c => c.Patient.PatientId == patientId) > 1)
            throw new InvalidNumberOfPatientAppointmentsPerDayException();
    }

    private void CheckOverlapOfPatientAppointmentsInDay(
        DateTime appointmentStartDateTime, int durationMinutes, List<Appointment> dayAppointments, Guid patientId)
    {
        var pervAppointment = dayAppointments.SingleOrDefault(s => s.Patient.PatientId == patientId);

        if (pervAppointment is null)
            return;

        if (appointmentStartDateTime > pervAppointment.StartTime && 
            appointmentStartDateTime < pervAppointment.EndTime
            )
            throw new AppointmentTimeHasOverlapWithPreviousException();
        
        if (appointmentStartDateTime.AddMinutes(durationMinutes) > pervAppointment.StartTime && 
            appointmentStartDateTime.AddMinutes(durationMinutes) < pervAppointment.EndTime
            )
            throw new DoctorIsNotAvailableOnThisTimeException();

    }

    private void CheckOverlapForDoctors(
        LevelType doctorLevelType, DateTime appointmentStartDateTime, byte durationMinutes, List<Appointment> dayAppointments)
    {
        var alowableOverlapCount = 0;

        switch (doctorLevelType) 
        {
            case LevelType.General:
                alowableOverlapCount = 2;
                break;
            case LevelType.Expert:
                alowableOverlapCount = 3;
                break;
        }

        var isOutOfRange = dayAppointments.Count(
            c => 
            appointmentStartDateTime < c.StartTime && 
            appointmentStartDateTime.Add(new TimeSpan(0, durationMinutes, 0)) < c.EndTime);

        if (isOutOfRange >= alowableOverlapCount)
            throw new OverlapForDoctorsException();
    }

}
