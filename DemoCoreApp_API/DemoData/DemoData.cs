using DemoCoreApp_API.Models.Dto;

namespace DemoCoreApp_API.DemoData
{
    public static class DemoData
    {
        public static List<DemoFDTO> DemoDataList = new List<DemoFDTO>
            {
                new DemoFDTO{Id=1,Name="Anas Nafees",EmployeeNumber=101,sallary=1000},
                new DemoFDTO{Id=2,Name="Pradeep Singh",EmployeeNumber=102,sallary=500}
            };
    }
}
