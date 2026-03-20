using Microsoft.EntityFrameworkCore;
using miniEcommerceApi.Data;
using miniEcommerceApi.DTOs.AddressDTO.Request;
using miniEcommerceApi.DTOs.AddressDTO.Response;
using miniEcommerceApi.Interfaces;
using miniEcommerceApi.Mappings;
using miniEcommerceApi.Models;

namespace miniEcommerceApi.Services
{
    public class AddressesService : IAddressesService
    {
        private readonly AppDbContext _context;

        public AddressesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AddressResponse> GetAddressById(Guid id)
        {
            var Address = await _context.Addresses.FindAsync(id);
            if (Address == null)
                throw new KeyNotFoundException("Address not found");

            return Address.ToResponse();
        }
        public async Task<IEnumerable<AddressResponse>> GetAddressesByCustomerId(Guid customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException("Customer not found");

            var Address = await _context.Addresses.Where(a => a.CustomerId == customerId).ToListAsync();
            return Address.Select(a => a.ToResponse());
        }
        public async Task<AddressResponse> CreateAddress(Guid customerId, CreateAddressRequest dto)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException("Customer not found");
            var address = new Addresses
            {
                CustomerId = customerId,
                ZipCode = dto.ZipCode,
                Street = dto.Street,
                Number = dto.Number,
                Neighborhood = dto.Neighborhood,
                City = dto.City,
                State = dto.State
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return address.ToResponse();
        }
        public async Task<AddressResponse> UpdateAddress(Guid id, UpdateAddressRequest dto)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
                throw new KeyNotFoundException("Address not found");

            if (dto.ZipCode != null) address.ZipCode = dto.ZipCode;
            if (dto.Street != null) address.Street = dto.Street;
            if (dto.Number != null) address.Number = dto.Number;
            if (dto.Neighborhood != null) address.Neighborhood = dto.Neighborhood;
            if (dto.City != null) address.City = dto.City;
            if (dto.State != null) address.State = dto.State;

            await _context.SaveChangesAsync();

            return address.ToResponse();
        }

        public async Task DeleteAddress(Guid id)
        {
            var address = await _context.Addresses.FindAsync(id);

            if (address == null)
                throw new KeyNotFoundException("Address not found");

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
        }
    }
}
