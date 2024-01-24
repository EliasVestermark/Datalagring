using PresentationDatalagringUppgift.Interfaces;

namespace PresentationDatalagringUppgift.Services;

public class ValidationService : IValidationService
{
    public string NumberValidation(string number)
    {
        while (string.IsNullOrEmpty(number))
        {
            Console.WriteLine("This field can not be blank, please provide a valid input");
            number = Console.ReadLine()!;
        }
        while (!number.All(char.IsDigit))
        {
            Console.WriteLine("This field may only contain digits, please provide a valid number");
            number = Console.ReadLine()!;
        }

        return number;
    }

    public string EmptyStringInputValidation(string userInput)
    {
        while (string.IsNullOrEmpty(userInput))
        {
            Console.WriteLine("This field can not be blank, please provide a valid input");
            userInput = Console.ReadLine()!;
        }

        return userInput;
    }

    public decimal StringToNumber(string number)
    {
        decimal price;

        while (string.IsNullOrEmpty(number))
        {
            Console.WriteLine("This field can not be blank, please provide a valid input");
            number = Console.ReadLine()!;
        }
        while (!decimal.TryParse(number, out price))
        {
            Console.WriteLine("This field may only contain digits and (,). Please provide a valid price");
            number = Console.ReadLine()!;
        }

        return Math.Round(price, 2);
    }
}
