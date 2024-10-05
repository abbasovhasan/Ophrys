global using FluentValidation.AspNetCore;
global using Feedback_System.Dtos;
global using Feedback_System.Abstractions;
global using Feedback_System.Concretes;
global using Feedback_System.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Identity;
global using Feedback_System.Validations;  // Validator sınıflarını kullanmak için ekliyoruz
global using Microsoft.AspNetCore.Authentication.Cookies;
global using Feedback_System.Roles;
global using Feedback_System.Helpers;  // RoleInitializer kullanımı için ekliyoruz
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;