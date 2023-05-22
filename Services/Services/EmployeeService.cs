using AutoMapper;
using Domain.Models;
using Repository.Repositories.Interfaces;
using Services.DTOs.Employee;
using Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepo,IMapper mapper)
        {
            _employeeRepo = employeeRepo; 
            _mapper = mapper;
        }

        public async Task CreateAsync(EmployeeCreateDto employee) => await _employeeRepo.CreateAsync(_mapper.Map<Employee>(employee));
      
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync() => _mapper.Map<IEnumerable<EmployeeDto>>(await _employeeRepo.FindAllAsync());

        public async Task<EmployeeDto> GetByIdAsync(int? id) => _mapper.Map<EmployeeDto>(await _employeeRepo.GetByIdAsync(id));

        public async Task DeleteAsync(int? id) => await _employeeRepo.DeleteAsync(await _employeeRepo.GetByIdAsync(id));

        public async Task UpdateAsync(int? id, EmployeeUpdateDto employee)
        {
            if (id is null) throw new ArgumentNullException();

            var existEmployee = await _employeeRepo.GetByIdAsync(id) ?? throw new NullReferenceException();

            _mapper.Map(employee, existEmployee);

            await _employeeRepo.UpdateAsync(existEmployee);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchAsync(string? searchText) 
        {
            if(string.IsNullOrEmpty(searchText))
                return _mapper.Map<IEnumerable<EmployeeDto>>(await _employeeRepo.FindAllAsync());
            return _mapper.Map<IEnumerable<EmployeeDto>>(await _employeeRepo.FindAllAsync(m => m.FullName.Contains(searchText)));
        }

        public async Task SoftDeleteAsync(int? id)
        {
            await _employeeRepo.SoftDeleteAsync(id);
        }
    }
}
