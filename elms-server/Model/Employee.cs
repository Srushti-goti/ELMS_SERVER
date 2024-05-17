
using System.ComponentModel.DataAnnotations;

public class Employee
{
    [Key]
    public int EmployeeId { get; set; } 

    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime Birthdate { get; set; }

    public string Department { get; set; }

    public string Position { get; set; }

    public string PreviousLeave { get; set; }

    public string Email { get; set; }
    public string Password { get; set; }

    public string ContactNumber { get; set; }
    public bool IsActive { get; set; }
}
public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}