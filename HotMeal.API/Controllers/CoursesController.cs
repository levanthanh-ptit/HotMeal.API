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
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private IHotMealRepository _hotMealRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public CoursesController(IHotMealRepository hotMealRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _hotMealRepository = hotMealRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetCourses")]
        public IActionResult GetCourses(CoursesResourceParameters coursesResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<CourseDto, Course>
               (coursesResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            //if (!_typeHelperService.TypeHasProperties<CourseDto>
            //    (coursesResourceParameters.Fields))
            //{
            //    return BadRequest();
            //}

            var coursesFromRepo = _hotMealRepository.GetCourses(coursesResourceParameters);

            var previousPageLink = coursesFromRepo.HasPrevious ?
                CreateCoursesResourceUri(coursesResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = coursesFromRepo.HasNext ?
                CreateCoursesResourceUri(coursesResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = coursesFromRepo.TotalCount,
                pageSize = coursesFromRepo.PageSize,
                currentPage = coursesFromRepo.CurrentPage,
                totalPages = coursesFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var courses = Mapper.Map<IEnumerable<CourseDto>>(coursesFromRepo);
            return Ok(courses);
        }

        private string CreateCoursesResourceUri(
            CoursesResourceParameters coursesResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetCourses",
                      new
                      {
                          //fields = coursesResourceParameters.Fields,
                          orderBy = coursesResourceParameters.OrderBy,
                          searchQuery = coursesResourceParameters.SearchQuery,
                          pageNumber = coursesResourceParameters.PageNumber - 1,
                          pageSize = coursesResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetCourses",
                      new
                      {
                          //fields = coursesResourceParameters.Fields,
                          orderBy = coursesResourceParameters.OrderBy,
                          searchQuery = coursesResourceParameters.SearchQuery,
                          pageNumber = coursesResourceParameters.PageNumber + 1,
                          pageSize = coursesResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("GetCourses",
                    new
                    {
                        //fields = coursesResourceParameters.Fields,
                        orderBy = coursesResourceParameters.OrderBy,
                        searchQuery = coursesResourceParameters.SearchQuery,
                        pageNumber = coursesResourceParameters.PageNumber,
                        pageSize = coursesResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetCourse")]
        public IActionResult GetCourse(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<CourseDto>
              (fields))
            {
                return BadRequest();
            }

            var courseFromRepo = _hotMealRepository.GetCourse(id);

            if (courseFromRepo == null)
            {
                return NotFound();
            }

            var course = Mapper.Map<CourseDto>(courseFromRepo);
            return Ok(course);
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody] CourseForCreationDto course)
        {
            if (course == null)
            {
                return BadRequest();
            }

            var courseEntity = Mapper.Map<Course>(course);

            _hotMealRepository.AddCourse(courseEntity);

            if (!_hotMealRepository.Save())
            {
                throw new Exception("Creating an course failed on save.");
                // return StatusCode(500, "A problem happened with handling your request.");
            }

            var courseToReturn = Mapper.Map<CourseDto>(courseEntity);

            return CreatedAtRoute("GetCourse",
                new { id = courseToReturn.Id },
                courseToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockCourseCreation(Guid id)
        {
            if (_hotMealRepository.CourseExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(Guid id)
        {
            var courseFromRepo = _hotMealRepository.GetCourse(id);
            if (courseFromRepo == null)
            {
                return NotFound();
            }

            _hotMealRepository.DeleteCourse(courseFromRepo);

            if (!_hotMealRepository.Save())
            {
                throw new Exception($"Deleting course {id} failed on save.");
            }

            return NoContent();
        }

    }
}
