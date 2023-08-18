using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.DTOs;

namespace WebApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {

        private readonly IReminderService _reminderService;

        public RemindersController(IReminderService reminderService)
        {
            _reminderService = reminderService;
        }



        [HttpPost("addreminder")]
        public async Task<ActionResult<DefaultReminderDTO>> AddReminder(DefaultReminderDTO reminder)
        {
            _reminderService.Insert(new Reminder()
            {
                UserId = reminder.UserId,
                Title = reminder.Title,
                Date = reminder.Date,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
            });

            return reminder;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateReminder(GetReminderDTO dto)
        {

            if (ModelState.IsValid)
            {
                var reminderToUpdate = _reminderService.GetElementById(dto.Id);
                if (reminderToUpdate == null)
                {
                    return NotFound();
                }

                reminderToUpdate.UserId = dto.UserId;
                reminderToUpdate.Date = dto.Date;
                reminderToUpdate.Title = dto.Title;
                reminderToUpdate.Description = dto.Description;
                reminderToUpdate.IsRead = dto.IsRead;


                _reminderService.Update(reminderToUpdate);

                return Ok("Reminder successfully updated");
            }
            else
            {
                return BadRequest("Invalid data provided.");
            }
        }


        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteReminder(int reminderID)
        {
            var remainder = _reminderService.GetElementById(reminderID);
            if (remainder == null)
            {
                return NotFound();
            }

            _reminderService.Delete(remainder);

            return Ok("Remainder deleted successfully");
        }



        [HttpGet("get")]
        public Reminder GetReminder(int id)
        {
            var reminder = _reminderService.GetElementById(id);

            if (reminder == null)
            {
                throw new Exception("NotFound");
            }

            return reminder;
        }


        [HttpGet]
        public List<GetReminderDTO> GetAllReminders()
        {
            List<Reminder> reminders = _reminderService.GetListAll();

            List<GetReminderDTO> remindersDTOs = reminders.Select(reminder => new GetReminderDTO
            {
                Id = reminder.Id,
                Title = reminder.Title,
                Date = reminder.Date,
                UserId = reminder.UserId,
                Description = reminder.Description,
                IsRead = reminder.IsRead,
            }).ToList();

            return remindersDTOs;
        }




    }
}
