using AutoMapper;
using HeadlessCMS.ApplicationCore.Core.Interfaces.Services;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using HeadlessCMS.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    public abstract class GenericController<TEntity, TEntityDto> : ControllerBase
        where TEntity : BaseEntity
        where TEntityDto : BaseEntityDto
    {
        protected DbSet<TEntity> genericDbSet;
        protected ApplicationDbContext applicationDbContext;
        protected IBaseEntityRepository baseEntityRepository;
        protected IMapper _mapper;
        protected IPermissionService _permissionService;

        protected GenericController(ApplicationDbContext applicationDbContext,
                                    IBaseEntityRepository baseEntityRepository,
                                    IMapper mapper,
                                    IPermissionService permissionService)
        {
            this.applicationDbContext = applicationDbContext;
            this.baseEntityRepository = baseEntityRepository;
            applicationDbContext.Database.EnsureCreated();
            genericDbSet = applicationDbContext.Set<TEntity>();
            _mapper = mapper;
            _permissionService = permissionService;
        }

        // GET: api/<GenericController>
        [HttpGet]
        [EnableQuery]
        public virtual IEnumerable<TEntityDto> Get()
        {
            var mappedSet = _mapper.Map<TEntityDto[]>(genericDbSet.ToArray());
            return mappedSet;
        }

        // POST api/<GenericController>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public virtual async Task<TEntityDto> Post([FromBody] TEntityDto value)
        {
            var entity = _mapper.Map<TEntity>(value);
            entity = baseEntityRepository.Add(entity, User);
            await applicationDbContext.SaveChangesAsync();

            return _mapper.Map<TEntityDto>(entity);
        }

        // PUT api/<GenericController>/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public virtual async Task<IActionResult> PutAsync([FromBody] TEntityDto value)
        {
            try
            {
                _permissionService.ValidateOwnership<TEntity>(User, value.Id.ToString());
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }

            var entity = _mapper.Map<TEntity>(value);
            entity = baseEntityRepository.Update(entity, User);
            await applicationDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<TEntityDto>(entity));
        }

        // DELETE api/<GenericController>/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                _permissionService.ValidateOwnership<TEntity>(User, id);
            }
            catch(Exception e)
            {
                return Unauthorized(e.Message);
            }

            var entity = genericDbSet.FirstOrDefault(x => x.Id.ToString() == id);
            if (entity != null)
            {
                genericDbSet.Remove(entity);
                await applicationDbContext.SaveChangesAsync();
            }

            return Ok();
        }
    }
}