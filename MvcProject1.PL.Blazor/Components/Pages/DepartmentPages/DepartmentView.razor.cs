using Microsoft.AspNetCore.Components;
using MvcProject1.BLL.RepositoryInterfaces;
using MvcProject1.DAL.Models;

namespace MvcProject1.PL.Blazor.Components.Pages.DepartmentPages
{
    public partial class DepartmentView : ComponentBase
    {
        [Inject]
        IDepartmentRepository DepartmentRepository {  get; set; }
        private IEnumerable<Department> Departments;

        protected override async Task OnInitializedAsync()
        {
            // Retrieve data asynchronously
            Departments = await Task.Run(() => DepartmentRepository.GetAll());
        }
    }
}
