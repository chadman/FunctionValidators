using FluentValidation;

namespace ValidateFunction.Models;
public class Person {
    public string FirstName { get; set; }

    public string LastName { get; set; }
}

public class PersonValidator : AbstractValidator<Person> {
    public PersonValidator() {
        RuleFor(f => f.FirstName).NotEmpty();
        RuleFor(f => f.LastName).NotEmpty();
    }
}
