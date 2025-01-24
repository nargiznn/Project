using System;
using Domain.Entities;
using Domain.Enum;

namespace Service.Services.Interfaces
{
	public interface IReservationService
	{
        Task<Reservation> CreateReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationsAsync();
        Task<Reservation> UpdateReservationStatusAsync(int id, ReservationStatus status);

    }
}

