namespace BudgetTracker.Application.Models.User.Requests;

public record CreateUser
(
     string Firstname,
     string Lastname ,
     string Username ,
     string Email ,
     string Password 
    );