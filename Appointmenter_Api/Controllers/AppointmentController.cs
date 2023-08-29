using Application_Service.Handlers;
using Domain.Appointment.Commands;
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
        catch (Exception)
        {

            return Ok("some thing is wrrong. call to support");
        }
    }
}
