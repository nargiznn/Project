using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using Service.Helpers.DTOs.Reservation;
using Service.Services.Interfaces;

namespace SteakInProject.Controllers
{
    public class ReservationsController : BaseController
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreateDto reservationDto)
        {
            if (reservationDto == null)
            {
                return BadRequest("Invalid reservation data");
            }
            var reservation = new Reservation
            {
                Name = reservationDto.Name,
                Surname = reservationDto.Surname,
                Email = reservationDto.Email,
                PhoneNumber = reservationDto.PhoneNumber,
                Date = reservationDto.Date,
                Time = reservationDto.Time,
                PeopleCount = reservationDto.PeopleCount,
                Status = ReservationStatus.Pending  
            };

            var createdReservation = await _reservationService.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationById), new { id = createdReservation.Id }, createdReservation);
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await _reservationService.GetReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReservationById(int id)
        {
            var reservations = await _reservationService.GetReservationsAsync();
            var foundReservation = reservations.FirstOrDefault(r => r.Id == id);

            if (foundReservation == null)
            {
                return NotFound("Reservation not found");
            }

            return Ok(foundReservation);
        }

        [HttpPost("update-status/{id}")]
        public async Task<IActionResult> UpdateReservationStatus(int id, [FromBody] ReservationStatus status)
        {
            var reservation = await _reservationService.UpdateReservationStatusAsync(id, status);
            if (reservation == null)
            {
                return NotFound("Reservation not found");
            }
            return Ok(new { message = "Reservation status updated and email sent successfully.", reservation });
        }
    }
}
