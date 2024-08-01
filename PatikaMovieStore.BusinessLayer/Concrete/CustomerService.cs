using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PatikaMovieStore.BusinessLayer.Abstract;
using PatikaMovieStore.BusinessLayer.Exceptions;
using PatikaMovieStore.DataAccessLayer.Context;
using PatikaMovieStore.DtoLayer.CustomerDtos;
using PatikaMovieStore.EntityLayer.Entities;

namespace PatikaMovieStore.BusinessLayer.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly MovieStoreContext _context;
        private readonly IMapper _mapper;

        public CustomerService(MovieStoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                throw new NotFoundException($"Customer with ID {id} not found !");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CustomerDto>> GetAllCustomerAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return _mapper.Map<List<CustomerDto>>(customers);
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
                throw new NotFoundException($"Customer with ID {id} not found !");

            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
        {
            var customer = await _context.Customers.FindAsync(updateCustomerDto.Id);

            if (customer == null)
                throw new NotFoundException($"Actor with ID {updateCustomerDto.Id} not found !");

            _mapper.Map(updateCustomerDto, customer);
            await _context.SaveChangesAsync();
        }
    }
}
