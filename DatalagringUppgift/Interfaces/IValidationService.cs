namespace PresentationDatalagringUppgift.Interfaces
{
    public interface IValidationService
    {
        string EmptyStringInputValidation(string userInput);
        string NumberValidation(string number);
        decimal StringToNumber(string number);
    }
}