using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Service.Helpers.DTOs.Customer;
using Service.Services.Interfaces;

namespace Service.Services
{
	public class CustomerService:ICustomerService
	{
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;

        public CustomerService(AppDbContext context,
                             IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<string> CreateAsync(CustomerCreateDto customer)
        {
            var fileResponse = await _fileService.UploadAsync(customer.file);

            if (fileResponse.HasError == true)
            {
                return fileResponse.Response;
            }

            var newCustomer = new Customer
            {
                Name = customer.Name,
                SurName = customer.Surname,
                Text = customer.Text,
                Raiting = (byte)customer.Rating,
                Image = fileResponse.Response
            };

            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var findData = await _context.Customers.FindAsync(id);

            if (findData == null)
            {
                return "Data not found";
            }
            await _fileService.DeletePath(findData.Image);
            _context.Customers.Remove(findData);
            await _context.SaveChangesAsync();

            return "Success";
        }


        public async Task<string> EditAsync(int id, CustomerEditDto customer)
        {
            var findCustomer = await _context.Customers.FindAsync(id);

            if (findCustomer == null)
            {
                return "Data not found";
            }
            if (!string.IsNullOrWhiteSpace(customer.Name))
            {
                findCustomer.Name = customer.Name;
            }
            if (!string.IsNullOrWhiteSpace(customer.Surname))
            {
                findCustomer.SurName = customer.Surname;
            }
            if (!string.IsNullOrWhiteSpace(customer.Text))
            {
                findCustomer.Text = customer.Text;
            }
            if (customer.Rating.HasValue)
            {
                findCustomer.Raiting = (byte)customer.Rating.Value;
            }
            if (customer.file != null)
            {
                if (!string.IsNullOrWhiteSpace(findCustomer.Image))
                {
                    await _fileService.DeletePath(findCustomer.Image);
                }
                var fileResponse = await _fileService.UploadAsync(customer.file);

                if (fileResponse.HasError)
                {
                    return fileResponse.Response;
                }

                findCustomer.Image = fileResponse.Response;
            }

            await _context.SaveChangesAsync();

            return "Success";
        }

        public async Task<ICollection<Customer>> GetAllAsync()
        {
            var datas = await _context.Customers.ToListAsync();

            return datas;
        }

        public Task<Customer> GetById(int id)
        {
            return _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

