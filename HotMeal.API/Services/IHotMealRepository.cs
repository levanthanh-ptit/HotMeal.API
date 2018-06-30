using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotMeal.API.Entities;
using HotMeal.API.Helpers;

namespace HotMeal.API.Services
{
    public interface IHotMealRepository
    {
        PagedList<Customer> GetCustomers(CustomersResourceParameters customersResourceParameters);
        Customer GetCustomer(Guid customerId);
        IEnumerable<Customer> GetCustomers(IEnumerable<Guid> customerIds);
        void AddCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        bool CustomerExists(Guid customerId);
        PagedList<Course> GetCourses(CoursesResourceParameters coursesResourceParameters);
        Course GetCourse(Guid courseId);
        IEnumerable<Course> GetCourses(IEnumerable<Guid> courseIds);
        void AddCourse(Course course);
        void DeleteCourse(Course course);
        void UpdateCourse(Course course);
        bool CourseExists(Guid courseId);
        bool Save();

    }
}
