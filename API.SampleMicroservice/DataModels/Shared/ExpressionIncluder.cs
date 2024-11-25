using System.Linq.Expressions;

namespace API.SampleMicroservice.DataModels.Shared
{
    public class ExpressionIncluder<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, object>>[] Includes { get; set; } = null!;
    }
}
