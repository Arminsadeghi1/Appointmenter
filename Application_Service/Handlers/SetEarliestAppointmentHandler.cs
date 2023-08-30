using Core.Appointment.Entities;
using Core.Doctor.Entities;
using Data;
using Data.Repositories.Contracts;
using Domain.Appointment.Commands;
using Domain.Appointment.Exceptions;
using Domain.Doctor.Enum;
using Domain.Doctor.Exceptions;
using System;

namespace Application_Service.Handlers;

public sealed class SetEarliestAppointmentHandler
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    private Guid patientId;
    private Doctor doctor;

    public SetEarliestAppointmentHandler(
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork)
    {
        _doctorRepository = doctorRepository;
        _appointmentRepository = appointmentRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<DateTime> Handle(SetEarliestAppointmentCommand command, CancellationToken cancellationToken)
    {
        patientId = command.PatientId;
        doctor = await _doctorRepository.Load(command.DoctorId, cancellationToken);
        if (doctor == null)
            throw new DoctorNotFoundException();

        CheckAppointmentDuration(
            doctor.LevelType, command.DurationMinutes);

        var searchPeriod = 30;
        var appointmentStartDateTime = await FindFirstAvailableTime(command.DurationMinutes, searchPeriod, cancellationToken);
        if (appointmentStartDateTime is null)
            throw new NotFoundAnyAppiontmentChanseException();

        var appointment = new Appointment(
            appointmentStartDateTime.Value,
            appointmentStartDateTime.Value.AddMinutes(command.DurationMinutes),
            AppointmentDoctor.New(command.DoctorId, doctor.LevelType, doctor.FullName),
            AppointmentPatient.New(command.PatientId, ""));

        await _appointmentRepository.Add(appointment, cancellationToken);

        await _unitOfWork.CommitAsync();

        return appointmentStartDateTime.Value;
    }

    private async Task<DateTime?> FindFirstAvailableTime(
        byte durationMinutes, int period, CancellationToken cancellationToken)
    {
        var today = DateTime.Now.Date;

        for (int i = 0; i < period; i++)
        {
            var dayAppointments = await _appointmentRepository.LoadByDay(today.AddDays(i), cancellationToken);

            var appointmentStartDateTime = ProseccDuringADay(durationMinutes, today.AddDays(i), dayAppointments);
            if (appointmentStartDateTime is not null)
                return today.AddDays(i).Add(appointmentStartDateTime.Value);
        }
        return null;
    }

    private TimeSpan? ProseccDuringADay(
        int durationMinutes, DateTime dete, List<Appointment> appointments)
    {
        TimeSpan clinikStartTime = new TimeSpan(9, 0, 0);
        TimeSpan clinikEndTime = new TimeSpan(18, 0, 0);
        var appointmentsCounts = appointments.Count();

        if (clinikStartTime.Add(new TimeSpan(0, durationMinutes, 0)) < appointments[0].StartDateTime.TimeOfDay)
            if (Varification(dete, clinikStartTime, durationMinutes, appointments))
                return clinikStartTime;

        for (int i = 0; i < appointmentsCounts - 1; i++)
        {
            if (appointments[i].EndDateTime.TimeOfDay.Add(new TimeSpan(0, durationMinutes, 0)) < appointments[i + 1].StartDateTime.TimeOfDay)
                if (Varification(dete, clinikStartTime, durationMinutes, appointments))
                    return appointments[i].EndDateTime.TimeOfDay;
        }

        if (appointments[appointmentsCounts - 1].EndDateTime.TimeOfDay.Add(new TimeSpan(0, durationMinutes, 0)) < clinikEndTime)
            if (Varification(dete, clinikStartTime, durationMinutes, appointments))
                return appointments[appointmentsCounts - 1].EndDateTime.TimeOfDay;

        return null;
    }

    private bool Varification(
        DateTime date, TimeSpan time, int durationMinutes, List<Appointment> appointments)
    {
        try
        {
            CheckAppointmentTimeAccordingDoctorSchedule(
                date.Add(time), durationMinutes, doctor.WeekLySchedule);

            CheckNumberOfPatientAppointmentsPerDay(
                appointments, patientId);

            CheckOverlapOfPatientAppointmentsInDay(
                date.Add(time), durationMinutes, appointments, patientId);

            CheckOverlapForDoctors(
                doctor.LevelType, date.Add(time), durationMinutes, appointments);

            return true;
        }
        catch (Exception)
        {
           return false;
        }

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

    private void CheckAppointmentTimeAccordingDoctorSchedule(
        DateTime appointmentStartDateTime, int durationMinutes, List<DoctorSchedule> weekLySchedule)
    {
        var schedule = weekLySchedule.SingleOrDefault(s => s.DayOfWeek == appointmentStartDateTime.DayOfWeek);
        if (schedule == null)
            throw new DoctorIsNotAvailableOnThisDayException();

        if (appointmentStartDateTime.TimeOfDay < schedule.StartTime)
            throw new DoctorIsNotAvailableOnThisTimeException();

        if (appointmentStartDateTime.TimeOfDay.Add(new TimeSpan(0, durationMinutes, 0)) < schedule.EndTime)
            throw new DoctorIsNotAvailableOnThisTimeException();

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

        if (appointmentStartDateTime > pervAppointment.StartDateTime &&
            appointmentStartDateTime < pervAppointment.EndDateTime
            )
            throw new AppointmentTimeHasOverlapWithPreviousException();

        if (appointmentStartDateTime.AddMinutes(durationMinutes) > pervAppointment.StartDateTime &&
            appointmentStartDateTime.AddMinutes(durationMinutes) < pervAppointment.EndDateTime
            )
            throw new AppointmentTimeHasOverlapWithPreviousException();

    }

    private void CheckOverlapForDoctors(
        LevelType doctorLevelType, DateTime appointmentStartDateTime, int durationMinutes, List<Appointment> dayAppointments)
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
            appointmentStartDateTime < c.StartDateTime &&
            appointmentStartDateTime.Add(new TimeSpan(0, durationMinutes, 0)) < c.EndDateTime);

        if (isOutOfRange >= alowableOverlapCount)
            throw new OverlapForDoctorsException();
    }


}
