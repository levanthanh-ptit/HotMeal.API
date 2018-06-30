using HotMeal.API.Entities;
using HotMeal.API.Helpers;
using HotMeal.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotMeal.API.Services
{
    public class HotMealRepository : IHotMealRepository
    {
        private HotMealContext _context;
        private IPropertyMappingService _propertyMappingService;

        public HotMealRepository(HotMealContext context,
            IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }

        public void AddCustomer(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            _context.Customers.Add(customer);
        }

        public bool CustomerExists(Guid customerId)
        {
            return _context.Customers.Any(a => a.Id == customerId);
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }


        public Customer GetCustomer(Guid customerId)
        {
            return _context.Customers.FirstOrDefault(a => a.Id == customerId);
        }

        public PagedList<Customer> GetCustomers(
            CustomersResourceParameters customersResourceParameters)
        {
            //var collectionBeforePaging = _context.Customers
            //    .OrderBy(a => a.FirstName)
            //    .ThenBy(a => a.LastName).AsQueryable();

            var collectionBeforePaging =
                _context.Customers
                .ApplySort(customersResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<CustomerDto, Customer>());

            if (!string.IsNullOrEmpty(customersResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = customersResourceParameters.Genre
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(customersResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = customersResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Customer>.Create(collectionBeforePaging,
                customersResourceParameters.PageNumber,
                customersResourceParameters.PageSize);
        }

        public IEnumerable<Customer> GetCustomers(IEnumerable<Guid> customerIds)
        {
            return _context.Customers.Where(a => customerIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void UpdateCustomer(Customer customer)
        {
            // no code in this implementation
        }
        public void AddCourse(Course course)
        {
            course.Id = Guid.NewGuid();
            _context.Courses.Add(course);
        }

        public bool CourseExists(Guid courseId)
        {
            return _context.Courses.Any(a => a.Id == courseId);
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }


        public Course GetCourse(Guid courseId)
        {
            return _context.Courses.FirstOrDefault(a => a.Id == courseId);
        }

        public PagedList<Course> GetCourses(
            CoursesResourceParameters coursesResourceParameters)
        {
            //var collectionBeforePaging = _context.Courses
            //    .OrderBy(a => a.FirstName)
            //    .ThenBy(a => a.LastName).AsQueryable();

            var collectionBeforePaging =
                _context.Courses
                .ApplySort(coursesResourceParameters.OrderBy,
                _propertyMappingService.GetPropertyMapping<CourseDto, Course>());

            

            if (!string.IsNullOrEmpty(coursesResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = coursesResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Description.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Course>.Create(collectionBeforePaging,
                coursesResourceParameters.PageNumber,
                coursesResourceParameters.PageSize);
        }

        public IEnumerable<Course> GetCourses(IEnumerable<Guid> courseIds)
        {
            return _context.Courses.Where(a => courseIds.Contains(a.Id))
                .OrderBy(a => a.Name)
                .ToList();
        }

        public void UpdateCourse(Course course)
        {
            // no code in this implementation
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
