﻿using DataAccessLayer_API.Models;
using System.Numerics;

namespace DataAccessLayer_AutoAuctioneer.Repositories.Interfaces
{
    public interface IBidRepository
    {
        Task<bool> DeleteBid(BidEntity entity);
        Task<List<BidEntity>?> GetAllBids();
        Task<BidEntity?> GetBidById(Guid id);
        Task<List<BidEntity>?> GetOwned(Guid id);
        Task<List<BidEntity>?> GetBidsPerListing(Guid guid);
        Task<bool> PostBid(BidEntity bid);
        Task<bool> UpdateBidAmt(BidEntity entity);
    }
}