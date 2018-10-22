using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GeoAPI.Geometries;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.OData.NetTopology.Mapping;
using Microsoft.Spatial;

namespace Microsoft.AspNetCore.OData.NetTopology
{
    /// <summary>
    ///     Property mappings betwen OData Microsoft.Spatial types and NTS types
    /// </summary>
    public static class GeographyMappingExtensions
    {
        static GeographyMappingExtensions()
        {
            IgnoreMethod = typeof(GeographyMappingExtensions).GetMethod(
                nameof(Ignore),
                BindingFlags.NonPublic | BindingFlags.Static);
        }

        private static MethodInfo IgnoreMethod { get; }

        /// <summary>
        ///     Map an OData GeographyPoint to an EF NTS IPoint
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="builder">The ODataModelBuilder.</param>
        /// <param name="odataProperty">The Microsoft.Spatial GeographyPoint lambda.</param>
        /// <param name="ntsProperty">The NTS Point lambda.</param>
        /// <returns>The ODataModelBuilder.</returns>
        public static ODataModelBuilder MapSpatial<T>(
            this ODataModelBuilder builder,
            Expression<Func<T, GeographyPoint>> odataProperty,
            Expression<Func<T, IPoint>> ntsProperty)
            where T : class
        {
            var odataPropertyInfo = PropertySelectorVisitor.GetSelectedProperty(odataProperty);
            var ntsPropertyInfo = PropertySelectorVisitor.GetSelectedProperty(ntsProperty);
            return builder.MapSpatial(odataPropertyInfo, ntsPropertyInfo);
        }

        /// <summary>
        ///     Map an OData GeographyPolygon to an EF NTS IPolygon
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="builder">The ODataModelBuilder.</param>
        /// <param name="odataProperty">The Microsoft.Spatial GeographyPolygon lambda.</param>
        /// <param name="ntsProperty">The NTS Polygon lambda.</param>
        /// <returns>The ODataModelBuilder.</returns>
        public static ODataModelBuilder MapSpatial<T>(
            this ODataModelBuilder builder,
            Expression<Func<T, GeographyPolygon>> odataProperty,
            Expression<Func<T, IPolygon>> ntsProperty)
            where T : class
        {
            var odataPropertyInfo = PropertySelectorVisitor.GetSelectedProperty(odataProperty);
            var ntsPropertyInfo = PropertySelectorVisitor.GetSelectedProperty(ntsProperty);
            return builder.MapSpatial(odataPropertyInfo, ntsPropertyInfo);
        }

        /// <summary>
        ///     Map an OData GeographyPoint to an EF NTS IPoint
        /// </summary>
        /// <param name="builder">The ODataModelBuilder.</param>
        /// <param name="odataPropertyInfo">The Microsoft.Spatial property.</param>
        /// <param name="ntsPropertyInfo">The NTS property.</param>
        /// <returns>The ODataModelBuilder.</returns>
        public static ODataModelBuilder MapSpatial(
            this ODataModelBuilder builder,
            PropertyInfo odataPropertyInfo,
            PropertyInfo ntsPropertyInfo)
        {
            GeographyMapping.Instance.MapPoint(odataPropertyInfo, ntsPropertyInfo);
            if (odataPropertyInfo.DeclaringType == null)
            {
                throw new ArgumentException(nameof(odataPropertyInfo));
            }

            if (ntsPropertyInfo.DeclaringType == null)
            {
                throw new ArgumentException(nameof(odataPropertyInfo));
            }

            var declaringType = odataPropertyInfo.DeclaringType.IsAssignableFrom(ntsPropertyInfo.DeclaringType)
                ? ntsPropertyInfo.DeclaringType
                : odataPropertyInfo.DeclaringType;

            var configuration = builder.StructuralTypes.First(t => t.ClrType == declaringType);
            var primitivePropertyConfiguration = configuration.AddProperty(odataPropertyInfo);
            primitivePropertyConfiguration.Name = ntsPropertyInfo.Name;
            var param = Expression.Parameter(ntsPropertyInfo.DeclaringType);
            var member = Expression.Property(param, ntsPropertyInfo.Name);
            var lambda = Expression.Lambda(member, param);
            IgnoreMethod.MakeGenericMethod(declaringType, ntsPropertyInfo.PropertyType)
                .Invoke(null, new object[] {builder, lambda});
            return builder;
        }

        private static void Ignore<T, TNtsPoint>(ODataModelBuilder builder,
            Expression lambda)
            where T : class
        {
            builder.EntityType<T>().Ignore((Expression<Func<T, TNtsPoint>>) lambda);
        }
    }
}