﻿namespace Messenger.Application.DataTransferObjects.Auth
{
    public class RegisterDto
    {
        public string FirstName { get; set; }  // Foydalanuvchi ismi
        public string LastName { get; set; }  // Ixtiyoriy. Foydalanuvchi familiyasi
        public string Username { get; set; }  // Ixtiyoriy. Foydalanuvchi foydalanuvchi nomi
        public string PhoneNumber { get; set; }  // Foydalanuvchi telefon raqami
        public string Email { get; set; }  // Ixtiyoriy. Foydalanuvchi elektron pochta manzili
        public string Password { get; set; }  // Foydalanuvchi paroli
    }
}
