using System.ComponentModel.DataAnnotations;
namespace AutoDocFront.Models
{
    public class Parametri
    {
        [Display(Name = "Test Parameter")]
        public string TestParams { get; set; } = "test";

        public Person Person { get; set; } = new Person();
        public Address Address { get; set; } = new Address();
        public Job Job { get; set; } = new Job();
    }

    public class Person
    {
        [Display(Name = "Naziv")]
        public string Name { get; set; } = "John Doe";

        [Display(Name = "Person Age")]
        public int Age { get; set; } = 30;

        [Display(Name = "Person Gender")]
        public string Gender { get; set; } = "Male";
    }

    public class Address
    {
        [Display(Name = "Address Street")]
        public string Street { get; set; } = "123 Main St";

        [Display(Name = "Address City")]
        public string City { get; set; } = "Anytown";

        [Display(Name = "Address Country")]
        public string Country { get; set; } = "USA";
    }

    public class Job
    {
        [Display(Name = "Job Title")]
        public string Title { get; set; } = "Software Developer";

        [Display(Name = "Job Company")]
        public string Company { get; set; } = "Tech Corp";

        [Display(Name = "Job Salary")]
        public decimal Salary { get; set; } = 60000.00m;
    }
}