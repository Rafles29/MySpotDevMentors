﻿using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Abstractions;
using MySpot.Core.DomainServices;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

public sealed class ReserveParkingSpotForVehicleHandler : ICommandHandler<ReserveParkingSpotForVehicle>
{
    private readonly IWeeklyParkingSpotRepository _repository;
    private readonly IParkingReservationService _reservationService;
    private readonly IClock _clock;
    private readonly IUserRepository _userRepository;

    public ReserveParkingSpotForVehicleHandler(IWeeklyParkingSpotRepository repository,
        IParkingReservationService reservationService, IClock clock, IUserRepository userRepository)
    {
        _repository = repository;
        _reservationService = reservationService;
        _clock = clock;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(ReserveParkingSpotForVehicle command)
    {
        var (spotId, reservationId, userId, licencePlate, capacity, date) = command;
        var week = new Week(_clock.Current());
        var parkingSpotId = new ParkingSpotId(spotId);
        var weeklyParkingSpots = (await _repository.GetByWeekAsync(week)).ToList();
        var parkingSpotToReserve = weeklyParkingSpots.SingleOrDefault(x => x.Id == parkingSpotId);

        if (parkingSpotToReserve is null)
        {
            throw new WeeklyParkingSpotNotFoundException(spotId);
        }
        
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        var reservation = new VehicleReservation(reservationId, user.Id, new EmployeeName(user.FullName), licencePlate, capacity, new Date(date));

        _reservationService.ReserveSpotForVehicle(weeklyParkingSpots, JobTitle.Employee, parkingSpotToReserve,
            reservation);

        await _repository.UpdateAsync(parkingSpotToReserve);
    }
}