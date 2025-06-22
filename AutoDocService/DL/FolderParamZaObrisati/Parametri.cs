namespace AutoDocService.DL.FolderParamZaObrisati
{

    public class Parametri
    {
        public string TestParams { get; set; } = "test";
        public Person Person { get; set; } = new Person();
        public Address address{ get; set; } = new Address();
        public Job job { get; set; } = new Job();
    }

    public class Person
    {
        public string Name { get; set; } = "John Doe";
        public int Age { get; set; } = 30;
        public string Gender { get; set; } = "Male";
    }

    public class Address
    {
        public string Street { get; set; } = "123 Main St";
        public string City { get; set; } = "Anytown";
        public string Country { get; set; } = "USA";
    }

    public class Job
    {
        public string Title { get; set; } = "Software Developer";
        public string Company { get; set; } = "Tech Corp";
        public decimal Salary { get; set; } = 60000.00m;
    }

}
