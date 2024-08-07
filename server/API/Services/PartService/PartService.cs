﻿using API.Models.RequestModels;
using DataAccessLayer_AutoAuctioneer.Models;
using DataAccessLayer_AutoAuctioneer.Repositories.Interfaces;

namespace API.Services.PartService;

public class PartService : IPartService
{
    private readonly ILogger<PartService> _logger;
    private readonly IPartRepository _partRepository;
    private readonly IUserRepository _userRepository;

    public PartService(IPartRepository carPartRepository, IUserRepository userRepository, ILogger<PartService> logger)
    {
        _partRepository = carPartRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<List<PartEntity>?> GetAllParts()
    {
        var parts = await _partRepository.GetAllParts();
        return parts;
    }

    public async Task<List<PartEntity>?> GetOwned(Guid id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            _logger.LogInformation("No such user present in the database");
            return null;
        }

        var parts = await _partRepository.GetPartsOfSingleUser(id);

        return parts;
    }

    public async Task<PartEntity?> GetPartById(Guid guid)
    {
        var part = await _partRepository.GetPartById(guid);

        return part;
    }

    public async Task<bool> AddPart(AddPartRequest request)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            _logger.LogInformation("UserEntity does not exist");
            return false;
        }

        var part = new PartEntity
        {
            UserId = request.UserId,
            Name = request.Name,
            PartType = request.PartType,
            Description = request.Description,
            MarketPrice = request.MarketPrice,
            Category = request.Category,
            Manufacturer = request.Manufacturer
        };
        var response = await _partRepository.StorePart(part);

        return response;
    }

    public async Task<bool> UpdatePart(UpdatePartRequest request)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            _logger.LogInformation("UserEntity does not exist");
            return false;
        }

        var part = await _partRepository.GetPartById(request.Id);
        if (part == null)
        {
            _logger.LogInformation("PartEntity does not exist.");
            return false;
        }
/*  Do something to check for ownership
        if (request.UserId != response.Data.UserId) {
            Console.WriteLine("UserEntity does not own this part");
            return false;
        }*/

        if (request.Name != null) part.PartType = request.PartType;
        if (request.Description != null) part.Description = request.Description;
        if (request.MarketPrice != null) part.MarketPrice = request.MarketPrice;
        if (request.Category != null) part.Category = request.Category;
        if (request.Manufacturer != null) part.Manufacturer = request.Manufacturer;

        var response = await _partRepository.UpdatePart(part);

        return response;
    }

    public async Task<bool> DeletePart(DeletePartRequest request)
    {
        var user = await _userRepository.GetUserById(request.UserId);
        if (user == null)
        {
            _logger.LogInformation("UserEntity does not exist.");
            return false;
        }

        var part = await _partRepository.GetPartById(request.Id);
        if (part == null)
        {
            _logger.LogInformation("PartEntity does not exist.");
            return false;
        }
/*      TODO(Check for ownership)
        if (request.UserId != response.Data.UserId) {
            Console.WriteLine("UserEntity does not own this part");
            return false;
        }*/

        var response = await _partRepository.DeletePart(request.Id);
        return response;
    }
}