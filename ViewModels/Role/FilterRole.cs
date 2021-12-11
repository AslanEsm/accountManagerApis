using common.Utilities.Paging;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.Role
{
    public class FilterRole : PaginationDto
    {
        public string Name { get; set; }
        public SortOrderResult? SortOrder { get; set; }
    }

    public enum SortOrderResult
    {
        [Display(Name = "اسم")]
        Name,

        [Display(Name = "اسم_نزولی")]
        Name_Desc
    }
}