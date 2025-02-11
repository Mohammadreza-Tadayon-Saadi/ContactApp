using AutoMapper.QueryableExtensions;
using Application.CustomMappings.Configurations;

namespace Application.DTOs.Common;

public abstract class BaseDto<TSourceDto, TDestinationDto> : IHaveCustomMapping
    where TSourceDto : class
    where TDestinationDto : class
{
    public TDestinationDto ToEntity() =>
        AutoMapperConfiguration.Mapper.Map<TDestinationDto>(CastToDerivedClass(this));

    public TDestinationDto ToEntity(TDestinationDto entity) =>
        AutoMapperConfiguration.Mapper.Map(CastToDerivedClass(this), entity);

    public static TSourceDto FromEntity(TDestinationDto entity) =>
        AutoMapperConfiguration.Mapper.Map<TSourceDto>(entity);

    protected TSourceDto CastToDerivedClass(BaseDto<TSourceDto, TDestinationDto> baseInstance) =>
        AutoMapperConfiguration.Mapper.Map<TSourceDto>(baseInstance);

    public static IQueryable<TSourceDto> ProjectTo(IQueryable<TDestinationDto> queryable)
        => queryable.ProjectTo<TSourceDto>(AutoMapperConfiguration.Mapper.ConfigurationProvider);

    public void CreateMapping(Profile profile)
    {
        var dtoType = typeof(TSourceDto);
        var destinationType = typeof(TDestinationDto);

        var mappingExpression = profile.CreateMap<TSourceDto, TDestinationDto>();
        if (HasConstructorParameters<TDestinationDto>())
        {
            var constructorParameters = destinationType.GetConstructors()
                    .Where(d => d.GetParameters().Where(p => !p.HasDefaultValue).Any())
                    .First()
                    .GetParameters()
                    .Where(p => !p.HasDefaultValue)
                    .ToList();

            var constructorParameterNames = constructorParameters.Select(cp => cp.Name.ToCapitalLetter()).ToList();
            var dtoPropertiesCount = dtoType.GetProperties()
                    .Where(s => constructorParameterNames.Contains(s.Name))
                    .Count();

            if (dtoPropertiesCount == constructorParameters.Count)
            {
                foreach (var parameter in constructorParameters)
                    mappingExpression.ForCtorParam(parameter.Name, opt => opt.MapFrom(parameter.Name.ToCapitalLetter()));

                var otherProperties = destinationType.GetProperties()
                    .Where(p => !constructorParameterNames.Contains(p.Name))
                    .ToList();

                foreach (var property in otherProperties)
                    if (dtoType.GetProperty(property.Name) is null)
                        mappingExpression.ForMember(property.Name, opt => opt.Ignore());
            }
            else
                mappingExpression.ForAllMembers(opt => opt.Ignore());
        }
        else
        {
            //Ignore any property of source that dose not contains in destination 
            foreach (var property in destinationType.GetProperties())
                if (dtoType.GetProperty(property.Name) is null)
                    mappingExpression.ForMember(property.Name, opt => opt.Ignore());
        }

        CustomMappings(mappingExpression.ReverseMap());
    }

    private static bool HasConstructorParameters<TDto>() where TDto : class
    {
        try
        {
            var dtoType = typeof(TDto);
            var constructors = dtoType.GetConstructors();

            // Check if any constructor has parameters
            return constructors.Any(ctor => ctor.GetParameters().Length > 0);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public virtual void CustomMappings(IMappingExpression<TDestinationDto, TSourceDto> mapping)
    {
    }
}