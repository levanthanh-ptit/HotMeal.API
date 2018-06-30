using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotMeal.API.Entities;
using HotMeal.API.Helpers;
using HotMeal.API.Models;
using HotMeal.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotMeal.API.Controllers
{
    [Route("api/customers")]
    public class CustomersController : Controller
    {
        private IHotMealRepository _hotMealRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public CustomersController(IHotMealRepository hotMealRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _hotMealRepository = hotMealRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetCustomers")]
        public IActionResult GetCustomers(CustomersResourceParameters customersResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CustomerDto, Customer>
               (customersResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            //if (!_typeHelperService.TypeHasProperties<CustomerDto>
            //    (customersResourceParameters.Fields))
            //{
            //    return BadRequest();
            //}

            var customersFromRepo = _hotMealRepository.GetCustomers(customersResourceParameters);

            var previousPageLink = customersFromRepo.HasPrevious ?
                CreateCustomersResourceUri(customersResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = customersFromRepo.HasNext ?
                CreateCustomersResourceUri(customersResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = customersFromRepo.TotalCount,
                pageSize = customersFromRepo.PageSize,
                currentPage = customersFromRepo.CurrentPage,
                totalPages = customersFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var customers = Mapper.Map<IEnumerable<CustomerDto>>(customersFromRepo);
            return Ok(customers);
        }

        private string CreateCustomersResourceUri(
            CustomersResourceParameters customersResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetCustomers",
                      new
                      {
                          //fields = customersResourceParameters.Fields,
                          orderBy = customersResourceParameters.OrderBy,
                          searchQuery = customersResourceParameters.SearchQuery,
                          genre = customersResourceParameters.Genre,
                          pageNumber = customersResourceParameters.PageNumber - 1,
                          pageSize = customersResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetCustomers",
                      new
                      {
                          //fields = customersResourceParameters.Fields,
                          orderBy = customersResourceParameters.OrderBy,
                          searchQuery = customersResourceParameters.SearchQuery,
                          genre = customersResourceParameters.Genre,
                          pageNumber = customersResourceParameters.PageNumber + 1,
                          pageSize = customersResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("GetCustomers",
                    new
                    {
                        //fields = customersResourceParameters.Fields,
                        orderBy = customersResourceParameters.OrderBy,
                        searchQuery = customersResourceParameters.SearchQuery,
                        genre = customersResourceParameters.Genre,
                        pageNumber = customersResourceParameters.PageNumber,
                        pageSize = customersResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public IActionResult GetCustomer(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<CustomerDto>
              (fields))
            {
                return BadRequest();
            }

            var customerFromRepo = _hotMealRepository.GetCustomer(id);

            if (customerFromRepo == null)
            {
                return NotFound();
            }

            var customer = Mapper.Map<CustomerDto>(customerFromRepo);
            return Ok(customer);
        }

        [HttpPost]
        public IActionResult CreateCustomer([FromBody] CustomerForCreationDto customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            var customerEntity = Mapper.Map<Customer>(customer);

            _hotMealRepository.AddCustomer(customerEntity);

            if (!_hotMealRepository.Save())
            {
                throw new Exception("Creating an customer failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var customerToReturn = Mapper.Map<CustomerDto>(customerEntity);

            return CreatedAtRoute("GetCustomer",
                new { id = customerToReturn.Id },
                customerToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockCustomerCreation(Guid id)
        {
            if (_hotMealRepository.CustomerExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            var customerFromRepo = _hotMealRepository.GetCustomer(id);
            if (customerFromRepo == null)
            {
                return NotFound();
            }

            _hotMealRepository.DeleteCustomer(customerFromRepo);

            if (!_hotMealRepository.Save())
            {
                throw new Exception($"Deleting customer {id} failed on save.");
            }

            return NoContent();
        }

    }
}
