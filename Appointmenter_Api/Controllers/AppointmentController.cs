using Application_Service.Handlers;
using Domain.Appointment.Commands;
using Domain.Appointment.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Appointmenter_Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{

    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(ILogger<AppointmentController> logger)
    {
        _logger = logger;
    }

    [HttpPost("set-appointment")]
    public async Task<IActionResult> SetAppointment(
        [FromServices] SetAppointmentHandler handler, [FromBody] SetAppointmentCommand command)
    {
        try
        {
            await handler.Handle(command, CancellationToken.None);
            return Ok("Appointment registered successfully");
        }
        catch (InvalidDurationMinutesForGeneralDoctorException ex)
        {
            //ToDo: lod ex message
            return Ok("مدت زمان قرار ملاقات معتبر نمیباشد");
        }
        catch (TheClinicIsClosedOnThisDayException ex)
        {
            //ToDo: lod ex message
            return Ok("درمانگاه در این روز تعطیل میباشد");
        }
        catch (TheClinicIsClosedOnThisTimeException ex)
        {
            //ToDo: lod ex message
            return Ok("درمانگاه در این ساعت تعطیل میباشد");
        }
        catch (DoctorIsNotAvailableOnThisDayException ex)
        {
            //ToDo: lod ex message
            return Ok("دکتر مورد نظر در این روز حضور ندارد");
        }
        catch (DoctorIsNotAvailableOnThisTimeException ex)
        {
            //ToDo: lod ex message
            return Ok("دکتر مورد نظر در این ساعت حضور ندارد");
        }
        catch (InvalidNumberOfPatientAppointmentsPerDayException ex)
        {
            //ToDo: lod ex message
            return Ok("شما در حال حاضر دو نوبت فعال دارید و دیگر در این روز قادر به دریافت نوبت نمیباشید");
        }
        catch (AppointmentTimeHasOverlapWithPreviousException ex)
        {
            //ToDo: lod ex message
            return Ok("بازه زمانی انتخاب شده با دیگر نوبت شما در این روز تداخل دارد.");
        }
        catch (OverlapForDoctorsException ex)
        {
            //ToDo: lod ex message
            return Ok("به دلیل تکمیل ظرفیت پزشکان . درمانگاه ظرفیت ثبت نوبت جدید را ندارد. لطفا زمان دیگری را انتخاب بفرماییدو ");
        }
        catch (Exception ex)
        {
            //ToDo: lod ex message
            return Ok("some thing is wrrong. call to support");
        }
    }
}
