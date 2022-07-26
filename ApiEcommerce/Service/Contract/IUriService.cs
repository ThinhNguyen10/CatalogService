using ApiEcommerce.Filter;

namespace ApiEcommerce.Service.Contract
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
