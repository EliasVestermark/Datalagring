using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using PresentationDatalagringUppgift.Interfaces;

namespace PresentationDatalagringUppgift.Services;

public class MenuService : IMenuService
{
    private readonly IBookingService _bookingService;
    private readonly IProductService _productService;
    private readonly IValidationService _validationService;

    public MenuService(IBookingService bookingService, IValidationService validationService, IProductService productService)
    {
        _bookingService = bookingService;
        _productService = productService;
        _validationService = validationService;
    }

    public async Task ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            DisplayTitle("FIGGE FERRUMS FOODTRUCK, PRODUCT AND BOOKING MANAGER");
            Console.WriteLine("1. Add product");
            Console.WriteLine("2. Show all products");
            Console.WriteLine("3. Add booking");
            Console.WriteLine("4. Show all bookings");
            Console.WriteLine("x. Exit application");

            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await ShowAddProductMenu();
                    break;

                case "2":
                    await ShowAllProductsMenu();
                    break;

                case "3":
                    await ShowAddBookingMenu();
                    break;

                case "4":
                    await ShowAllBookingsMenu();
                    break;

                case "x":
                    ShowExitMenu();
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;

            }
        }
    }

    private async Task ShowAddProductMenu()
    {
        DisplayTitle("ADD PRODUCT");

        Console.Write("Name: ");
        string name = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Price: ");
        decimal price = _validationService.StringToNumber(Console.ReadLine()!);

        var resultList = await _productService.GetAllIngridients();
        int index = 1;
        ICollection<Ingridient> ingridients = [];


        foreach (var item in resultList)
        {
            Console.WriteLine($"{index}. Name: {item.Name}");
            index++;
        }

        Console.WriteLine("Choose one ingridient at a time, press any non-digit key when you are done");
        Console.Write("First ingridient (can't be empty): ");

        string option = Console.ReadLine()!;

        while (ingridients.Count < 1)
        {
            while (option.All(char.IsNumber) && !option.IsNullOrEmpty())
            {
                if (int.TryParse(option, out int optionInt) && optionInt <= index - 1 && optionInt >= 1)
                {
                    ingridients.Add(resultList.ElementAt(optionInt - 1));

                    Console.Write("Next ingridient: ");
                    option = Console.ReadLine()!;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                    option = Console.ReadLine()!;
                }
            }

            if (ingridients.Count < 1)
            {
                Console.WriteLine("The product must contain at least one ingridient, try again");
                option = Console.ReadLine()!;
            }
        }

        Console.WriteLine();

        Console.WriteLine("Choose a category:");
        Console.WriteLine("1. Side");
        Console.WriteLine("2. Main");
        Console.WriteLine("3. Beverage");

        var categoryOption = Console.ReadLine();
        int categoryId = 0;

        while (categoryId == 0)
        {
            switch (categoryOption)
            {
                case "1":
                    categoryId = 1;
                    break;

                case "2":
                    categoryId = 2;
                    break;

                case "3":
                    categoryId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    categoryOption = Console.ReadLine()!;
                    break;
            }
        }

        var result = await _productService.CreateProduct(new CreateProductDto(name, price, ingridients, categoryId));

        DisplayTitle("ADD PRODUCT");

        switch (result)
        {
            case ServiceStatus.SUCCESS:
                Console.WriteLine("The product has been added successfully");
                Console.WriteLine("Press any button to continue");
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine("A product with the same name already exists");
                Console.WriteLine("Press any button to continue");
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to add the product, please try again");
                Console.WriteLine("Press any button to continue");
                break;
        }

        Console.ReadKey();
    }

    private async Task ShowAllProductsMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("SHOW ALL PRODUCTS");

            var productList = await _productService.GetAllProducts();
            Console.Clear();

            if (productList.Count() == 0)
            {
                Console.WriteLine("No products has been added, the list is empty");
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
                run = false;
            }
            else
            {
                int index = 1;

                Console.WriteLine($"View product details or press (x) to go back to the main menu");
                Console.WriteLine();

                foreach (var product in productList)
                {
                    Console.WriteLine($"{index}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Price: {Math.Round(product.Price, 2)}");
                    Console.WriteLine($"Category: {product.Category}");
                    Console.WriteLine();
                    index++;
                }

                var option = Console.ReadLine();

                if (option!.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                {
                    run = false;
                }
                else if (int.TryParse(option, out int optionInt) && optionInt <= index - 1 && optionInt >= 1)
                {
                    await ShowProductInformationMenu(optionInt);
                }
                else
                {
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task ShowProductInformationMenu(int option)
    {
        DisplayTitle("PRODUCT INFORMATION");

        bool run = true;

        while (run)
        {
            var productList = await _productService.GetAllProducts();
            var product = productList.ElementAt(option - 1);

            DisplayTitle("PRODUCT OPTIONS");

            Console.WriteLine($"Press (r) to remove product, (u) to update client or (x) to go back to the list");
            Console.WriteLine();
            Console.WriteLine($"Name: {product.Name}");
            Console.WriteLine($"Price: {Math.Round(product.Price, 2)}");
            Console.WriteLine($"Category: {product.Category}");
            Console.WriteLine("Ingridients:");
            foreach (var item in product.Ingridients)
            {
                Console.WriteLine($" * {item.Name}");
            }

            var menuOption = Console.ReadLine();

            switch (menuOption)
            {
                case "r":
                    await ShowRemoveProductMenu($"{product.Name}");
                    run = false;
                    break;

                case "u":
                    await ShowUpdateProductMenu(product);
                    break;

                case "x":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ShowRemoveProductMenu(string name)
    {
        DisplayTitle("REMOVE PRODUCT");
        Console.WriteLine($"Are you sure you want to remove this product from the contact list? Press (y) to confirm or any other key to go back");

        var option = Console.ReadLine();

        if (option!.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {
            var result = await _productService.DeleteProduct(name);
            Console.Clear();
            DisplayTitle("REMOVE PRODUCT");

            switch (result)
            {
                case ServiceStatus.DELETED:
                    Console.WriteLine($"The product has been removed from the product database");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case ServiceStatus.FAILED:
                    Console.WriteLine("An error occured when trying to remove the product, please try again");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case ServiceStatus.NOT_FOUND:
                    Console.WriteLine($"The product does not seem to exist");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ShowUpdateProductMenu(IProductDto product)
    {
        DisplayTitle("UPDATE PRODUCT INFORMATION");

        Console.Write("Name: ");
        string newName = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Price: ");
        decimal newPrice = _validationService.StringToNumber(Console.ReadLine()!);

        var resultList = await _productService.GetAllIngridients();
        int index = 1;
        ICollection<Ingridient> newIngridients = [];


        foreach (var item in resultList)
        {
            Console.WriteLine($"{index}. Name: {item.Name}");
            index++;
        }

        Console.WriteLine("Choose one ingridient at a time, press any non-digit key when you are done");
        Console.Write("First ingridient (can't be empty): ");

        string option = Console.ReadLine()!;

        while (newIngridients.Count < 1)
        {
            while (option.All(char.IsNumber) && !option.IsNullOrEmpty())
            {
                if (int.TryParse(option, out int optionInt) && optionInt <= index - 1 && optionInt >= 1)
                {
                    newIngridients.Add(resultList.ElementAt(optionInt - 1));

                    Console.Write("Next ingridient: ");
                    option = Console.ReadLine()!;
                }
                else
                {
                    Console.WriteLine("Invalid option, try again");
                    option = Console.ReadLine()!;
                }
            }

            if (newIngridients.Count < 1)
            {
                Console.WriteLine("The product must contain at least one ingridient, try again");
                option = Console.ReadLine()!;
            }
        }

        Console.WriteLine();

        Console.WriteLine("Choose a category:");
        Console.WriteLine("1. Side");
        Console.WriteLine("2. Main");
        Console.WriteLine("3. Beverage");

        var categoryOption = Console.ReadLine();
        int newCategoryId = 0;

        while (newCategoryId == 0)
        {
            switch (categoryOption)
            {
                case "1":
                    newCategoryId = 1;
                    break;

                case "2":
                    newCategoryId = 2;
                    break;

                case "3":
                    newCategoryId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    categoryOption = Console.ReadLine()!;
                    break;
            }
        }

        var result = await _productService.UpdateProduct(product.Name, newName, newPrice, newIngridients, newCategoryId);

        switch (result)
        {
            case ServiceStatus.UPDATED:
                Console.WriteLine("The product information has been updates successfully");
                Console.WriteLine("Press any key to go back to the product");
                Console.ReadKey();
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to update the product, please try again");
                Console.WriteLine("Press any key to go back to the product");
                Console.ReadKey();
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"A product with the same name ({newName}) already exist, try again with a different name");
                Console.WriteLine("Press any key to go back to the product");
                Console.ReadKey();
                break;
        }
    }    

    private async Task ShowAddBookingMenu()
    {
        DisplayTitle("ADD BOOKING");

        Console.Write("Client First Name: ");
        string firstName = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Client Last Name: ");
        string lastName = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Client Phone Number: ");
        string phoneNumber = _validationService.NumberValidation(Console.ReadLine()!);

        Console.Write("Client Email: ");
        string email = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Booking Address: ");
        string address = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Booking Postal Code: ");
        string postalCode = _validationService.NumberValidation(Console.ReadLine()!);

        Console.Write("Booking City: ");
        string city = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Date (example: 2024-12-31): ");
        string date = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        while (date.Length != 10)
        {
            Console.Write("Please enter a valid date: ");
            date = _validationService.EmptyStringInputValidation(Console.ReadLine()!);
        }

        Console.WriteLine("Choose the requested time:");
        Console.WriteLine("1. 0800 - 1100");
        Console.WriteLine("2. 1200 - 1500");
        Console.WriteLine("3. 1700 - 2100");

        var menuOption1 = Console.ReadLine();
        int timeId = 0;

        while (timeId == 0)
        {
            switch (menuOption1)
            {
                case "1":
                    timeId = 1;
                    break;

                case "2":
                    timeId = 2;
                    break;

                case "3":
                    timeId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    menuOption1 = Console.ReadLine()!;
                    break;
            }
        }

        Console.WriteLine("Choose the requested amount of participants:");
        Console.WriteLine("1. 1 - 9");
        Console.WriteLine("2. 10 - 19");
        Console.WriteLine("3. 20+");

        var menuOption2 = Console.ReadLine();
        int participantsId = 0;

        while (participantsId == 0)
        {
            switch (menuOption2)
            {
                case "1":
                    participantsId = 1;
                    break;

                case "2":
                    participantsId = 2;
                    break;

                case "3":
                    participantsId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    menuOption2 = Console.ReadLine()!;
                    break;
            }
        }

        //1 corresponds to status "booked" in the database
        int statusId = 1;

        var result = await _bookingService.CreateBooking(new CreateBookingDto(firstName, lastName, phoneNumber, email, address, postalCode, city, date, statusId, participantsId, timeId));

        Console.Clear();

        DisplayTitle("ADD BOOKING");

        switch (result)
        {
            case ServiceStatus.SUCCESS:
                Console.WriteLine("The booking has been added to the database");
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine("A booking with the same date already exists, please try again with a different date");
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to add the booking to the database, please try again");
                break;
        }

        Console.ReadKey();
    }

    private async Task ShowAllBookingsMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("SHOW ALL BOOKINGS");

            var bookingList = await _bookingService.GetAllBookings();
            Console.Clear();

            if (bookingList == null)
            {
                Console.WriteLine("No bookings has been added, the list is empty");
                Console.WriteLine("Press any button to continue");
                Console.ReadKey();
                run = false;
            }
            else
            {
                int index = 1;

                Console.WriteLine($"View booking details or press (x) to go back to the main menu");
                Console.WriteLine();

                foreach (var booking in bookingList)
                {
                    Console.WriteLine($"{index}. Name: {booking.FirstName} {booking.LastName}, Date: {booking.Date}");
                    index++;
                }

                var option = Console.ReadLine();

                if (option!.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                {
                    run = false;
                }
                else if (int.TryParse(option, out int optionInt) && optionInt <= index - 1 && optionInt >= 1)
                {
                    await ShowBookingInformationMenu(optionInt);
                }
                else
                {
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                }
            }
        }
    }

    private async Task ShowBookingInformationMenu(int option)
    {
        DisplayTitle("BOOKING INFORMATION");

        bool run = true;

        while (run)
        {
            var bookingList = await _bookingService.GetAllBookings();
            var booking = bookingList.ElementAt(option - 1);

            DisplayTitle("BOOKING OPTIONS");

            Console.WriteLine($"Press (r) to remove booking, (1-3) to update client, location or booking information or (x) to go back to the list");
            Console.WriteLine();
            Console.WriteLine("1.");
            Console.WriteLine($"Name: {booking.FirstName} {booking.LastName}");
            Console.WriteLine($"Phone Number: {booking.PhoneNumber}");
            Console.WriteLine($"Email: {booking.Email}");
            Console.WriteLine();
            Console.WriteLine("2.");
            Console.WriteLine($"Address: {booking.Address}");
            Console.WriteLine($"       : {booking.PostalCode} {booking.City}");
            Console.WriteLine();
            Console.WriteLine("3.");
            Console.WriteLine($"Participants: {booking.Participants}");
            Console.WriteLine($"Date: {booking.Date}");
            Console.WriteLine($"Time: {booking.Time}");
            Console.WriteLine($"Booking Status: {booking.Status}");

            var menuOption = Console.ReadLine();

            switch (menuOption)
            {
                case "r":
                    await ShowRemoveBookingMenu($"{booking.FirstName} {booking.LastName}", booking.Date);
                    run = false;
                    break;

                case "1":
                    await ShowUpdateClientMenu(booking);
                    break;

                case "2":
                    await ShowUpdateLocationMenu(booking);
                    break;

                case "3":
                    await ShowUpdateBookingMenu(booking);
                    break;

                case "x":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private async Task ShowRemoveBookingMenu(string name, string date)
    {
        DisplayTitle("REMOVE CONTACT");
        Console.WriteLine($"Are you sure you want to remove this booking from the contact list? Press (y) to confirm or any other key to go back");

        var option = Console.ReadLine();

        if (option!.Equals("y", StringComparison.CurrentCultureIgnoreCase))
        {
            var result = await _bookingService.DeleteBooking(date);
            Console.Clear();
            DisplayTitle("REMOVE CONTACT");

            switch (result)
            {
                case ServiceStatus.DELETED:
                    Console.WriteLine($"The booking has been removed from the booking database");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case ServiceStatus.FAILED:
                    Console.WriteLine("An error occured when trying to remove the booking, please try again");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;

                case ServiceStatus.NOT_FOUND:
                    Console.WriteLine($"The booking does not seem to exist");
                    Console.WriteLine("Press any key to go back to the list");
                    Console.ReadKey();
                    break;
            }
        }
    }

    public async Task ShowUpdateClientMenu(IBookingDto booking)
    {
        DisplayTitle("UPDATE CLIENT INFORMATION");

        Console.Write("First Name: ");
        string newFirstName = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Last Name: ");
        string newLastName = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Phone Number: ");
        string newPhoneNumber = _validationService.NumberValidation(Console.ReadLine()!);

        Console.Write("Email: ");
        string newEmail = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        var result = await _bookingService.UpdateClient(new ClientEntity
        {
            FirstName = newFirstName,
            LastName = newLastName,
            PhoneNumber = newPhoneNumber,
            Email = newEmail
        }, newEmail, booking.Email);

        switch (result)
        {
            case ServiceStatus.UPDATED:
                Console.WriteLine("The client information has been updates successfully");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to update the contact, please try again");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"A client with the same email ({newEmail}) already exist, try again with a different email");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;
        }
    }

    public async Task ShowUpdateLocationMenu(IBookingDto booking)
    {
        DisplayTitle("UPDATE CLIENT INFORMATION");

        Console.Write("Address: ");
        string newAddress = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        Console.Write("Postal Code: ");
        string newPostalCode = _validationService.NumberValidation(Console.ReadLine()!);

        Console.Write("City: ");
        string newCity = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        var result = await _bookingService.UpdateLocation(new LocationEntity
        {
            Address = newAddress,
            PostalCode = newPostalCode,
            City = newCity
        }, newAddress, newPostalCode, booking.Address, booking.PostalCode);

        switch (result)
        {
            case ServiceStatus.UPDATED:
                Console.WriteLine("The location information has been updates successfully");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to update the location, please try again");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"A location with the same address and postal code ({newAddress}, {newPostalCode}) already exist, try again with a different address");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;
        }
    }

    public async Task ShowUpdateBookingMenu(IBookingDto booking)
    {
        DisplayTitle("UPDATE BOOKING INFORMATION");

        Console.Write("Date (example: 2024-12-31): ");
        string newDate = _validationService.EmptyStringInputValidation(Console.ReadLine()!);

        while (newDate.Length != 10)
        {
            Console.Write("Please enter a valid date: ");
            newDate = _validationService.EmptyStringInputValidation(Console.ReadLine()!);
        }

        Console.WriteLine("Choose the requested time:");
        Console.WriteLine("1. 0800 - 1100");
        Console.WriteLine("2. 1200 - 1500");
        Console.WriteLine("3. 1700 - 2100");

        var menuOption1 = Console.ReadLine();
        int newTimeId = 0;

        while (newTimeId == 0)
        {
            switch (menuOption1)
            {
                case "1":
                    newTimeId = 1;
                    break;

                case "2":
                    newTimeId = 2;
                    break;

                case "3":
                    newTimeId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    menuOption1 = Console.ReadLine()!;
                    break;
            }
        }

        Console.WriteLine("Choose the requested amount of participants:");
        Console.WriteLine("1. 1 - 9");
        Console.WriteLine("2. 10 - 19");
        Console.WriteLine("3. 20+");

        var menuOption2 = Console.ReadLine();
        int newParticipantsId = 0;

        while (newParticipantsId == 0)
        {
            switch (menuOption2)
            {
                case "1":
                    newParticipantsId = 1;
                    break;

                case "2":
                    newParticipantsId = 2;
                    break;

                case "3":
                    newParticipantsId = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    menuOption2 = Console.ReadLine()!;
                    break;
            }
        }

        Console.WriteLine("Change the booking status:");
        Console.WriteLine("1. Booked");
        Console.WriteLine("2. Canceled");
        Console.WriteLine("3. Completed");

        var menuOption3 = Console.ReadLine();
        int newStatusID = 0;

        while (newStatusID == 0)
        {
            switch (menuOption3)
            {
                case "1":
                    newStatusID = 1;
                    break;

                case "2":
                    newStatusID = 2;
                    break;

                case "3":
                    newStatusID = 3;
                    break;

                default:
                    Console.WriteLine("Invalid option, try again");
                    menuOption3 = Console.ReadLine()!;
                    break;
            }
        }

        var result = await _bookingService.UpdateBooking(newDate, booking.Date, booking.Email, booking.Address, booking.PostalCode, newStatusID, newParticipantsId, newTimeId);

        switch (result)
        {
            case ServiceStatus.UPDATED:
                Console.WriteLine("The booking information has been updates successfully");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.FAILED:
                Console.WriteLine("An error occured when trying to update the booking, please try again");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;

            case ServiceStatus.ALREADY_EXISTS:
                Console.WriteLine($"A booking with the same date ({newDate}) already exist, try again with a different date");
                Console.WriteLine("Press any key to go back to the booking");
                Console.ReadKey();
                break;
        }
    }

    private void ShowExitMenu()
    {
        bool run = true;

        while (run)
        {
            DisplayTitle("EXIT APPLICATION");

            Console.WriteLine("Are you sure you want to exit? (y/n)");
            var option = Console.ReadLine();

            switch (option)
            {
                case "y":
                    Environment.Exit(0);
                    break;

                case "n":
                    run = false;
                    break;

                default:
                    Console.WriteLine("Invalid option, press any key then try again");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void DisplayTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"## {title} ##");
        Console.WriteLine();
    }
}
